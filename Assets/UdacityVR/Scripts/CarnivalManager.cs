using System;
using UnityEngine;
using TMPro;

public class CarnivalManager : MonoBehaviour {

	[SerializeField]
	private int PlinkoPointsWin = 2000;
	[SerializeField]
	private int WheelPointsWin = 2000;
	[SerializeField]
	private int CoinPointsWin = 2000;

	[SerializeField]
	private GameObject PlinkoPrize;
	[SerializeField]
	private GameObject WheelPrize;
	[SerializeField]
	private GameObject CoinPrize;
	[SerializeField]
	private GameObject CoinPile;

	[SerializeField]
	private GameObject MainCamera;
	[SerializeField]
	private GameObject posCam1;
	[SerializeField]
	private GameObject posCam2;
	[SerializeField]
	private GameObject MainMenu;

	[SerializeField]
	private GameObject MainMenuMusic;
	[SerializeField]
	private GameObject GameMusic;

	[SerializeField]
	private TextMeshPro plinkoScore;
	[SerializeField]
	private TextMeshPro wheelScore;
	[SerializeField]
	private TextMeshPro coinScore;

	public static CarnivalManager Instance;

	private int plinkoPoints;
	private int wheelPoints;
	private int coinPoints;

	// starting value for the Lerp
	private float t = 0.0f;
	private float timeToMove = 1.0f;
	private Boolean isCameraAnimationRunning = false;

	// Possibles game states
	private enum gameState {MainMenu, Playing, GameOver};
	// Start Game state
	private gameState currentGameState = gameState.MainMenu;

	void Awake() {
		if (Instance == null)
			Instance = this;

		GameMusic.SetActive (false);
		

		PlinkoPrize.SetActive(false);
		WheelPrize.SetActive(false);
		CoinPrize.SetActive(false);
	}

	void OnDestroy() {
		if (Instance = this)
			Instance = null;
	}

	// Update is called once per frame
	void Update () {
		plinkoScore.text = "Plinko: " + plinkoPoints.ToString("0000");
		wheelScore.text = "Wheel: " + wheelPoints.ToString("0000");
		coinScore.text = "Coins: " + coinPoints.ToString("0000");

		// Verify if the win plinko
		if (plinkoPoints >= PlinkoPointsWin) {
			PlinkoPrize.SetActive(true);
		}

		if (wheelPoints >= WheelPointsWin) {
			WheelPrize.SetActive(true);
		}

		if (coinPoints >= CoinPointsWin) {
			CoinPrize.SetActive(true);
			// Hide the coin pile to the prize fall correctly
			CoinPile.SetActive(false);
		}
	}

	void LateUpdate() {
		ReOrientCamera ();
	}

	public void ReOrientCamera (){
		if (isCameraAnimationRunning) {
			if (currentGameState == gameState.Playing) {
				MainCamera.transform.position = Vector3.Lerp (posCam1.transform.position, posCam2.transform.position, t);
				MainCamera.transform.rotation = Quaternion.Euler (
					Mathf.LerpAngle (posCam1.transform.eulerAngles.x, posCam2.transform.eulerAngles.x, t),
					Mathf.LerpAngle (posCam1.transform.eulerAngles.y, posCam2.transform.eulerAngles.y, t),
					Mathf.LerpAngle (posCam1.transform.eulerAngles.z, posCam2.transform.eulerAngles.z, t)
				);
			} else {
				MainCamera.transform.position = Vector3.Lerp (posCam2.transform.position, posCam1.transform.position, t);
				MainCamera.transform.rotation = Quaternion.Euler (
					Mathf.LerpAngle (posCam2.transform.eulerAngles.x, posCam1.transform.eulerAngles.x, t),
					Mathf.LerpAngle (posCam2.transform.eulerAngles.y, posCam1.transform.eulerAngles.y, t),
					Mathf.LerpAngle (posCam2.transform.eulerAngles.z, posCam1.transform.eulerAngles.z, t)
				);
			}
			// now check if the interpolator has reached 1.0
			// and swap maximum and minimum so game object moves
			// in the opposite direction.
			if (t < timeToMove) {
				// .. increate the t interpolater
				t += Time.deltaTime / timeToMove;
			} else {
				t = timeToMove;
				isCameraAnimationRunning = false;
			}
		}


	}

	public void StartGame() {
		MainMenu.SetActive (false);
		MainMenuMusic.SetActive (false);
		GameMusic.SetActive (true);
		currentGameState = gameState.Playing;
		isCameraAnimationRunning = true;
	}

	public void GameOver() {
		MainMenu.SetActive (false);
		MainMenuMusic.SetActive (false);
		GameMusic.SetActive (true);
		currentGameState = gameState.Playing;
		isCameraAnimationRunning = true;
	}

	public void IncrementPlinkoScore(float points) {
		plinkoPoints += (int) points;
	}

	public void IncrementWheelScore(float points) {
		wheelPoints += (int) points;
	}

	public void IncrementCoinScore() {
		coinPoints += 1000;
	}
}
