using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarnivalTimer : MonoBehaviour {

	public Image loading;
	public Text timeText;
	public int minutes;
	public int sec;
	public AudioSource ClockTick;

	int totalSeconds = 0;
	int TOTAL_SECONDS = 0;
	float fillamount;

	// Auxiliary sound tick logic
	int updatedSecond = 10;

	private Boolean stopped = false;
	void Start ()
	{
		timeText.text = minutes.ToString("00") + " : " + sec.ToString("00");
		if (minutes > 0)
			totalSeconds += minutes * 60;
		if (sec > 0)
			totalSeconds += sec;
		TOTAL_SECONDS = totalSeconds;
		StartCoroutine (second ());
	}

	void Update ()
	{
		if (sec == 0 && minutes == 0 && !stopped) {
			timeText.text = "Game \nOver!";
			timeText.color = Color.red;
			CarnivalManager.Instance.GameOver ();
			stopped = true;
			StopCoroutine (second ());
		} else if (CarnivalManager.Instance.IsGameWin() && !stopped) {
			// Stop the countdown if the player win
			stopped = true;
			StopCoroutine (second ());
		}
	}
	IEnumerator second()
	{
		yield return new WaitForSeconds (1f);
		if(sec > 0)
			sec--;
		if (sec == 0 && minutes != 0) {
			sec = 60;
			minutes--;
		}
		if (!stopped) {
			timeText.text = minutes.ToString ("00") + " : " + sec.ToString ("00");

			// The last 10 seconds of the countdown
			if(sec < 10 && minutes == 0){
				// If second is not the same play the alert sound (Play one tick per second when sec <10 just to warn user)
				if (updatedSecond != sec) {
					ClockTick.Play ();
					updatedSecond = sec;
				}
				if(timeText.color != Color.red){
					timeText.color = Color.red;
				}
				
			}
		}
		fillLoading ();
		StartCoroutine (second ());
	}

	void fillLoading()
	{
		totalSeconds--;
		float fill = (float)totalSeconds/TOTAL_SECONDS;
		loading.fillAmount = fill;
	}
}
