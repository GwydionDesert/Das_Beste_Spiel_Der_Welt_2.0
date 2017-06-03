using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Pause : MonoBehaviour {


	private ShowPanels showPanels;						//Reference to the ShowPanels script used to hide and show UI panels
	public bool isPaused;								//Boolean to check if the game is paused or not
	private StartOptions startScript;					//Reference to the StartButton script
	public bool inInventory;
	
	//Awake is called before Start()
	void Awake()
	{
		//Get a component reference to ShowPanels attached to this object, store in showPanels variable
		showPanels = GetComponent<ShowPanels> ();
		//Get a component reference to StartButton attached to this object, store in startScript variable
		startScript = GetComponent<StartOptions> ();
	}

	// Update is called once per frame
	void Update () {

		//Check if the Cancel button in Input Manager is down this frame (default is Escape key) and that game is not paused, and that we're not in main menu
		if (Input.GetButtonDown ("Cancel") && !isPaused && !inInventory && !startScript.inMainMenu) 
		{
			//Call the DoPause function to pause the game
			DoPause();
			return;
		} 
		//If the button is pressed and the game is paused and not in main menu
		else if (Input.GetButtonDown ("Cancel") && isPaused && !inInventory && !startScript.inMainMenu) 
		{
			//Call the UnPause function to unpause the game
			UnPause ();
			return;
		}

		if (Input.GetKeyDown(KeyCode.I) && !inInventory && !isPaused && !startScript.inMainMenu){
			ShowInventory();
			return;
		}

		if ((Input.GetButtonDown ("Cancel") || Input.GetKeyDown(KeyCode.I))&& inInventory && !isPaused && !startScript.inMainMenu) {
			HideInventory();
			return;
		}
	
	}


	public void DoPause()
	{
		//Set isPaused to true
		isPaused = true;
		//Set time.timescale to 0, this will cause animations and physics to stop updating
		Time.timeScale = 0;
		setCollider(false);
		//call the ShowPausePanel function of the ShowPanels script
		showPanels.ShowPausePanel ();
	}


	public void UnPause()
	{
		//Set isPaused to false
		isPaused = false;
		//Set time.timescale to 1, this will cause animations and physics to continue updating at regular speed
		Time.timeScale = 1;
		setCollider(true);
		//call the HidePausePanel function of the ShowPanels script
		showPanels.HidePausePanel ();
	}

	public void ShowInventory(){
		inInventory = true;
 		setCollider(false);
		showPanels.ShowInventory ();
	}

	public void HideInventory(){
		inInventory = false;
		setCollider(true);
		showPanels.HideInventory ();
	}

	private void setCollider(bool state){
		// get root objects in scene
		List<GameObject> rootObjects = new List<GameObject>();
		Scene scene = SceneManager.GetActiveScene();
		scene.GetRootGameObjects( rootObjects );
		 
		// iterate root objects and do something
		foreach (GameObject g in rootObjects)
		{
			if (g.name != "UI" && g.name != "GM"){
				if (g.GetComponent<Collider2D>()){
					g.GetComponent<Collider2D>().enabled = state;
				}
			}
		}
	}
}