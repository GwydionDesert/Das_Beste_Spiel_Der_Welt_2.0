﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class InventoryController : MonoBehaviour {

	[HideInInspector]
	public Transform selectedItem, selectedSlot, originalSlot;

	public GameObject slotPrefab, itemPrefab;
	public Vector2 inventorySize = new Vector2(4,6);
	public Vector2 windowSize;

	public bool canDragItem;

	public GameObject text;

	private string[,] combo;
	private int comboPos;

	public Vector3 offset = new Vector3(0,1,0);
	private float textScale = 80;

	void Awake () {
		// create Inventory
		for (int y = 1; y <= inventorySize.y; y++){
			for (int x = 1; x <= inventorySize.x; x++){
				GameObject slot = Instantiate(slotPrefab) as GameObject;
				slot.transform.SetParent(this.transform, false);
				slot.name = "slot_" + (inventorySize.x * (y - 1) + x);
				slot.GetComponent<RectTransform>().anchoredPosition = new Vector3(windowSize.x / (inventorySize.x + 1) * x, windowSize.y / (inventorySize.y + 1)* -y, 0);
			}
		}

		combo = GM.gm.combo;

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
					GameObject item1 = selectedItem.gameObject;
					GameObject item2 = selectedSlot.GetChild(0).gameObject;

					for (int i = 0; i < combo.Length / 4; i ++){
						if (combo[0,i].Equals(item1.name) && combo[1,i].Equals(item2.name)){
								comboPos = i;
								combineItems(item1, item2);
								break;
						}
						else if (combo[0,i].Equals(item2.name) && combo[1,i].Equals(item1.name)){
							comboPos = i;
							combineItems(item2, item1);
							break;
						}
						else {
							selectedItem.SetParent(originalSlot);
							displayText("So wird das nichts.");
						}
					}
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

	private void combineItems(GameObject item1, GameObject item2){
		if (comboPos >= 0){
			// show Text
			displayText(combo[3, comboPos]);

			GameObject comboItem = Instantiate(itemPrefab) as GameObject;
			comboItem.transform.SetParent(selectedSlot.transform, false);
			comboItem.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
			comboItem.name = combo[2, comboPos];

			if (Resources.Load<Sprite>("Icons/" + combo[2, comboPos]) != null){
				comboItem.GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/" + combo[2, comboPos]);
			}
			else{
				Debug.Log("Icon not found: Icons/" + combo[2, comboPos]);
			}
			comboItem.GetComponent<Item>().inInventory = true;

			Destroy(item1);
			Destroy(item2);

			comboPos = -1;
		}
	}

	public void displayText (string s){
		// clear shown text
		foreach (Transform child in transform){
			if (child.gameObject.name.Contains("(Clone)")){
				Destroy(child.gameObject);
				StopAllCoroutines();								// could cause PROBLEMS with outher COROUTINES!!!!!!
			}
		}
		GameObject textInstance = Instantiate(text, new Vector3 (transform.position.x, transform.position.y, -2.0f), transform.rotation, transform);
		textInstance.transform.localScale = new Vector3 ((textInstance.transform.localScale.x / transform.localScale.x) * textScale,
														 (textInstance.transform.localScale.y / transform.localScale.y) * textScale, 0f);
		textInstance.transform.position += offset;
		TextMeshPro textMP = textInstance.GetComponent<TextMeshPro> ();
		textMP.text = s;

		StartCoroutine(fadeText(textMP));
	}

	public void addItem(Item i){
		for (int j = 0; j < inventorySize.x * inventorySize.y; j++){
			if (this.transform.GetChild(j).childCount == 0){
				GameObject item = Instantiate(itemPrefab) as GameObject;
				item.transform.SetParent(this.transform.GetChild(j).transform, false);
				item.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

				item.name = i.name;
				item.GetComponent<Image>().sprite = i.image;
				item.GetComponent<Item>().inInventory = true;

				if (!GameObject.Find("GM").GetComponent<GM>().objectInInventory.ContainsKey(i.name)){
					GameObject.Find("GM").GetComponent<GM>().objectInInventory.Add(i.name, true);
				}

				return;
			}
		}
	}

	public static IEnumerator fadeText (TextMeshPro textMP) {
		while (textMP.alpha > 0) {
			textMP.alpha -= Time.deltaTime / 3;	// fade time
			yield return null;
        }
		Destroy(textMP.gameObject);
	}

	public void setItemColliders(bool state){
		foreach(GameObject item in GameObject.FindGameObjectsWithTag("Item")){
			item.GetComponent<BoxCollider>().enabled = state;
		}
	}

	public void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawCube(new Vector3 (transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z), new Vector3(0.2f, 1, 1));
	}
}
