using System;
using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(PolygonCollider2D))]
public class Quest : MonoBehaviour {

	public Chapter[] chapter;

	// display text
	private GameObject textField;
	private TextMeshProUGUI textMP;

	[HideInInspector]
	public int iText;		// ammount of text instances
	private GameObject UI;

	private AudioSource primaryAS;
	private AudioSource secondaryAS;
	private InventoryController invCont;

	public int state = 0;
	private bool willDie = false;
	private bool displayingText = false;

	private String[] displayText;
	private Color[] color;
	private AudioClip[] playingSounds;
	private int[] tempState;

	// get Text prefab from GM
	private void Start(){
		UI = GameObject.Find("UI").gameObject;
		textField = GameObject.Find("GM").gameObject.GetComponent<GM>().textField;
		textMP = textField.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

		primaryAS = gameObject.AddComponent<AudioSource>();
		primaryAS.clip = Resources.Load (name) as AudioClip;
		secondaryAS = gameObject.AddComponent<AudioSource>();
		secondaryAS.clip = Resources.Load(name) as AudioClip;

		// find InventoryController
		int i = 0;
		Transform g = GameObject.Find("UI").transform;
		while (g.GetChild(i) != null){
			if (g.GetChild(i).GetComponent<InventoryController>() != null){
				invCont = g.GetChild(i).GetComponent<InventoryController>();
				break;
			}
			i++;
		}

		if (GM.gm.questState.ContainsKey(gameObject.name)){
			state = GM.gm.questState[gameObject.name];
		}
		else{
			GM.gm.questState.Add(gameObject.name, state);
		}

		if (state > chapter.Length){
			state --;
		}

		if (state > 0){
				if (chapter[state - 1].vanishes){
					specificQuests();
					Destroy(gameObject);
				}
		}
	}

	public void questLine(){
		specificQuests();
		
		if (!displayingText){
			// progress in Quest Line
			if (state < chapter.Length){
				int nextState = state;
				while (playerHasObject()){
					Debug.Log("questline");
					nextState ++;
					GM.gm.questState[gameObject.name] = state;

					// add item
					if (chapter[state].givesItem != null){
						invCont.addItem(new Item(chapter[state].givesItem.name, chapter[state].givesItem));
					}

					if (state == 1 && chapter[0].givesItem != null){
						invCont.addItem(new Item(chapter[0].givesItem.name, chapter[0].givesItem));
					}

					if (chapter[state].vanishes){
						willDie = true;
					}
				}

				// show text and play sound
				String[] disp = new String[nextState - state + 1];
				AudioClip[] sound = new AudioClip[nextState - state + 1];
				Color[] color = new Color[nextState - state + 1];
				int[] tempState = new int[nextState - state + 1];
					
				for (int i = disp.Length - 1; i >= 0; i--){
					Debug.Log("adding values");
					disp[disp.Length - i - 1] = chapter[nextState - state - i].text[0];
					if (chapter[nextState - state - i].sound.Length > 0){
						sound[disp.Length - i - 1] = chapter[nextState - state - i].sound[0];
					}
					color[disp.Length - i - 1] = chapter[nextState - state - i].sprechfarbe;
					tempState[disp.Length - i - 1] = nextState - state - i;
				}

				foreach(int s in tempState){
					Debug.Log(s);
				}

				display(disp, sound, tempState, color);
			}
		}
		else {
			display(displayText, playingSounds, this.tempState, this.color);
		}
	}

