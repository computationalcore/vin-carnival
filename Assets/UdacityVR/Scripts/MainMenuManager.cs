using UnityEngine;
using System.Collections;

/// <summary>
/// The main menu buttons handlers.
/// </summary>
public class MainMenuManager : MonoBehaviour {

	[SerializeField]
	private GameObject MainMenuLayer;
	[SerializeField]
	private GameObject AboutLayer;
	[SerializeField]
	private GameObject HelpLayer;

	void Awake() {
		AboutLayer.SetActive(false);
		HelpLayer.SetActive(false);
	}

	/// <summary>
	/// Open the project github page on the device default browser.
	/// </summary>
	public void showAbout() {
		MainMenuLayer.SetActive(false);
		AboutLayer.SetActive(true);
		HelpLayer.SetActive(false);
	}

	/// <summary>
	/// Open the project github page on the device default browser.
	/// </summary>
	public void showHelp() {
		MainMenuLayer.SetActive(false);
		AboutLayer.SetActive(false);
		HelpLayer.SetActive(true);
	}

	/// <summary>
	/// Open the project github page on the device default browser.
	/// </summary>
	public void backToMainMenu() {
		MainMenuLayer.SetActive(true);
		AboutLayer.SetActive(false);
		HelpLayer.SetActive(false);
	}

	/// <summary>
	/// Open the project github page on the device default browser.
	/// </summary>
	public void openGithub() {
		//Open the project github page
		Application.OpenURL("https://github.com/computationalcore/vin-carnival/");
	}

}