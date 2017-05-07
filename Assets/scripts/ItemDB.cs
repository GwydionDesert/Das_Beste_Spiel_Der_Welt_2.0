using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class ItemDB : MonoBehaviour {

	public static List<Item> itemList = new List<Item>();

	public Sprite[] sprite;

	void Awake () {
		// add Items
		Item i0 = new Item("Sword", sprite[0]);
		//i0.type = Item.Type.equip;
		Item i1 = new Item("Sword_2", sprite[1]);
		Item i2 = new Item("HP", sprite[2]);

		itemList.Add(i0);
		itemList.Add(i1);
		itemList.Add(i2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
