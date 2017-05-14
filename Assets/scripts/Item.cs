using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	public string name;
	//public enum Type {equip, consumables, misc};
	//public Type type;
	public Sprite image;

	public bool isDragged;

	public Item(string name, Sprite image){
		this.name = name;
		this.image = image;
	}

	void OnMouseEnter(){
		transform.parent.parent.GetComponent<InventoryController>().selectedItem = this.transform;
	}

	void OnMouseExit() { 
		if (isDragged) {
			if (!transform.parent.GetComponent<InventoryController> ().canDragItem) {
				transform.parent.GetComponent<InventoryController> ().selectedItem = null; 
			}
		} else {
			if (!transform.parent.parent.GetComponent<InventoryController>().canDragItem){
				transform.parent.parent.GetComponent<InventoryController>().selectedItem = null; 
			}
		}
	}
}
