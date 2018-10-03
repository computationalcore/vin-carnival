using System;
using UnityEngine;
using UnityEditor;
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
	private GameObject GameOverCanvas;
	[SerializeField]
	private TextMeshPro GameOverText;


	[SerializeField]
	private AudioSource MainMenuMusic;
	[SerializeField]
	private AudioSource GameMusic;
	[SerializeField]
	private AudioSource WinSound;
	[SerializeField]
	private AudioSource GameOverSound;

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
	private enum gameState {MainMenu, Playing, GameWin, GameOver};
	// Start Game state
	private gameState currentGameState = gameState.MainMenu;

	// To store prizes original position
	private Vector3 plinkoPrizeOriginalPos;
	private Vector3 wheelPrizeOriginalPos;
	private Vector3 coinPrizeOriginalPos;

	void Awake() {
		if (Instance == null)
			Instance = this;

		// Store prizes original position
		plinkoPrizeOriginalPos =  new Vector3(PlinkoPrize.transform.position.x, PlinkoPrize.transform.position.y, PlinkoPrize.transform.position.z);
		wheelPrizeOriginalPos = new Vector3(WheelPrize.transform.position.x, WheelPrize.transform.position.y, WheelPrize.transform.position.z);
		coinPrizeOriginalPos = new Vector3(CoinPrize.transform.position.x, CoinPrize.transform.position.y, CoinPrize.transform.position.z);

		ResetGame ();
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

		// Check if the user won the game
		if ( 
			(plinkoPoints >= PlinkoPointsWin) &&
			(wheelPoints >= WheelPointsWin) &&
			(coinPoints >= CoinPointsWin) &&
			(currentGameState == gameState.Playing)
		   )
		{
			GameWin();
		}
	}

	void LateUpdate() {
		ReOrientCamera ();
	}

	public void ReOrientCamera (){
		if (isCameraAnimationRunning) {
			if (currentGameState == gameState.Playing && MainCamera.transform.position != posCam2.transform.position) {
				MainCamera.transform.position = Vector3.Lerp (posCam1.transform.position, posCam2.transform.position, t);
				MainCamera.transform.rotation = Quaternion.Euler (
					Mathf.LerpAngle (posCam1.transform.eulerAngles.x, posCam2.transform.eulerAngles.x, t),
					Mathf.LerpAngle (posCam1.transform.eulerAngles.y, posCam2.transform.eulerAngles.y, t),
					Mathf.LerpAngle (posCam1.transform.eulerAngles.z, posCam2.transform.eulerAngles.z, t)
				);
			} else if (currentGameState == gameState.MainMenu && MainCamera.transform.position != posCam1.transform.position) {
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
				t = 0.0f;
				isCameraAnimationRunning = false;
			}
		}
	}

	private void ResetGame(){
		// Reset score
		plinkoPoints = 0;
		wheelPoints = 0;
		coinPoints = 0;

		// Reset prizes positions
		/*PlinkoPrize.transform.position = plinkoPrizeOriginalPos;
		WheelPrize.transform.position = wheelPrizeOriginalPos;
		CoinPrize.transform.position = coinPrizeOriginalPos;*/
		PrefabUtility.ResetToPrefabState (PlinkoPrize);
		PrefabUtility.ResetToPrefabState (WheelPrize);
		PrefabUtility.ResetToPrefabState (CoinPrize);
		// Reset Prizes
		PlinkoPrize.SetActive(false);
		WheelPrize.SetActive(false);
		CoinPrize.SetActive(false);

		// Show the coin pile
		CoinPile.SetActive(true);
	}

	public void GoToMainMenu() {
		ResetGame();
		GameOverCanvas.SetActive(false);
		MainMenu.SetActive(true);
		MainMenuMusic.Play();
		currentGameState = gameState.MainMenu;
		isCameraAnimationRunning = true;
	}

	public void StartGame() {
		ResetGame();
		GameOverCanvas.SetActive(false);
		MainMenu.SetActive(false);
		MainMenuMusic.Stop();
		GameMusic.Play();
		currentGameState = gameState.Playing;
		isCameraAnimationRunning = true;
	}

	public void GameWin()
	{
		// Stop the game music
		GameMusic.Stop();
		WinSound.Play();
		currentGameState = gameState.GameWin;
		GameOverCanvas.SetActive(true);
		GameOverText.text = "You Win!!!";
		GameOverText.color = Color.yellow;
	}

	public void GameOver()
	{
		// Stop the game music
		GameMusic.Stop();
		GameOverSound.Play();
		currentGameState = gameState.GameOver;
		GameOverCanvas.SetActive(true);
		GameOverText.text = "Game Over";
		GameOverText.color = Color.red;
	}

	public bool IsPlaying() {
		return (currentGameState == gameState.Playing);
	}

	public bool IsPlinkoWon() {
		return (plinkoPoints >= PlinkoPointsWin);
	}

	public bool IsWheelWon() {
		return (wheelPoints >= WheelPointsWin);
	}

	public bool IsCoinWon() {
		return (coinPoints >= CoinPointsWin);
	}

	public bool IsGameWon() {
		return (currentGameState == gameState.GameWin);
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
