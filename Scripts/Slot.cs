using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler{

	public GameObject behavior{
		get{
			if (transform.childCount > 0) {
				return transform.GetChild (0).gameObject;
			}
			return null;
		}
	}

	#region IDropHandler implementation
	public void OnDrop (PointerEventData eventData)
	{
		string slotPanel = transform.parent.name;
		if (slotPanel == "BehaviorHierarchy") {
			Debug.Log ("Attempt Drop of " + transform.name + " into " + slotPanel);
		}
		if (!behavior) {
			DragHandler.behaviorBeingDragged.transform.SetParent (transform);
			ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasChanged());
			Debug.Log ("Dragging " + transform.parent.name + " at " + transform.position);
		}
	}
	#endregion

}
