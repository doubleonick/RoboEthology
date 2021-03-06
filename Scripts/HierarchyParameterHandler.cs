using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;


public class HierarchyParameterHandler : MonoBehaviour {
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
		string hierarchyString;
		char[] delimiter = " - ".ToCharArray ();
		//int delimiterIndx;
		//int prevDelimiterIndx;
		int numDelimiters = 0;
		string[] behaviors;
		System.CharEnumerator hierarchyEnumerator;
		System.Text.StringBuilder generationMsg = new System.Text.StringBuilder ();
		hierarchyString = Hierarchy.hierarchyText.text;

		Debug.Log ("OnGenerateClick()");

		generationMsg.Append ("Hierarchy");
		generationMsg.Append (hierarchyString);
		generationMsg.Append (" controller type: ");
		generationMsg.Append (selectedPlatform);
		Debug.Log (generationMsg);

		hierarchyEnumerator = hierarchyString.GetEnumerator ();
		//delimiterIndx = 0;
		//prevDelimiterIndx = delimiterIndx;
		while (hierarchyEnumerator.MoveNext ()) {
			//delimiterIndx++;
			if (hierarchyEnumerator.Current == '-') {
				numDelimiters++;
				//Debug.Log ("- found at... " + delimiterIndx);

			}
			//prevDelimiterIndx = delimiterIndx;
		}
		Debug.Log ("numDelimiters = " + numDelimiters.ToString ());
		behaviors = hierarchyString.Split (delimiter, numDelimiters+1);
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
			BuildArduinoHierarchy (finalBehaviors);
			break;
		case "KIPR Link":
			BuildLinkHierarchy (finalBehaviors);
			break;
		case "EZ Robot":
			break;
		default:
			BuildLinkHierarchy (finalBehaviors);
			break;
		}

	}
	/****************************************************************/
	private void BuildArduinoHierarchy(string[] behaviors){
		string arduinoDirectoryPath = Directory.GetCurrentDirectory() + "/RobotCode/Template Code Files/Hierarchy Temp/Arduino";
		string arduinoHierarchyPath = Directory.GetCurrentDirectory () + "/RobotCode/Generated Code Files/Hierarchy/Arduino";
		string arduinoFileName = arduinoDirectoryPath + "/HI_TEMP_ARDUINO_PCA/HI_TEMP_ARDUINO_PCA.ino";
		string hierarchyFileName = arduinoHierarchyPath + "/HI_ARDUINO/HI_ARDUINO.ino";
		System.Text.StringBuilder hierarchyBuilder = new System.Text.StringBuilder();

		Debug.Log ("Directory Root: " + Directory.GetDirectoryRoot ("RobotCode"));

		bool arduinoDirectoryExists = System.IO.Directory.Exists(arduinoDirectoryPath);

		Debug.Log ("BuildArduinoHierarchy()");
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
				hierarchyBuilder.Append ("if(");
				if (behavior == "Cruise") {
					Debug.Log (behavior);
					hierarchyBuilder.Append ("TRUE){\nstraight_cruise();\n}");
				}else if (behavior == "Arc_Cruise") {
					Debug.Log (behavior);
					hierarchyBuilder.Append ("TRUE){\narc_cruise();\n}");
				}


			} else if (behaviorIndx < behaviors.Length - 1) {
				Debug.Log("Middle behavior: " + behavior);
				hierarchyBuilder.Append ("else if(");
				if (behavior == "Cruise") {
					hierarchyBuilder.Append ("true){straight_cruise();}\n");
				}else if (behavior == "Arc_Cruise") {
					hierarchyBuilder.Append ("true){arc_cruise();}\n");
				}
			} else {
				Debug.Log ("Last Behavior: " + behavior);
				if (behavior == "Cruise") {
					hierarchyBuilder.Append ("else{straight_cruise();}\n");
				}else if (behavior == "Arc_Cruise") {
					hierarchyBuilder.Append ("else{arc_cruise();}\n");
				} else {
					hierarchyBuilder.Append ("else if(");
				}
			}
			if (behavior != "Cruise" && behavior != "Arc_Cruise") {
				hierarchyBuilder.Append ("check_");
				hierarchyBuilder.Append (behavior.ToLower());
				hierarchyBuilder.Append ("_conditions()){\n");
				hierarchyBuilder.Append (behavior.ToLower());
				hierarchyBuilder.Append ("();\n}\n");
			}
			behaviorIndx++;
		}

		Debug.Log ("ReadAllText for " + arduinoFileName + "\n");

		string arduinoCode = System.IO.File.ReadAllText (arduinoFileName);
		Debug.Log ("Replace <<HIERARCHY>> with the hierarchy string.\n");
		string hierarchyCode = arduinoCode.Replace ("<<HIERARCHY>>", hierarchyBuilder.ToString ());
		Debug.Log (hierarchyCode);
		Debug.Log ("WriteAllText() to " + hierarchyFileName + "\n");
		System.IO.File.WriteAllText (hierarchyFileName, hierarchyCode);
		//System.IO.File.OpenWrite
		//byte[] linkCodeByteArray = GetBytes(linkCode);
		//FileStream linkStream = System.IO.File.OpenWrite (linkFileName);
		//linkStream.Write (linkCodeByteArray, 0, linkCode.Length);

		//Record some data about this experiment before moving on.
		//System.IO.File.AppendAllText (experimentFileName, experimentalDataString);
		//Debug.Log("delimiterIndx = " + delimiterIndx.ToString());

	}
	/****************************************************************/
	private void BuildLinkHierarchy(string[] behaviors){
		string linkDirectoryPath = Directory.GetCurrentDirectory() + "/RobotCode/Template Code Files/Hierarchy Temp/Link";
		string linkHierarchyPath = Directory.GetCurrentDirectory() + "/RobotCode/Generated Code Files/Hierarchy/Link";
		string linkFileName = linkDirectoryPath + "/HI_TEMP_LINK.c";
		string hierarchyFileName = linkHierarchyPath + "/HILINK.c";
		System.Text.StringBuilder hierarchyBuilder = new System.Text.StringBuilder();

		Debug.Log ("Directory Root: " + Directory.GetCurrentDirectory());

		bool linkDirectoryExists = System.IO.Directory.Exists(linkDirectoryPath);

		Debug.Log ("BuildLinkHierarchy()");
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
				hierarchyBuilder.Append ("if(");
				if (behavior == "Cruise") {
					Debug.Log (behavior);
					hierarchyBuilder.Append ("TRUE){\nstraight_cruise();\n}");
				}else if (behavior == "Arc_Cruise") {
					Debug.Log (behavior);
					hierarchyBuilder.Append ("TRUE){\narc_cruise();\n}");
				}


			} else if (behaviorIndx < behaviors.Length - 1) {
				Debug.Log("Middle behavior: " + behavior);
				hierarchyBuilder.Append ("else if(");
				if (behavior == "Cruise") {
					hierarchyBuilder.Append ("true){straight_cruise();}\n");
				}else if (behavior == "Arc_Cruise") {
					hierarchyBuilder.Append ("true){arc_cruise();}\n");
				}
			} else {
				Debug.Log ("Last Behavior: " + behavior);
				if (behavior == "Cruise") {
					hierarchyBuilder.Append ("else{straight_cruise();}\n");
				}else if (behavior == "Arc_Cruise") {
					hierarchyBuilder.Append ("else{arc_cruise();}\n");
				} else {
					hierarchyBuilder.Append ("else if(");
				}
			}
			if (behavior != "Cruise" && behavior != "Arc_Cruise") {
				hierarchyBuilder.Append ("check_");
				hierarchyBuilder.Append (behavior.ToLower());
				hierarchyBuilder.Append ("_conditions()){\n");
				hierarchyBuilder.Append (behavior.ToLower());
				hierarchyBuilder.Append ("();\n}\n");
			}
			behaviorIndx++;
		}

		Debug.Log ("ReadAllText for " + linkFileName + "\n");

		string linkCode = System.IO.File.ReadAllText (linkFileName);
		Debug.Log ("Replace <<HIERARCHY>> with the hierarchy string.\n");
		string hierarchyCode = linkCode.Replace ("<<HIERARCHY>>", hierarchyBuilder.ToString ());
		Debug.Log (hierarchyCode);
		Debug.Log ("WriteAllText() to " + hierarchyFileName + "\n");
		linkDirectoryExists = System.IO.Directory.Exists(linkHierarchyPath);
		if (linkDirectoryExists == false) {
			System.IO.Directory.CreateDirectory (linkHierarchyPath);
		}
		System.IO.File.WriteAllText (hierarchyFileName, hierarchyCode);
		//System.IO.File.OpenWrite
		//byte[] linkCodeByteArray = GetBytes(linkCode);
		//FileStream linkStream = System.IO.File.OpenWrite (linkFileName);
		//linkStream.Write (linkCodeByteArray, 0, linkCode.Length);

		//Record some data about this experiment before moving on.
		//System.IO.File.AppendAllText (experimentFileName, experimentalDataString);
		//Debug.Log("delimiterIndx = " + delimiterIndx.ToString());

	}

}
