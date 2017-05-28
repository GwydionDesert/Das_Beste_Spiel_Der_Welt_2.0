using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	public string name;
	public Sprite image;

	public bool isDragged;
	public bool inInventory;

	public Item(string name, Sprite image){
		this.name = name;
		this.image = image;
	}

	void OnMouseEnter(){
		if (inInventory)
			transform.parent.parent.GetComponent<InventoryController>().selectedItem = this.transform;
	}

	void OnMouseExit() { 
		if (inInventory){
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
}
