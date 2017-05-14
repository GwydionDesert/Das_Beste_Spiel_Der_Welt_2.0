using UnityEngine;

public class onClick : MonoBehaviour {

    private GameObject lastHit;
    private Texture2D cursorActive;
    private Texture2D cursorInactive;

    private void Start()
    {
        cursorActive = GameObject.Find("GM").gameObject.GetComponent<GM>().cursorActive;
        cursorInactive = GameObject.Find("GM").gameObject.GetComponent<GM>().cursorInactive;
    }

    void Update () {
        {
            // get mouse position to world space
            Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

            // stop interaction mode    -   description
            if (Cursor.visible == false && Input.GetButtonDown("Fire1"))
            {
                lastHit.transform.gameObject.GetComponent<DisplayText>().iText ++;
                lastHit.transform.gameObject.GetComponent<DisplayText>().display();
            }

            // on mouse over
            if (hit)
            {
                // change cursor icon
                Cursor.SetCursor(cursorActive, Vector2.zero, CursorMode.Auto);

                if (Input.GetButtonDown("Fire1"))
                {
                    lastHit = hit.transform.gameObject;

                    // show object description
                    // enter Interaction Mode
                    if (Cursor.visible && hit.transform.GetComponent<DisplayText>() != null)
                    {
                        hit.transform.gameObject.GetComponent<DisplayText>().display();
                        Cursor.visible = false;
                    }

                    if (hit.transform.GetComponent<ChangeScene>() != null)
                    {
                        hit.transform.gameObject.GetComponent<ChangeScene>().changeScene();
                    }
                }
            }
            else
            {
                // reset cursor
                Cursor.SetCursor(cursorInactive, Vector2.zero, CursorMode.Auto);
            }
        }
    }
}


/*  Interaction Mode:
 *          the player interacts with an object/ NPC -> object specific methods run
 *          cursor is hidden
 *          fire 1 -> exit Interaction Mode/ interaction specific method handles input
 */