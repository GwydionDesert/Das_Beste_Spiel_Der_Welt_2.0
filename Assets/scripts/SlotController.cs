using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotController : MonoBehaviour {
	
	void OnMouseEnter(){
		transform.parent.GetComponent<InventoryController>().selectedSlot = this.transform;
	}

	void OnMouseExit() { 
		transform.parent.GetComponent<InventoryController>().selectedSlot = null; 
	}
}
