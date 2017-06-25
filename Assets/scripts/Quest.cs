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
	private GameObject knuffel;

	[HideInInspector]
	public int iText;		// ammount of text instances
	private GameObject UI;

	private AudioSource primaryAS;
	private AudioSource secondaryAS;
	private InventoryController invCont;

	public int state;
	private bool willDie = false;
	private bool displayingText = false;

	private string[] displayText;
	private Color[] color;
	private AudioClip[] playingSounds;
	private int[] tempState;

	// get Text prefab from GM
	private void Start(){
		UI = GameObject.Find("UI").gameObject;
		textField = GameObject.Find("GM").gameObject.GetComponent<GM>().textField;
		textMP = textField.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
		knuffel = GameObject.Find("Knuffel");

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
			GM.gm.questState.Add(gameObject.name, 0);
		}

		if (state > chapter.Length){
			state --;
		}

		if (state > 0){
				if (chapter[state - 1].vanishes){
					specificQuests();
					Destroy(gameObject);
				}
				else{
					specificQuests();
				}
		}
	}

	public void questLine(){
		
		specificQuests();
		if (!displayingText){
			// progress in Quest Line
			if (state < chapter.Length){
				int previousState = state;
				while (playerHasObject()){
					state ++;
					GM.gm.questState[gameObject.name] = state;
					// add item
					if (chapter.Length > 1){
						if (chapter[state].givesItem != null){
							invCont.addItem(new Item(chapter[state].givesItem.name, chapter[state].givesItem));
						}

						if (chapter[state].vanishes){
							willDie = true;
						}
					}
					else {
						if (chapter[0].givesItem != null){
							invCont.addItem(new Item(chapter[0].givesItem.name, chapter[0].givesItem));
						}

						if (chapter[0].vanishes){
							willDie = true;
						}
					}
				}

				// show Text
				if (chapter.Length > 1){
					// show text and play sound
					string[] disp = new string[state - previousState + 1];
					AudioClip[] sound = new AudioClip[state - previousState + 1];
					Color[] color = new Color[state - previousState + 1];
					int[] tempState = new int[state - previousState + 1];
						
					for (int i = disp.Length - 1; i >= 0; i--){
						disp[disp.Length - i - 1] = chapter[state - i].text[0];
						if (chapter[state - i].sound.Length > 0){
							sound[disp.Length - i - 1] = chapter[state - i].sound[0];
						}
						color[disp.Length - i - 1] = chapter[state - i].sprechfarbe;
						tempState[disp.Length - i - 1] = state - i;
					}

					state = previousState;
					display(disp, sound, tempState, color);
				}
				else {
					if (chapter[0].sound.Length > 0){
						display(chapter[0].text, chapter[0].sound, new int[] {1}, new Color[] {chapter[0].sprechfarbe});
					}
					else{
						display(chapter[0].text, new AudioClip[1], new int[] {1}, new Color[] {chapter[0].sprechfarbe});
					}
					
				}
			}
		}
		else {
			display(displayText, playingSounds, this.tempState, this.color);
		}
	}

	private void specificQuests(){
		if (gameObject.name.Equals("Hippogreif") && state >= chapter.Length){
			GameObject.Find("Auto").GetComponent<PolygonCollider2D>().enabled = true;
		}
		if (gameObject.name.Equals("Einhorn") && state >= chapter.Length){
			GameObject.Find("changeSceneKarte").GetComponent<PolygonCollider2D>().enabled = true;
		}

		if (gameObject.name.Equals("Origamivogel") && state >= chapter.Length){
			GM.gm.karteWege = true;
		}

		if (gameObject.name.Equals("Phoenix") && state >= chapter.Length - 1){
			GetComponent<PolygonCollider2D>().enabled = false;
		}

		if (gameObject.name.Equals("Kiste") && state >= 1){
			gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/Kiste_offen");
			gameObject.GetComponent<PolygonCollider2D>().enabled = false;
		}

		//	Tutorial
		if (gameObject.name.Equals("Tutorial") && state == 0){
			try{
				GameObject.Find("Canvas").SetActive(false);
			}
			catch{}
		}

		if (gameObject.name.Equals("Tutorial") && state >= chapter.Length - 1){
			knuffel.GetComponent<SpriteRenderer>().enabled = false;
			ChangeScene.changeScene("forrest");
			GetComponent<Quest>().enabled = false;
		}

		//	Karten Quest
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
			GameObject.Find("changeSceneForrest").SetActive(false);
		}

		//	Auto Quest
		if (gameObject.name.Equals("Auto")){
			switch ((int) state){
				case 3: case 4:
					gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/Auto_auf");
					break;
				case 5: case 6: case 7: case 8:
					gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/Auto_auf_batterie+kont");
					break;
				case 9: case 10:
					gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/Auto_zu_reifen");
					break;
				case 11: case 12: case 13: case 14:
					gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/Auto_tür_auf");
					break;
				case 15:
					Cursor.visible = true;
					Camera.main.GetComponent<onClick>().lastHit = gameObject;
					Camera.main.GetComponent<onClick>().state = onClick.State.idle;
					foreach(Transform t in GameObject.Find("UI").transform){
						if (t.gameObject.name.Equals("SiegAuto")){
							t.gameObject.SetActive(true);
						}
					}
					ChangeScene.changeScene("AutoSieg");
					break;
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
		else if(chapter.Length == 1 && !willDie){
			if (chapter[0].needsItem != ""){
				// look through inventory
				foreach(Transform t in invCont.transform){
					if (t.childCount > 0){
						if (t.GetChild(0).gameObject.name.Equals(chapter[0].needsItem)){
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

	private void display(string[] questText, AudioClip[] sound, int[] tempState, Color[] color){
		displayText = questText;
		playingSounds = sound;
		this.tempState = tempState;
		this.color = color;

		displayingText = true;

		// instantiate Text once
		invCont.setItemColliders(false);
		if (iText == 0){
			UI.GetComponent<StartOptions>().inMainMenu = true;	// -> Options Menu and Inventory can't be opened
			textField.SetActive(true);
		}

		// cycle through texts
		if (iText < questText.Length && iText >= 0){
			state = tempState[iText];

			textMP.text = questText[iText];
			textMP.color = color[iText];

			// display Knuffel
			if (textMP.color.CompareRGB(GM.gm.knuffelText)){
				knuffel.GetComponent<SpriteRenderer>().enabled = true;
			}
			else{
				knuffel.GetComponent<SpriteRenderer>().enabled = false;
			}


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
		knuffel.GetComponent<SpriteRenderer>().enabled = false;

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
	public string[] text;
	public Color sprechfarbe = new Color(141, 104, 36, 255);
	public AudioClip[] sound;
	public string needsItem;
	public Sprite givesItem;
	public bool vanishes;
}