using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class FrameworkHandler : MonoBehaviour {

	public Dropdown frameworkDropdown;
	public Sprite subsumptionSprite;
	public Sprite stateMachineSprite;
	public Sprite braitenbergSprite;
	public Sprite virtualFieldSprite;
	public Image frameworkImage;
	private string selectedFramework;

	// Update is called once per frame
	void Update () {
		switch (frameworkDropdown.captionText.text) {
		case "Subsumption":
			frameworkImage.sprite = subsumptionSprite;
			selectedFramework = "BuildHierarchy";
			break;
		case "Finite State Machine":
			frameworkImage.sprite = stateMachineSprite;
			selectedFramework = "BuildStateMachine";
			break;
		case "Braitenberg":
			frameworkImage.sprite = braitenbergSprite;
			selectedFramework = "BuildVehicle";
			break;
		case "Virtual Fields":
			frameworkImage.sprite = virtualFieldSprite;
			selectedFramework = "BuildVirtualFields";
			break;
		default:
			frameworkImage.sprite = subsumptionSprite;
			selectedFramework = "BuildHierarchy";
			break;
		}

	}

	public void OnContinueClick(){
		ProjectManager.SetFramework (selectedFramework);
		SceneManager.LoadScene (selectedFramework);
	}

	public void OnHomeClick(){
		SceneManager.LoadScene ("PlatformSelection");
	}
}
