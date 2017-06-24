using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

	public string nextScene;

	public void changeScene()
	{
		SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
	}

	public static void changeScene(string scene)
	{
		SceneManager.LoadScene(scene, LoadSceneMode.Single);
	}

	public static void cleanScene(){
		// delete all objects which are in Inventory
		Dictionary<string, bool> d = GameObject.Find("GM").GetComponent<GM>().objectInInventory;
		foreach(string key in d.Keys){
			if (d[key] == true){
				if (GameObject.Find(key) != null){
					Destroy(GameObject.Find(key).gameObject);
				}
			}
		}

		if (GameObject.Find("Kiste")!= null){
			if (GameObject.Find("Kiste").GetComponent<Quest>().state >= 1){
				GameObject.Find("Kiste").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/Kiste_offen");
				Destroy(GameObject.Find("Kiste").GetComponent<Quest>());
				Destroy(GameObject.Find("Kiste").GetComponent<PolygonCollider2D>());
			}
		}
	}
}