using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class FieldsParameterHandler : MonoBehaviour {

	const int NUM_SENSORS = 6;
	public Toggle[] fieldToggles = new Toggle[NUM_SENSORS];
	//Text fieldToggleText;

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
	//Output the new state of the Toggle into Text
	public void ToggleValueChanged(Toggle change)
	{
		if (change.isOn == false) {
			Debug.Log (change.name);
			change.transform.GetChild(1).GetComponent<Text> ().color = Color.black;
		}
		//frontLightPanel.GetComponent<Tog
		//fieldToggleText.text =  "New Value : " + change.isOn;
	}
	/****************************************************************/
	public void OnExcitatoryClick(string whichSensor){
		Debug.Log ("OnExcitatoryClick(), which sensor?");
		Debug.Log (whichSensor);
		string whichToggle = whichSensor.Replace("Panel", "Toggle");
		Toggle currentToggle = GameObject.Find(whichToggle).GetComponent<Toggle> ();
		Debug.Log (whichToggle);
		Debug.Log (currentToggle.name);
		if(currentToggle.isOn){
			currentToggle.transform.GetChild (1).GetComponent<Text> ().color = Color.green;
		}

	}
	/****************************************************************/
	public void OnInhibitoryClick(string whichSensor){
		string whichToggle = whichSensor.Replace("Panel", "Toggle");
		Toggle currentToggle = GameObject.Find(whichToggle).GetComponent<Toggle> ();
		Debug.Log (whichToggle);
		Debug.Log (currentToggle.name);
		if(currentToggle.isOn){
			currentToggle.transform.GetChild (1).GetComponent<Text> ().color = Color.red;
		}

	}
	/****************************************************************/
	public void OnGenerateClick(){

		int lightCoeff 			= 0;
		int irCoeff				= 0;
		int frontBumpCoeff		= 0;
		int backBumpCoeff		= 0;
		int [] fieldCoeffs		= new int[4];

		Debug.Log ("OnGenerateClick()");

		if (fieldToggles [0].transform.GetChild (1).GetComponent<Text> ().color == Color.red) {
			lightCoeff = -1;
		} else if (fieldToggles [0].transform.GetChild (1).GetComponent<Text> ().color == Color.green) {
			lightCoeff = 1;
		} else {
			lightCoeff = 0;
		}

		if (fieldToggles [1].transform.GetChild (1).GetComponent<Text> ().color == Color.red) {
			irCoeff = -1;
		} else if (fieldToggles [1].transform.GetChild (1).GetComponent<Text> ().color == Color.green) {
			irCoeff = 1;
		} else {
			irCoeff = 0;
		}

		if (fieldToggles [2].transform.GetChild (1).GetComponent<Text> ().color == Color.red) {
			frontBumpCoeff = -1;
		} else if (fieldToggles [2].transform.GetChild (1).GetComponent<Text> ().color == Color.green) {
			frontBumpCoeff = 1;
		} else {
			frontBumpCoeff = 0;
		}

		if (fieldToggles [5].transform.GetChild (1).GetComponent<Text> ().color == Color.red) {
			backBumpCoeff = -1;
		} else if (fieldToggles [5].transform.GetChild (1).GetComponent<Text> ().color == Color.green) {
			backBumpCoeff = 1;
		} else {
			backBumpCoeff = 0;
		}
			
		fieldCoeffs [0] = backBumpCoeff;
		fieldCoeffs [1] = frontBumpCoeff;
		fieldCoeffs [2] = irCoeff;
		fieldCoeffs [3] = lightCoeff;

		switch (selectedPlatform) {
		case "Simulation":
			break;
		case "Arduino":
			break;
		case "KIPR Link":
			BuildLinkFields (fieldCoeffs);
			break;
		case "EZ Robot":
			break;
		default:
			BuildLinkFields (fieldCoeffs);
			break;
		}

	}
	/****************************************************************/
	private void BuildLinkFields(int [] fieldCoeffs){
		string linkDirectoryPath = Directory.GetCurrentDirectory() + "/RobotCode/Template Code Files/Virtual Fields Temp/Link";
		string linkFieldsPath = Directory.GetCurrentDirectory() + "/RobotCode/Generated Code Files/Virtual Fields/Link";
		string linkFileName = linkDirectoryPath + "/VF_TEMP_LINK.c";
		string fieldsFileName = linkFieldsPath + "/VFLINK.c";

		Debug.Log ("ReadAllText for " + linkFileName + "\n");
		string templateCode = System.IO.File.ReadAllText (linkFileName);
		string generatedCode = templateCode;
		generatedCode = generatedCode.Replace ("<<BBC>>", fieldCoeffs[0].ToString ());
		generatedCode = generatedCode.Replace ("<<FBC>>", fieldCoeffs [1].ToString ());
		generatedCode = generatedCode.Replace ("<<IRC>>", fieldCoeffs [2].ToString ());
		generatedCode = generatedCode.Replace ("<<LDC>>", fieldCoeffs [3].ToString ());

		bool linkDirectoryExists = System.IO.Directory.Exists(linkDirectoryPath);

		Debug.Log ("BuildLinkFields()");
		
		bool linkFileExists = System.IO.File.Exists (fieldsFileName);
		Debug.Log ("experimentFileExists? " + linkFileExists + " for file " + fieldsFileName);
		if (linkFileExists == false) {
			System.IO.File.Create (fieldsFileName);
		}

		System.IO.File.WriteAllText (fieldsFileName, generatedCode);
		Debug.Log (generatedCode);	

	}

}
