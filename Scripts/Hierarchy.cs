using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Hierarchy : MonoBehaviour, IHasChanged{
	[SerializeField] Transform slots;
	public GameObject hierarchyTextGameObject;
	public static Text hierarchyText;
	// Use this for initialization
	void Start () {
		hierarchyText = hierarchyTextGameObject.GetComponent<Text> ();
		HasChanged ();
	}

	#region IHasChanged implementation
	public void HasChanged ()
	{
		System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder ();
		stringBuilder.Append ("-");
		foreach (Transform slotTransform in slots) {
			GameObject behavior = slotTransform.GetComponent<Slot> ().behavior;
			if (behavior) {
				stringBuilder.Append (behavior.name);
				stringBuilder.Append ("-");
			}
		}
		hierarchyText.text = stringBuilder.ToString ();
	}
	#endregion

}

namespace UnityEngine.EventSystems{
	public interface IHasChanged : IEventSystemHandler	{
		void HasChanged ();
	}
}