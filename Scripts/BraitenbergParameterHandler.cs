using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class BraitenbergParameterHandler : MonoBehaviour {
	//<Braitenberg vehicle variabls....
	//
	//Eventually want a more generalized version of this.
	public Button leftIRButton;
	public Button rightIRButton;
	public Button leftLDRButton;
	public Button rightLDRButton;
	public Button leftServoButton;
	public Button rightServoButton;
	public Button excitatoryButton;
	public Button inhibitoryButton;
	//Is a wire connection initiated?
	//private bool initiatedConnection;
	//Wires
	const int NUM_WIRES = 8;
	const int RightIR_To_LeftServo = 0;
	const int LeftIR_To_LeftServo = 1;
	const int RightLDR_To_LeftServo = 2;
	const int LeftLDR_To_LeftServo = 3;
	const int RightIR_To_RightServo = 4;
	const int LeftIR_To_RightServo = 5;
	const int RightLDR_To_RightServo = 6;
	const int LeftLDR_To_RightServo = 7;

	const int RightIR = 0;
	const int LeftIR = 1;
	const int RightLDR = 2;
	const int LeftLDR = 3;
	const int LeftServo = 4;
	const int RightServo = 5;
	int wireInput;
	int wireOutput;
	public GameObject[] wires = new GameObject[NUM_WIRES];
	//Data structure for software wires that will be written to Arduino/Link/etc.
	private int[] connections = new int[NUM_WIRES];
	private string selectedPlatform;


	//.... Braitenberg vehicle variables>
	/****************************************************************/
	void Start(){
		wires[RightIR_To_LeftServo].transform.gameObject.SetActive (false);
		wires[LeftIR_To_LeftServo].transform.gameObject.SetActive (false);
		wires[RightLDR_To_LeftServo].transform.gameObject.SetActive (false);
		wires[LeftLDR_To_LeftServo].transform.gameObject.SetActive (false);
		wires[RightIR_To_RightServo].transform.gameObject.SetActive (false);
		wires[LeftIR_To_RightServo].transform.gameObject.SetActive (false);
		wires[RightLDR_To_RightServo].transform.gameObject.SetActive (false);
		wires[LeftLDR_To_RightServo].transform.gameObject.SetActive (false);
		excitatoryButton.transform.gameObject.SetActive (false);
		inhibitoryButton.transform.gameObject.SetActive (false);
		leftServoButton.interactable = false;
		rightServoButton.interactable = false;

		for (int i = 0; i < NUM_WIRES; i++) {
			connections[i] = 0;
		}

		selectedPlatform = ProjectManager.GetPlatform ();
	}
	/****************************************************************/
	public void OnLeftIRClick(){
		//initiatedConnection = true;
		rightIRButton.interactable = false;
		leftLDRButton.interactable = false;
		rightLDRButton.interactable = false;
		leftServoButton.interactable = true;
		rightServoButton.interactable = true;
		wireInput = LeftIR;
		Debug.Log ("OnLeftIRClick " + wireInput);
	}
	/****************************************************************/
	public void OnRightIRClick(){
		//initiatedConnection = true;
		leftIRButton.interactable = false;
		leftLDRButton.interactable = false;
		rightLDRButton.interactable = false;
		leftServoButton.interactable = true;
		rightServoButton.interactable = true;
		wireInput = RightIR;
		Debug.Log ("OnRightIRClick " + wireInput);
	}
	/****************************************************************/
	public void OnLeftLDRClick(){
		//initiatedConnection = true;
		leftIRButton.interactable = false;
		rightIRButton.interactable = false;
		rightLDRButton.interactable = false;
		leftServoButton.interactable = true;
		rightServoButton.interactable = true;
		wireInput = LeftLDR;
		Debug.Log ("OnLeftLDRClick " + wireInput);
	}
	/****************************************************************/
	public void OnRightLDRClick(){
		//initiatedConnection = true;
		leftIRButton.interactable = false;
		rightIRButton.interactable = false;
		leftLDRButton.interactable = false;
		leftServoButton.interactable = true;
		rightServoButton.interactable = true;
		wireInput = RightLDR;
		Debug.Log ("OnRightIRClick " + wireInput);
	}
	/****************************************************************/
	public void OnLeftServoClick(){
		//initiatedConnection = false;
		leftIRButton.interactable = true;
		rightIRButton.interactable = true;
		leftLDRButton.interactable = true;
		rightLDRButton.interactable = true;
		leftServoButton.interactable = false;
		rightServoButton.interactable = false;
		Debug.Log ("OnLeftServoClick " + wireInput);
		wireOutput = LeftServo;
		excitatoryButton.transform.gameObject.SetActive (true);
		inhibitoryButton.transform.gameObject.SetActive (true);
		switch (wireInput) {
		case RightIR:
			wires [RightIR_To_LeftServo].transform.gameObject.SetActive (true);
			break;
		case LeftIR:
			wires [LeftIR_To_LeftServo].transform.gameObject.SetActive (true);
			break;
		case RightLDR:
			wires [RightLDR_To_LeftServo].transform.gameObject.SetActive (true);
			break;
		case LeftLDR:
			wires [LeftLDR_To_LeftServo].transform.gameObject.SetActive (true);
			break;
		default:
			break;
		}
	}
	/****************************************************************/
	public void OnRightServoClick(){
		//initiatedConnection = false;
		leftIRButton.interactable = true;
		rightIRButton.interactable = true;
		leftLDRButton.interactable = true;
		rightLDRButton.interactable = true;
		leftServoButton.interactable = false;
		rightServoButton.interactable = false;
		excitatoryButton.transform.gameObject.SetActive (true);
		inhibitoryButton.transform.gameObject.SetActive (true);
		Debug.Log ("OnRightServoClick " + wireInput);
		wireOutput = RightServo;
		switch (wireInput) {
		case RightIR:
			wires [RightIR_To_RightServo].transform.gameObject.SetActive (true);
			break;
		case LeftIR:
			wires [LeftIR_To_RightServo].transform.gameObject.SetActive (true);
			break;
		case RightLDR:
			wires [RightLDR_To_RightServo].transform.gameObject.SetActive (true);
			break;
		case LeftLDR:
			wires [LeftLDR_To_RightServo].transform.gameObject.SetActive (true);
			break;
		default:
			break;
		}
	}
	/****************************************************************/
	public void OnExcitatoryClick(){
		if (wireOutput == RightServo) {
			switch (wireInput) {
			case RightIR:
				wires [RightIR_To_RightServo].transform.GetComponent<SpriteRenderer> ().color = Color.green;
				connections [RightIR_To_RightServo] = 1;
				break;
			case LeftIR:
				wires [LeftIR_To_RightServo].transform.GetComponent<SpriteRenderer> ().color = Color.green;
				connections [LeftIR_To_RightServo] = 1;
				break;
			case RightLDR:
				wires [RightLDR_To_RightServo].transform.GetComponent<SpriteRenderer> ().color = Color.green;
				connections [RightLDR_To_RightServo] = 1;
				break;
			case LeftLDR:
				wires [LeftLDR_To_RightServo].transform.GetComponent<SpriteRenderer> ().color = Color.green;
				connections [LeftLDR_To_RightServo] = 1;
				break;
			default:
				break;
			}
		} else if (wireOutput == LeftServo) {
			switch (wireInput) {
			case RightIR:
				wires [RightIR_To_LeftServo].transform.GetComponent<SpriteRenderer> ().color = Color.green;
				connections [RightIR_To_LeftServo] = 1;
				break;
			case LeftIR:
				wires [LeftIR_To_LeftServo].transform.GetComponent<SpriteRenderer>().color = Color.green;
				connections [LeftIR_To_LeftServo] = 1;
				break;
			case RightLDR:
				wires [RightLDR_To_LeftServo].transform.GetComponent<SpriteRenderer>().color = Color.green;
				connections [RightLDR_To_LeftServo] = 1;
				break;
			case LeftLDR:
				wires [LeftLDR_To_LeftServo].transform.GetComponent<SpriteRenderer>().color = Color.green;
				connections [LeftLDR_To_LeftServo] = 1;
				break;
			default:
				break;
			}
		}
		inhibitoryButton.transform.gameObject.SetActive (false);
		excitatoryButton.transform.gameObject.SetActive (false);
	}
	/****************************************************************/
	public void OnInhibitoryClick(){
		if (wireOutput == RightServo) {
			switch (wireInput) {
			case RightIR:
				wires [RightIR_To_RightServo].transform.GetComponent<SpriteRenderer>().color = Color.red;
				connections [RightIR_To_RightServo] = -1;
				break;
			case LeftIR:
				wires [LeftIR_To_RightServo].transform.GetComponent<SpriteRenderer>().color = Color.red;
				connections [LeftIR_To_RightServo] = -1;
				break;
			case RightLDR:
				wires [RightLDR_To_RightServo].transform.GetComponent<SpriteRenderer>().color = Color.red;
				connections [RightLDR_To_RightServo] = -1;
				break;
			case LeftLDR:
				wires [LeftLDR_To_RightServo].transform.GetComponent<SpriteRenderer>().color = Color.red;
				connections [LeftLDR_To_RightServo] = -1;
				break;
			default:
				break;
			}
		} else if (wireOutput == LeftServo) {
			switch (wireInput) {
			case RightIR:
				wires [RightIR_To_LeftServo].transform.GetComponent<SpriteRenderer>().color = Color.red;
				connections [RightIR_To_LeftServo] = -1;
				break;
			case LeftIR:
				wires [LeftIR_To_LeftServo].transform.GetComponent<SpriteRenderer>().color = Color.red;
				connections [LeftIR_To_LeftServo] = -1;
				break;
			case RightLDR:
				wires [RightLDR_To_LeftServo].transform.GetComponent<SpriteRenderer>().color = Color.red;
				connections [RightLDR_To_LeftServo] = -1;
				break;
			case LeftLDR:
				wires [LeftLDR_To_LeftServo].transform.GetComponent<SpriteRenderer>().color = Color.red;
				connections [LeftLDR_To_LeftServo] = -1;
				break;
			default:
				break;
			}

		}
		inhibitoryButton.transform.gameObject.SetActive (false);
		excitatoryButton.transform.gameObject.SetActive (false);
	}
	/****************************************************************/
	public void OnHomeClick(){
		SceneManager.LoadScene ("PlatformSelection");
	}
	/****************************************************************/
	public void OnDownloadClick(){

		string platformDirectoryPath = Directory.GetCurrentDirectory() + "/RobotCode";
		string platformFileName = platformDirectoryPath + "/Template Code Files/Vehicle Temp/Link/BV_TEMP_LINK.c";
		string vehicleFileName = platformDirectoryPath + "/Generated Code Files/Vehicle/Link/BV_Link.c";

		switch (selectedPlatform) {
		case "Simulation":
			break;
		case "Arduino":
			platformDirectoryPath = Directory.GetCurrentDirectory() + "/RobotCode";
			platformFileName = platformDirectoryPath + "/Template Code Files/Vehicle Temp/Arduino/BV_TEMP_ARDUINO/BV_TEMP_ARDUINO.ino";
			vehicleFileName = platformDirectoryPath + "/Generated Code Files/Vehicle/BV_ARDUINO/BV_ARDUINO.ino";
			break;
		case "KIPR Link":
			platformFileName = platformDirectoryPath + "/Template Code Files/Vehicle Temp/Link/BV_TEMP_LINK.c";
			vehicleFileName = platformDirectoryPath + "/Generated Code Files/Vehicle/Link/BV_LINK.c";
			break;
		case "EZ Robot":
			break;
		default:
			platformFileName = platformDirectoryPath + "/Template Code Files/Vehicle Temp/Link/BV_TEMP_LINK.c";
			vehicleFileName = platformDirectoryPath + "/Generated Code Files/Vehicle/Link/BV_LINK.c";
			break;
		}
			
		System.Text.StringBuilder vehicleBuilder = new System.Text.StringBuilder();

		bool linkDirectoryExists = System.IO.Directory.Exists(platformDirectoryPath);

		Debug.Log ("OnDowloadClick()");
		for (int i = 0; i < NUM_WIRES; i++) {
			Debug.Log (connections [i]);
		}
		if (linkDirectoryExists == false) {
			System.IO.Directory.CreateDirectory (platformDirectoryPath);
		} 
		bool linkFileExists = System.IO.File.Exists (platformFileName);
		Debug.Log ("experimentFileExists? " + linkFileExists + " for file " + platformFileName);
		if (linkFileExists == false) {
			System.IO.File.Create (platformFileName);
		}

		vehicleBuilder.Append (" = {{");
		vehicleBuilder.Append (connections [LeftLDR_To_LeftServo].ToString ());
		vehicleBuilder.Append (", ");
		vehicleBuilder.Append (connections [LeftLDR_To_RightServo].ToString ());
		vehicleBuilder.Append ("}, {");
		vehicleBuilder.Append (connections [RightLDR_To_LeftServo].ToString ());
		vehicleBuilder.Append (", ");
		vehicleBuilder.Append (connections [RightLDR_To_RightServo].ToString ());
		vehicleBuilder.Append ("}, {");
		vehicleBuilder.Append (connections [LeftIR_To_LeftServo].ToString ());
		vehicleBuilder.Append (", ");
		vehicleBuilder.Append (connections [LeftIR_To_RightServo].ToString ());
		vehicleBuilder.Append ("}, {");
		vehicleBuilder.Append (connections [RightIR_To_LeftServo].ToString ());
		vehicleBuilder.Append (", ");
		vehicleBuilder.Append (connections [RightIR_To_RightServo].ToString ());
		vehicleBuilder.Append ("}};");

		Debug.Log ("ReadAllText for " + platformFileName + "\n");

		string linkCode = System.IO.File.ReadAllText (platformFileName);
		Debug.Log ("Replace <<VEHICLE>> with the connections string.\n");
		string vehicleCode = linkCode.Replace ("<<VEHICLE>>", vehicleBuilder.ToString ());
		Debug.Log (vehicleCode);
		Debug.Log ("WriteAllText() to " + vehicleFileName + "\n");
		System.IO.File.WriteAllText (vehicleFileName, vehicleCode);
		//System.IO.File.OpenWrite
		//byte[] linkCodeByteArray = GetBytes(linkCode);
		//FileStream linkStream = System.IO.File.OpenWrite (platformFileName);
		//linkStream.Write (linkCodeByteArray, 0, linkCode.Length);

		//Record some data about this experiment before moving on.
		//System.IO.File.AppendAllText (experimentFileName, experimentalDataString);
		//Debug.Log("delimiterIndx = " + delimiterIndx.ToString());

	}

}
