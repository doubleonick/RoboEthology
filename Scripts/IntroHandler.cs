using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class IntroHandler : MonoBehaviour {

	public void OnContinueClick(){
		SceneManager.LoadScene ("PlatformSelection");
	}
}
