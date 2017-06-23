using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainQuest : MonoBehaviour {

	private int count = 0;
	void Start () {
		int i = 0;
		Transform g = GameObject.Find("UI").transform;
		while (g.GetChild(i) != null){
			if (g.GetChild(i).GetComponent<InventoryController>() != null){
				break;
			}
			i++;
		}

		Invoke("checkMap", 0.1f);
	}
	
	private void checkMap(){
		foreach(Transform t in transform){
			if (t.gameObject.GetComponent<SpriteRenderer>().enabled){
				count ++;
				Debug.Log(t.gameObject.name);
			}
		}

		
		if (count == transform.childCount){
			Cursor.visible = false;
			Camera.main.GetComponent<onClick>().lastHit = gameObject;
			Camera.main.GetComponent<onClick>().state = onClick.State.quest;
			GetComponent<Quest>().questLine();
		}
	}
}
