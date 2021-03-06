using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;


public class StateParameterHandler : MonoBehaviour {
	//[SerializeField] string controllerType;
	//public Dropdown controllerDropdown;
	private string selectedPlatform;
	/****************************************************************/
	void Start(){
		selectedPlatform = ProjectManager.GetPlatform ();
	}
	/****************************************************************/
	public void OnHomeClick(){
		SceneManager.LoadScene ("PlatformSelection");
	}
	/****************************************************************/
	public void OnGenerateClick(){
		string stateString = "";
		char[] delimiter = " - ".ToCharArray ();
		//int delimiterIndx;
		//int prevDelimiterIndx;
		int numDelimiters = 0;
		string[] behaviors;
		System.CharEnumerator stateEnumerator;
		System.Text.StringBuilder generationMsg = new System.Text.StringBuilder ();

		Debug.Log ("OnGenerateClick()");

		generationMsg.Append ("state");
		generationMsg.Append (stateString);
		generationMsg.Append (" controller type: ");
		generationMsg.Append (selectedPlatform);
		Debug.Log (generationMsg);

		stateEnumerator = stateString.GetEnumerator ();
		//delimiterIndx = 0;
		//prevDelimiterIndx = delimiterIndx;
		while (stateEnumerator.MoveNext ()) {
			//delimiterIndx++;
			if (stateEnumerator.Current == '-') {
				numDelimiters++;
				//Debug.Log ("- found at... " + delimiterIndx);

			}
			//prevDelimiterIndx = delimiterIndx;
		}
		Debug.Log ("numDelimiters = " + numDelimiters.ToString ());
		behaviors = stateString.Split (delimiter, numDelimiters+1);
		string[] finalBehaviors = new string[numDelimiters - 1];
		int i = 0;
		foreach (string behavior in behaviors) {
			//Debug.Log (behavior);
			if (behavior != "") {
				finalBehaviors [i] = behavior;
				Debug.Log ("finalBehaviors[i]: " + finalBehaviors[i]);
				i++;
			}
		}

		switch (selectedPlatform) {
		case "Simulation":
			break;
		case "Arduino":
			break;
		case "KIPR Link":
			BuildLinkState (finalBehaviors);
			break;
		case "EZ Robot":
			break;
		default:
			BuildLinkState (finalBehaviors);
			break;
		}

	}
	/****************************************************************/
	private void BuildArduinostate(string[] behaviors){
		string arduinoDirectoryPath = Directory.GetCurrentDirectory() + "/Assets/RobotCode/RoboEthologyArduino";
		string arduinoFileName = arduinoDirectoryPath + "/HI_TEMP_ARDUINO/HI_TEMP_ARDUINO.ino";
		string stateFileName = arduinoDirectoryPath + "/HI_ARDUINO/HI_ARDUINO.ino";
		System.Text.StringBuilder stateBuilder = new System.Text.StringBuilder();

		bool arduinoDirectoryExists = System.IO.Directory.Exists(arduinoDirectoryPath);

		Debug.Log ("BuildArduinostate()");
		if (arduinoDirectoryExists == false) {
			System.IO.Directory.CreateDirectory (arduinoDirectoryPath);
		} 
		bool arduinoFileExists = System.IO.File.Exists (arduinoFileName);
		Debug.Log ("experimentFileExists? " + arduinoFileExists + " for file " + arduinoFileName);
		if (arduinoFileExists == false) {
			System.IO.File.Create (arduinoFileName);
		}

		int behaviorIndx = 0;
		foreach (string behavior in behaviors) {
			Debug.Log ("Behavior " + behaviorIndx + " of " + (behaviors.Length - 1).ToString() + " = " + behavior);
			if (behaviorIndx == 0) {
				Debug.Log ("First behavior...");
				stateBuilder.Append ("if(");
				if (behavior == "Cruise") {
					Debug.Log (behavior);
					stateBuilder.Append ("true){\nstraightCruise();\n}");
				}

			} else if (behaviorIndx < behaviors.Length - 1) {
				Debug.Log("Middle behavior: " + behavior);
				stateBuilder.Append ("else if(");
				if (behavior == "Cruise") {
					stateBuilder.Append ("true){straightCruise();}\n");
				}
			} else {
				Debug.Log ("Last Behavior: " + behavior);
				if (behavior == "Cruise") {
					stateBuilder.Append ("else{straightCruise();}\n");
				} else {
					stateBuilder.Append ("else if(");
				}
			}
			if (behavior != "Cruise") {
				stateBuilder.Append ("check");
				stateBuilder.Append (behavior);
				stateBuilder.Append ("Constraints(){\n");
				stateBuilder.Append (behavior);
				stateBuilder.Append ("();\n}\n");
			}
			behaviorIndx++;
		}

		Debug.Log ("ReadAllText for " + arduinoFileName + "\n");

		string arduinoCode = System.IO.File.ReadAllText (arduinoFileName);
		Debug.Log ("Replace <<state>> with the state string.\n");
		string stateCode = arduinoCode.Replace ("<<state>>", stateBuilder.ToString ());
		Debug.Log (stateCode);
		Debug.Log ("WriteAllText() to " + stateFileName + "\n");
		System.IO.File.WriteAllText (stateFileName, stateCode);
		//System.IO.File.OpenWrite
		//byte[] linkCodeByteArray = GetBytes(linkCode);
		//FileStream linkStream = System.IO.File.OpenWrite (linkFileName);
		//linkStream.Write (linkCodeByteArray, 0, linkCode.Length);

		//Record some data about this experiment before moving on.
		//System.IO.File.AppendAllText (experimentFileName, experimentalDataString);
		//Debug.Log("delimiterIndx = " + delimiterIndx.ToString());

	}
	/****************************************************************/
	private void BuildLinkState(string[] behaviors){
		string linkDirectoryPath = Directory.GetCurrentDirectory() + "/Desktop/CR-NBL/UDemy/RoboEthology/Assets/RobotCode/RoboEthologyKIPR";
		string linkFileName = linkDirectoryPath + "/HI_TEMP_LINK.c";
		string stateFileName = linkDirectoryPath + "/HILINK.c";
		System.Text.StringBuilder stateBuilder = new System.Text.StringBuilder();

		bool linkDirectoryExists = System.IO.Directory.Exists(linkDirectoryPath);

		Debug.Log ("BuildLinkState()");
		if (linkDirectoryExists == false) {
			System.IO.Directory.CreateDirectory (linkDirectoryPath);
		} 
		bool linkFileExists = System.IO.File.Exists (linkFileName);
		Debug.Log ("experimentFileExists? " + linkFileExists + " for file " + linkFileName);
		if (linkFileExists == false) {
			System.IO.File.Create (linkFileName);
		}

		int behaviorIndx = 0;
		foreach (string behavior in behaviors) {
			Debug.Log ("Behavior " + behaviorIndx + " of " + (behaviors.Length - 1).ToString() + " = " + behavior);
			if (behaviorIndx == 0) {
				Debug.Log ("First behavior...");
				stateBuilder.Append ("if(");
				if (behavior == "Cruise") {
					Debug.Log (behavior);
					stateBuilder.Append ("true){\nStraightCruise();\n}");
				}

			} else if (behaviorIndx < behaviors.Length - 1) {
				Debug.Log("Middle behavior: " + behavior);
				stateBuilder.Append ("else if(");
				if (behavior == "Cruise") {
					stateBuilder.Append ("true){StraightCruise();}\n");
				}
			} else {
				Debug.Log ("Last Behavior: " + behavior);
				if (behavior == "Cruise") {
					stateBuilder.Append ("else{StraightCruise();}\n");
				} else {
					stateBuilder.Append ("else if(");
				}
			}
			if (behavior != "Cruise") {
				stateBuilder.Append ("Check");
				stateBuilder.Append (behavior);
				stateBuilder.Append ("Constraints(){\n");
				stateBuilder.Append (behavior);
				stateBuilder.Append ("();\n}\n");
			}
			behaviorIndx++;
		}

		Debug.Log ("ReadAllText for " + linkFileName + "\n");

		string linkCode = System.IO.File.ReadAllText (linkFileName);
		Debug.Log ("Replace <<STATE>> with the state string.\n");
		string stateCode = linkCode.Replace ("<<STATE>>", stateBuilder.ToString ());
		Debug.Log (stateCode);
		Debug.Log ("WriteAllText() to " + stateFileName + "\n");
		System.IO.File.WriteAllText (stateFileName, stateCode);
		//System.IO.File.OpenWrite
		//byte[] linkCodeByteArray = GetBytes(linkCode);
		//FileStream linkStream = System.IO.File.OpenWrite (linkFileName);
		//linkStream.Write (linkCodeByteArray, 0, linkCode.Length);

		//Record some data about this experiment before moving on.
		//System.IO.File.AppendAllText (experimentFileName, experimentalDataString);
		//Debug.Log("delimiterIndx = " + delimiterIndx.ToString());

	}

}
