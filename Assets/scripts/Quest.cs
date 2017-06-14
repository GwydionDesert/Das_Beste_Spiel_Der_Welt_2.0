using System;
using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(PolygonCollider2D))]
public class Quest : MonoBehaviour {    

    public Chapter[] chapter;
    private bool stateChanged = false;

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
    }

    public void questLine(){
        // progress in Quest Line
        if (!stateChanged){
            if (chapter[state].needsItem != null){
                // find quest item in inventory
                foreach(Transform t in invCont.transform){
                    if (t.childCount > 0){
                        if (t.GetChild(0).gameObject.name.Equals(chapter[state].needsItem)){
                            
                            // remove quest item from inventory
                            Destroy(t.GetChild(0).gameObject);

                            // add item
                            if (chapter[state].givesItem != ""){
                                invCont.addItem(new Item(chapter[state].givesItem, (Sprite) Resources.Load("Icons//" + chapter[state].givesItem + ".psd")));
                            }

                            state ++;
                            stateChanged = true;
                        }
                    }
                //if (GameObject.Find("GM").GetComponent<GM>().objectInInventory.ContainsKey(chapter[state].needsItem)){
                    
                }
            }
            else{
                if (chapter[state].givesItem != ""){
                    invCont.addItem(new Item(chapter[state].givesItem, (Sprite) Resources.Load("Icons//" + chapter[state].givesItem + ".psd")));
                }
                state ++;
                stateChanged = true;
            }
        }

        //#################################################
        // specific Quests
        if (gameObject.name.Equals("Hippogreif") && state >= chapter.Length - 1){
            GameObject.Find("Auto").GetComponent<PolygonCollider2D>().enabled = true;
        }

        display(chapter[state].text);
    }

    // display questtext
    private void display(String[] questText){
        // instantiate Text once
        invCont.setItemColliders(false);
        if (iText == 0){
            UI.GetComponent<StartOptions>().inMainMenu = true;  // -> Options Menu and Inventory can't be opened
            textField.SetActive(true);
        }

        // cycle through texts
        if (iText < questText.Length && iText >= 0){
			textMP.text = questText[iText];
            if (chapter[state].sound.Length != 0){
                if (iText % 2 == 0) {
                    secondaryAS.PlayOneShot (chapter[state].sound [iText], GM.gm.effect_volume);
                    StartCoroutine (FadeOut (primaryAS, 0.3f));
                }
                else {
                    primaryAS.PlayOneShot (chapter[state].sound [iText], GM.gm.effect_volume);
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

        stateChanged = false;

        if (state > 0){
            if (chapter[state].vanishes){
                Destroy(gameObject);
            }
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

[System.SerializableAttribute]
public class Chapter{
    [MultilineAttribute]
    public String[] text;
    public AudioClip[] sound;
    public String needsItem;
    public String givesItem;
    public bool vanishes;
}