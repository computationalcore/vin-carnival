using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarnivalTimer : MonoBehaviour {

	public Image loading;
	public Text timeText;
	public int minutes;
	public int sec;
	int totalSeconds = 0;
	int TOTAL_SECONDS = 0;
	float fillamount;

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
		if (sec == 0 && minutes == 0) {
			timeText.text = "Time's Up!";
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
		timeText.text = minutes.ToString("00") + " : " + sec.ToString("00");
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
