using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class ItemDB : MonoBehaviour {

	public static List<Item> itemList = new List<Item>();

	public Sprite[] sprite;

	void Awake () {
		// add Items
		Item i0 = new Item("Candy 01", sprite[0]);
		Item i1 = new Item("Candy 02", sprite[1]);
		Item i2 = new Item("Candy 03", sprite[2]);

		itemList.Add(i0);
		itemList.Add(i1);
		itemList.Add(i2);
	}
}
