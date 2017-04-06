using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onClick : MonoBehaviour {

    public GameObject text;

    private GameObject lastHit;
    private DisplayText displayScript;
    
    void Update () {
        {
            // get mouse position to world space
            Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

            // stop interaction mode
            if (Cursor.visible == false && Input.GetButtonDown("Fire1"))
            {
                lastHit.transform.gameObject.GetComponent<DisplayText>().stop();
                Cursor.visible = true;
                return;
            }

            // check if object has a sprite renderer
            // mostly only called because of else block
            try
            {
                // on mouse over
                if (hit)
                {
                    // on mouse over highlight object
                    lastHit = hit.transform.gameObject;
                    lastHit.GetComponent<SpriteRenderer>().color = Color.red;

                    if (Input.GetButtonDown("Fire1"))
                    {

                        // show object description
                        // start interaction mode
                        if (Cursor.visible)
                        {
                            hit.transform.gameObject.GetComponent<DisplayText>().display();
                            Cursor.visible = false;
                        }
                    }
                }
                else
                {
                    // reset highlighting
                    lastHit.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
            catch
            {
                Debug.Log("no SpriteRenderer found");
            }
        }
    }
}


/*  Interaction Mode:
 *          the player interacts with an object/ NPC -> object specific methods run
 *          cursor is hidden
 *          fire 1 -> exit Interaction Mode/ interaction specific method handles input
 */