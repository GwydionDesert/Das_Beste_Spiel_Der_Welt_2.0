using UnityEngine;

public class onClick : MonoBehaviour {

    private GameObject lastHit;
    private Texture2D cursorActive;
    private Texture2D cursorInactive;

    public enum State {idle, hit, interact, changeScene, stop};
    [HideInInspector]
    public int state = 0;

    private void Start()
    {
        cursorActive = GameObject.Find("GM").gameObject.GetComponent<GM>().cursorActive;
        cursorInactive = GameObject.Find("GM").gameObject.GetComponent<GM>().cursorInactive;
    }

    void Update () {
        {
            switch (state){
                case ((int) State.idle):
                    //Cursor.visible = true;
                    // get mouse position to world space
                    Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
                    RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

                    if (lastHit != null && Input.GetButtonDown("Fire1")){
                        state = (int) State.hit;
                    }

                    if (hit && Cursor.visible == true)
                    {
                        // on mouse over change cursor icon
                        Cursor.SetCursor(cursorActive, Vector2.zero, CursorMode.Auto);

                        // start interaction with object or npc
                        if (Input.GetButtonDown("Fire1")){
                            state = (int) State.hit;
                            lastHit = hit.transform.gameObject;
                        }
                    }
                    else
                    {
                        // reset cursor
                        Cursor.SetCursor(cursorInactive, Vector2.zero, CursorMode.Auto);
                    }
                    break;

                case ((int) State.hit):
                        // show object description
                        if (lastHit.GetComponent<DisplayText>() != null)
                            state = (int) State.interact;

                        if (lastHit.GetComponent<ChangeScene>() != null)
                            state = (int) State.changeScene;
                    break;

                case ((int) State.interact):
                    Cursor.visible = false;
                    lastHit.GetComponent<DisplayText>().display();
                    lastHit.GetComponent<DisplayText>().iText ++;

                    if (lastHit.GetComponent<DisplayText>().iText > 0)
                    {
                        state = (int) State.idle;
                    }
                    else{
                        state = (int) State.stop;
                    }
                    break;

                case ((int) State.changeScene):
                    if (lastHit != null){
                        lastHit.GetComponent<ChangeScene>().changeScene();
                    }

                    if (lastHit == null){
                        state = (int) State.idle;
                        ChangeScene.cleanScene();
                    }
                    
                    break;

                // end interaction mode
                case ((int) State.stop):
                    Cursor.visible = true;
                    lastHit = null;
                    state = (int) State.idle;
                    break;

                default:
                    Debug.Log("state unknown, state: " + state);
                break;
            }
        }
    }
}


/*  Interaction Mode:
 *          the player interacts with an object/ NPC -> object specific methods run
 *          cursor is hidden
 *          fire 1 -> exit Interaction Mode/ interaction specific method handles input
 */