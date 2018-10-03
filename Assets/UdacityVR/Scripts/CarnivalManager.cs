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

	// To store prizes original position and rotation
	private Vector3 plinkoPrizeOriginalPos;
	private Vector3 wheelPrizeOriginalPos;
	private Vector3 coinPrizeOriginalPos;
	private Quaternion plinkoPrizeOriginalRot;
	private Quaternion wheelPrizeOriginalRot;
	private Quaternion coinPrizeOriginalRot;

	/// <summary>
	/// Setup the game manager when created.
	/// </summary>
	void Awake() 
	{
		if (Instance == null)
			Instance = this;

		// Store prizes original position and rotation
		plinkoPrizeOriginalPos =  PlinkoPrize.transform.GetChild(0).transform.position;
		wheelPrizeOriginalPos = WheelPrize.transform.GetChild (0).transform.position;
		coinPrizeOriginalPos = CoinPrize.transform.GetChild(0).transform.position;
		plinkoPrizeOriginalRot =  PlinkoPrize.transform.GetChild(0).transform.rotation;
		wheelPrizeOriginalRot = WheelPrize.transform.GetChild (0).transform.rotation;
		coinPrizeOriginalRot = CoinPrize.transform.GetChild(0).transform.rotation;

		ResetGame ();
	}

	void OnDestroy() 
	{
		if (Instance = this)
			Instance = null;
	}

	/// <summary>
	/// Update is called once per frame.
	/// Checks the score, if a particular game is won and if user win all the games.
	/// </summary>
	void Update () 
	{
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

	/// <summary>
	/// Handle transition camera animations.
	/// </summary>
	void LateUpdate() 
	{
		ReOrientCamera ();
	}

	public void ReOrientCamera ()
	{
		if (isCameraAnimationRunning) 
		{
			if (currentGameState == gameState.Playing && MainCamera.transform.position != posCam2.transform.position) 
			{
				MainCamera.transform.position = Vector3.Lerp (posCam1.transform.position, posCam2.transform.position, t);
				MainCamera.transform.rotation = Quaternion.Euler (
					Mathf.LerpAngle (posCam1.transform.eulerAngles.x, posCam2.transform.eulerAngles.x, t),
					Mathf.LerpAngle (posCam1.transform.eulerAngles.y, posCam2.transform.eulerAngles.y, t),
					Mathf.LerpAngle (posCam1.transform.eulerAngles.z, posCam2.transform.eulerAngles.z, t)
				);
			} else if (currentGameState == gameState.MainMenu && MainCamera.transform.position != posCam1.transform.position) 
			{
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
			if (t < timeToMove) 
			{
				// .. increate the t interpolater
				t += Time.deltaTime / timeToMove;
			} else 
			{
				t = 0.0f;
				isCameraAnimationRunning = false;
			}
		}
	}

	/// <summary>
	/// Reset game score and prizes.
	/// </summary>
	private void ResetGame()
	{
		// Reset score
		plinkoPoints = 0;
		wheelPoints = 0;
		coinPoints = 0;

		// Reset prizes positions and rotation
		PlinkoPrize.transform.GetChild(0).transform.position = plinkoPrizeOriginalPos;
		WheelPrize.transform.GetChild(0).transform.position = wheelPrizeOriginalPos;
		CoinPrize.transform.GetChild(0).transform.position = coinPrizeOriginalPos;
		PlinkoPrize.transform.GetChild(0).transform.rotation = plinkoPrizeOriginalRot;
		WheelPrize.transform.GetChild(0).transform.rotation = wheelPrizeOriginalRot;
		CoinPrize.transform.GetChild(0).transform.rotation = coinPrizeOriginalRot;

		// Reset Prizes visibility
		PlinkoPrize.SetActive(false);
		WheelPrize.SetActive(false);
		CoinPrize.SetActive(false);

		// Show the coin pile
		CoinPile.SetActive(true);
	}

	/// <summary>
	/// To to main menu.
	/// </summary>
	public void GoToMainMenu() 
	{
		ResetGame();
		GameOverCanvas.SetActive(false);
		MainMenu.SetActive(true);
		MainMenuMusic.Play();
		currentGameState = gameState.MainMenu;
		isCameraAnimationRunning = true;
	}

	/// <summary>
	/// Start the game.
	/// </summary>
	public void StartGame() 
	{
		ResetGame();
		GameOverCanvas.SetActive(false);
		MainMenu.SetActive(false);
		MainMenuMusic.Stop();
		GameMusic.Play();
		currentGameState = gameState.Playing;
		isCameraAnimationRunning = true;
	}

	/// <summary>
	/// Set game win state.
	/// </summary>
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

	/// <summary>
	/// Set game game over state.
	/// </summary>
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

	/// <summary>
	/// Return true if game state is playing.
	/// </summary>
	public bool IsPlaying() 
	{
		return (currentGameState == gameState.Playing);
	}

	/// <summary>
	/// Return true user beats the plink mini game.
	/// </summary>
	public bool IsPlinkoWon() 
	{
		return (plinkoPoints >= PlinkoPointsWin);
	}

	/// <summary>
	/// Return true user beats the wheel mini game.
	/// </summary>
	public bool IsWheelWon() 
	{
		return (wheelPoints >= WheelPointsWin);
	}

	/// <summary>
	/// Return true user beats the CoinToss mini game.
	/// </summary>
	public bool IsCoinWon() 
	{
		return (coinPoints >= CoinPointsWin);
	}

	/// <summary>
	/// Return true user beats all the games.
	/// </summary>
	public bool IsGameWon() 
	{
		return (currentGameState == gameState.GameWin);
	}

	/// <summary>
	/// Increment the current score of the Plinko mini game.
	/// </summary>
	public void IncrementPlinkoScore(float points) 
	{
		plinkoPoints += (int) points;
	}

	/// <summary>
	/// Increment the current score of the Wheel mini game.
	/// </summary>
	public void IncrementWheelScore(float points) 
	{
		wheelPoints += (int) points;
	}

	/// <summary>
	/// Increment the current score of the CoinToss mini game.
	/// </summary>
	public void IncrementCoinScore() 
	{
		coinPoints += 1000;
	}
}
