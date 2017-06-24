using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainQuest : MonoBehaviour {

	private int count = 0;
	void Start () {
		Invoke("checkMap", 0.1f);
	}
	
	private void checkMap(){
		foreach(Transform t in transform){
			if (t.gameObject.GetComponent<SpriteRenderer>().enabled){
				count ++;
			}
		}

		if (count == transform.childCount){
			Cursor.visible = false;
			Camera.main.GetComponent<onClick>().lastHit = gameObject;
			Camera.main.GetComponent<onClick>().state = onClick.State.idle;
			GetComponent<Quest>().questLine();
		}
	}
}
