using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    public string nextScene;

    AsyncOperation asyncLoadLevel;
	public void changeScene()
    {
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }

    public static void cleanScene(){
        // delete all objects which are in Inventory
        Dictionary<string, bool> d = GameObject.Find("GM").GetComponent<GM>().objectInInventory;
        foreach(string key in d.Keys)
        {
            Debug.Log(key);
            if (d[key] == true){
                if (GameObject.Find(key) != null){
                    Destroy(GameObject.Find(key).gameObject);
                    Debug.Log("Destroy: " + GameObject.Find(key).name);
                }
            }
        }
    }
}
