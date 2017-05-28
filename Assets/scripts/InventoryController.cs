using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventoryController : MonoBehaviour {

	[HideInInspector]
	public Transform selectedItem, selectedSlot, originalSlot;

	public GameObject slotPrefab, itemPrefab;
	public Vector2 inventorySize = new Vector2(4,6);
	public float slotSize;
	public Vector2 windwoSize;

	public bool canDragItem;

	void Awake () {
		// create Inventory
		for (int y = 1; y <= inventorySize.y; y++){
			for (int x = 1; x <= inventorySize.x; x++){
				GameObject slot = Instantiate(slotPrefab) as GameObject;
				slot.transform.SetParent(this.transform, false);
				slot.name = "slot_" + (inventorySize.x * (y - 1) + x);
				slot.GetComponent<RectTransform>().anchoredPosition = new Vector3(windwoSize.x / (inventorySize.x + 1) * x, windwoSize.y / (inventorySize.y + 1)* -y, 0);
			}
		}
		this.gameObject.SetActive(false);
	}
	
	void Update () {
		// grab Item
		if (Input.GetMouseButtonDown(0) && selectedItem != null){
			canDragItem = true;
			selectedItem.GetComponent<Item> ().isDragged = true;
			originalSlot = selectedItem.parent;
			setItemColliders(false);
		}

		// drag Item
		if (Input.GetMouseButton(0) && selectedItem != null && canDragItem){
			selectedItem.position = new Vector3 (Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 100f);
			selectedItem.transform.SetParent(originalSlot.parent);
		}
		// let go of Item			-> 	move to Slot
		else if(Input.GetMouseButtonUp(0) && selectedItem != null){
			canDragItem = false;
			selectedItem.GetComponent<Item> ().isDragged = false;
			if (selectedSlot == null) {
				selectedItem.SetParent(originalSlot);
			}
			else{
				// combine Items
				if (selectedSlot.childCount > 0){
					Debug.Log("Fusion: " + selectedItem.name + " + " + selectedSlot.GetChild(0).name);
					selectedItem.SetParent(originalSlot);
					
				}
				else {
					selectedItem.SetParent(selectedSlot);
				}
			}
			if (originalSlot.childCount > 0){
				originalSlot.GetChild(0).localPosition = Vector3.zero;
			}
			selectedItem.localPosition = Vector3.zero;
			setItemColliders(true);
		}
	}

	public void addItem(Item i){
		for (int x = 0; x < inventorySize.x; x++){
			for (int y = 0; y < inventorySize.y; y++){
				if (this.transform.GetChild(x  + (y * (int) inventorySize.y)).childCount == 0){//.transform.childCount == 0){
					GameObject item = Instantiate(itemPrefab) as GameObject;
					item.transform.SetParent(this.transform.GetChild(x  + (y * (int) inventorySize.y)).transform, false);
					item.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

					item.name = i.name;
					item.GetComponent<Image>().sprite = i.image;
					item.GetComponent<Item>().inInventory = true;

					return;
				}
			}
		}
	}

	public void setItemColliders(bool state){
		foreach(GameObject item in GameObject.FindGameObjectsWithTag("Item")){
			item.GetComponent<BoxCollider>().enabled = state;
		}
	}
}
