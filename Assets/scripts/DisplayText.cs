using System;
using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(PolygonCollider2D))]
public class DisplayText : MonoBehaviour {    
	[HideInInspector]
	public string[] descriptionText;

	// display text
	private GameObject textField;
	private TextMeshProUGUI textMP;

	[HideInInspector]
	public int iText;		// ammount of text instances
	private GameObject UI;

	public AudioClip[] sound;
	private AudioSource primaryAS;
	private AudioSource secondaryAS;
	private InventoryController invCont;

	// get Text prefab from GM
	private void Start(){
		UI = GameObject.Find("UI").gameObject;
		textField = GameObject.Find("GM").gameObject.GetComponent<GM>().textField;

		primaryAS = gameObject.AddComponent<AudioSource>();
		primaryAS.clip = Resources.Load (name) as AudioClip;
		secondaryAS = gameObject.AddComponent<AudioSource>();
		secondaryAS.clip = Resources.Load(name) as AudioClip;

		if (GameObject.Find("GM").gameObject.GetComponent<GM>().description.ContainsKey(gameObject.name))
		{
			descriptionText = GameObject.Find("GM").gameObject.GetComponent<GM>().description[gameObject.name];
		}
		else
		{
			Debug.Log("Objektname stimmt mit keinem Key überein!");
			Debug.Log(gameObject.name);
		}

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
	}

	// display description above object
	public void display(){
		// instantiate Text once
		invCont.setItemColliders(false);
		if (iText == 0){
			UI.GetComponent<StartOptions>().inMainMenu = true;  // -> Options Menu and Inventory can't be opened

			textField.SetActive(true);
			textMP = textField.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
		}

		// cycle through texts
		if (iText < descriptionText.Length && iText >= 0){
			textMP.text = descriptionText [iText];

			if (sound.Length != 0){
				if (iText % 2 == 0) {
					secondaryAS.PlayOneShot (sound [iText], GM.gm.effect_volume);
					StartCoroutine (FadeOut (primaryAS, 0.3f));
				}
			else {
					primaryAS.PlayOneShot (sound [iText], GM.gm.effect_volume);
					StartCoroutine (FadeOut (secondaryAS, 0.3f));
				}
			}
		}
		else
		{
			stop();
		}
	}

	// hide description
	public void stop(){
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

		// add to Inventory
		if (this.GetComponent<Item>() != null){
			invCont.addItem(this.GetComponent<Item>());
			Destroy(this.gameObject);
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
}