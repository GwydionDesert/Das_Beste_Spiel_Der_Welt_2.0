using UnityEngine;
using UnityEngine.SceneManagement;

public class onClick : MonoBehaviour {

	public GameObject lastHit;
	private Texture2D cursorActive;
	private Texture2D cursorInactive;

	public enum State {idle, hit, interact, quest, changeScene, stop};
	public State state = State.idle;

	private void Start(){
		cursorActive = GameObject.Find("GM").gameObject.GetComponent<GM>().cursorActive;
		cursorInactive = GameObject.Find("GM").gameObject.GetComponent<GM>().cursorInactive;
	}

	private void Update () {
		{
			switch (state){
				case (State.idle):
					// get mouse position to world space
					Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
					RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

					if (lastHit != null && Input.GetButtonDown("Fire1")){
						state = State.hit;
					}

					if (hit && Cursor.visible == true)
					{
						// on mouse over change cursor icon
						Cursor.SetCursor(cursorActive, Vector2.zero, CursorMode.Auto);

						// start interaction with object or npc
						if (Input.GetButtonDown("Fire1")){
							state = State.hit;
							lastHit = hit.transform.gameObject;
						}
					}
					else
					{
						// reset cursor
						Cursor.SetCursor(cursorInactive, Vector2.zero, CursorMode.Auto);
					}
					break;

				case (State.hit):
						// show object description
						if (lastHit.GetComponent<DisplayText>() != null)
							state = State.interact;

						if (lastHit.GetComponent<Quest>() != null)
							state = State.quest;

						if (lastHit.GetComponent<ChangeScene>() != null)
							state = State.changeScene;
					break;

				case (State.interact):
					if (lastHit != null){
						Cursor.visible = false;
						lastHit.GetComponent<DisplayText>().display();
						lastHit.GetComponent<DisplayText>().iText ++;

						if (lastHit.GetComponent<DisplayText>().iText > 0)
						{
							state = State.idle;
						}
						else{
							state = State.stop;
						}
					}
					else {
						state = State.stop;
					}
					break;
			
				case (State.quest):
					if (lastHit != null){
						Cursor.visible = false;
						lastHit.GetComponent<Quest>().questLine();
						lastHit.GetComponent<Quest>().iText ++;

						if (lastHit.GetComponent<Quest>().iText > 0)
						{
							state = State.idle;
						}
						else{
							state = State.stop;
							lastHit = null;
						}
					}
					else {
						state = State.stop;
					}
					break;


				case (State.changeScene):
					if (lastHit != null){
						lastHit.GetComponent<ChangeScene>().changeScene();
					}

					if (lastHit == null){
						state = State.idle;
						ChangeScene.cleanScene();
					}
					
					break;

				// end interaction mode
				case (State.stop):
					Cursor.visible = true;
					lastHit = null;
					state = State.idle;
					break;

				default:
					Debug.Log("state unknown, state: " + state);
				break;
			}
		}
	}
}


/*	Interaction Mode:
*			the player interacts with an object/ NPC -> object specific methods run
*			cursor is hidden
*			fire 1 -> exit Interaction Mode/ interaction specific method handles input
 */