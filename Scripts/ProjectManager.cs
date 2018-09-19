using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectManager : MonoBehaviour {

	const string PLATFORM_KEY  = "platform";
	const string FRAMEWORK_KEY = "framework";

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
	}
	
	public static void SetPlatform(string platform){
		PlayerPrefs.SetString (PLATFORM_KEY, platform);
	}
	public static void SetFramework(string framework){
		PlayerPrefs.SetString (FRAMEWORK_KEY, framework);
	}

	public static string GetPlatform(){
		return PlayerPrefs.GetString (PLATFORM_KEY);
	}
	public static string GetFramework(){
		return PlayerPrefs.GetString (FRAMEWORK_KEY);
	}
}
