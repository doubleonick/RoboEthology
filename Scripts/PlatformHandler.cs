using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class PlatformHandler : MonoBehaviour {

	public Dropdown platformDropdown;
	public Sprite simulationSprite;
	public Sprite arduinoSprite;
	public Sprite linkSprite;
	public Sprite ezRobotSprite;
	public Image platformImage;
	private string selectedPlatform;

	// Update is called once per frame
	void Update () {
		switch (platformDropdown.captionText.text) {
		case "Simulation":
			selectedPlatform = "Simulation";
			platformImage.sprite = simulationSprite;
			break;
		case "Arduino":
			selectedPlatform = "Arduino";
			platformImage.sprite = arduinoSprite;
			break;
		case "KIPR Link":
			selectedPlatform = "KIPR Link";
			platformImage.sprite = linkSprite;
			break;
		case "EZ Robot":
			selectedPlatform = "EZ Robot";
			platformImage.sprite = ezRobotSprite;
			break;
		default:
			selectedPlatform = "KIPR Link";
			platformImage.sprite = linkSprite;
			break;
		}

	}

	public void OnContinueClick(){
		ProjectManager.SetPlatform (selectedPlatform);
		SceneManager.LoadScene ("FrameworkSelection");
	}
}
