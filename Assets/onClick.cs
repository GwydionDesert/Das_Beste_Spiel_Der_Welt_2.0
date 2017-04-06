using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onClick : MonoBehaviour {

    GameObject lastHit;

	void Update () {
        {
            // get mouse position to world space
            Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

            Debug.DrawRay(new Vector3 (0,0,0), rayPos);

            // check if object has a sprite renderer
            // mostly only called because of else block
            try
            {
                // on mouse over
                if (hit)
                {

                    lastHit = hit.transform.gameObject;
                    lastHit.GetComponent<SpriteRenderer>().color = Color.red;
                    Debug.Log(hit.transform.name);
                    Debug.Log(hit.transform.gameObject);

                }
                else
                {
                    lastHit.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
            catch
            {
                //Debug.Log("no SpriteRenderer found");
            }
        }
    }
}
