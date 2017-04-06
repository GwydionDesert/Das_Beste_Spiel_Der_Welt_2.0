using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onClick : MonoBehaviour {

    GameObject lastHit;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        {
            // get mouse position to world space
            Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

            // on hit
            if (hit)
            {
                lastHit = hit.transform.gameObject;
                lastHit.GetComponent<SpriteRenderer>().color = Color.green;
                Debug.Log(hit.transform.name);
                Debug.Log( hit.transform.gameObject);
            }
            else
            {
                lastHit.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
}
