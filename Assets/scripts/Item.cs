using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	public string name;
	//public enum Type {equip, consumables, misc};
	//public Type type;
	public Sprite image;

	public Item(string name, Sprite image){
		this.name = name;
		this.image = image;
	}

	void OnMouseEnter(){
		transform.parent.parent.GetComponent<InventoryController>().selectedItem = this.transform;
	}

	void OnMouseExit() { 
		if (!transform.parent.parent.GetComponent<InventoryController>().canDragItem){
			transform.parent.parent.GetComponent<InventoryController>().selectedItem = null; 
		}
	}
}