	private void specificQuests(){
		Debug.Log("specific quest");
		if (gameObject.name.Equals("Hippogreif") && state >= chapter.Length){
			GameObject.Find("Auto").GetComponent<PolygonCollider2D>().enabled = true;
		}
		if (gameObject.name.Equals("Einhorn") && state >= chapter.Length){
			GameObject.Find("changeSceneKarte").GetComponent<PolygonCollider2D>().enabled = true;
		}

		if (gameObject.name.Equals("Origamivogel") && state >= chapter.Length){
			GM.gm.karteWege = true;
		}

		if (gameObject.name.Equals("Kiste") && state >= 1){
			gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/Kiste_offen");
			gameObject.GetComponent<PolygonCollider2D>().enabled = false;
		}

		if (gameObject.name.Equals("Tutorial") && state == 0){
			GameObject.Find("Canvas").SetActive(false);
		}

		if (gameObject.name.Equals("Tutorial") && state >= chapter.Length){
			ChangeScene.changeScene("forrest");
			GetComponent<Quest>().enabled = false;
		}

		if (gameObject.name.Equals("karteMitte") && state >= chapter.Length){
			GM.gm.karteMitte = true;
		}

		if (gameObject.name.Equals("kartePilz") && state >= chapter.Length){
			GM.gm.kartePilze = true;
		}

		if (gameObject.name.Equals("karteHoehle") && state >= chapter.Length){
			GM.gm.karteHoehle = true;
		}

		if (gameObject.name.Equals("karteWald") && state >= chapter.Length){
			foreach(Transform t in GameObject.Find("UI").transform){
				if (t.gameObject.name.Equals("SiegKarte")){
					t.gameObject.SetActive(true);
				}
			}
		}
	}

	private bool playerHasObject(){
		if (state < chapter.Length - 1){
			if (chapter[state].needsItem != ""){
				// look through inventory
				foreach(Transform t in invCont.transform){
					if (t.childCount > 0){
						if (t.GetChild(0).gameObject.name.Equals(chapter[state].needsItem)){
							// remove quest item from inventory
							Destroy(t.GetChild(0).gameObject);
							return true;
						}
					}
				}
			}
			else{
				return true;
			}
			return false;
		}
		else{
			return false;
		}
	}

	private void display(String[] questText, AudioClip[] sound, int[] tempState, Color[] color){
		Debug.Log("display");
		displayText = questText;
		playingSounds = sound;
		this.tempState = tempState;

		displayingText = true;
		// instantiate Text once
		invCont.setItemColliders(false);
		if (iText == 0){
			UI.GetComponent<StartOptions>().inMainMenu = true;	// -> Options Menu and Inventory can't be opened
			textField.SetActive(true);
		}

		// cycle through texts
		if (iText < questText.Length && iText >= 0){
			Debug.Log("iText: " + iText);
			Debug.Log(state);
			state = tempState[iText];

			textMP.text = questText[iText];
			textMP.color = color[iText];


			try{
				if (iText % 2 == 0) {
					secondaryAS.PlayOneShot (sound [iText], GM.gm.effect_volume);
					StartCoroutine (FadeOut (primaryAS, 0.3f));
				}
				else {
					primaryAS.PlayOneShot (sound [iText], GM.gm.effect_volume);
					StartCoroutine (FadeOut (secondaryAS, 0.3f));
				}
			}
			catch{
				Debug.Log("Audio not found! " + state + " : " + gameObject.name);
			}
		}
		else
		{
			stop();
		}
	}

	// fade AudioSource
	public static IEnumerator FadeOut (AudioSource audioSource, float FadeTime) {
		float startVolume = audioSource.volume;

		while (audioSource.volume > 0) {
			audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

			yield return null;
		}
		audioSource.Stop ();
		audioSource.volume = startVolume;
	}

	public void stop(){
		displayingText = false;

		if (iText % 2 == 0){
			StartCoroutine (FadeOut (primaryAS, 0.0f));
		}
		else {
			StartCoroutine (FadeOut (secondaryAS, 0.0f));
		}

		iText = -1;
		textField.SetActive(false);
		UI.GetComponent<StartOptions>().inMainMenu = false;
		invCont.setItemColliders(true);

		if (willDie){
			state ++;
			GM.gm.questState[gameObject.name] = state;
			specificQuests();

			Camera.main.GetComponent<onClick>().state = onClick.State.stop;
			Destroy(gameObject);
		}
	}
}

	[System.SerializableAttribute]
	public class Chapter{
	[MultilineAttribute]
	public String[] text;
	public Color sprechfarbe = new Color();
	public AudioClip[] sound;
	public String needsItem;
	public Sprite givesItem;
	public bool vanishes;

	Chapter(){
		sprechfarbe.a = 255;
		sprechfarbe.r = 141;
		sprechfarbe.g = 104;
		sprechfarbe.b = 36;
		
	}
}