using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarnivalTimer : MonoBehaviour {

	public Image loading;
	public Text timeText;
	public int Minutes;
	public int Seconds;
	public AudioSource ClockTick;

	private int totalSeconds = 0;
	private int TOTAL_SECONDS = 0;
	private int minutes;
	private int sec;

	// Auxiliary sound tick logic
	int updatedSecond = 10;

	private bool stopped = false;

	void Start ()
	{
		minutes = Minutes;
		sec = Seconds;
	}

	void Update ()
	{
		if (sec == 0 && minutes == 0 && !stopped) {
			timeText.text = "Game \nOver!";
			timeText.color = Color.red;
			CarnivalManager.Instance.GameOver ();
			stopped = true;
			StopAllCoroutines ();
		} else if (CarnivalManager.Instance.IsGameWon() && !stopped) {
			// Stop the countdown if the player win
			stopped = true;
			StopAllCoroutines ();
		}
	}

	IEnumerator second()
	{
		yield return new WaitForSeconds (1f);
		if (CarnivalManager.Instance.IsPlaying ()) {
			if (sec > 0)
				sec--;
			if (sec == 0 && minutes != 0) {
				sec = 60;
				minutes--;
			}
			if (!stopped) {
				timeText.text = (sec != 60 ? minutes: minutes+1).ToString ("00") + " : " + (sec != 60 ? sec.ToString ("00"): "00");

				// The last 10 seconds of the countdown
				if (sec <= 10 && minutes == 0) {
					// If second is not the same play the alert sound (Play one tick per second when sec <10 just to warn user)
					if (updatedSecond != sec) {
						ClockTick.Play ();
						updatedSecond = sec;
					}
					if (timeText.color != Color.red) {
						timeText.color = Color.red;
					}
				
				}
			}
			fillLoading ();
		}
		StartCoroutine (second ());
	}

	void fillLoading()
	{
		totalSeconds--;
		float fill = (float)totalSeconds/TOTAL_SECONDS;
		loading.fillAmount = fill;
	}

	public void ResetTimer()
	{
		minutes = Minutes;
		sec = Seconds;

		totalSeconds = 0;
		TOTAL_SECONDS = 0;
		stopped = false;

		// Reset color of the time text
		timeText.color = Color.white;



		timeText.text = minutes.ToString("00") + " : " + sec.ToString("00");
		if (minutes > 0)
			totalSeconds += minutes * 60;
		if (sec > 0)
			totalSeconds += sec;
		TOTAL_SECONDS = totalSeconds;

		// Reset the circunference
		float fill = (float)totalSeconds/TOTAL_SECONDS;
		loading.fillAmount = fill;

		StartCoroutine (second ());
	}
}
