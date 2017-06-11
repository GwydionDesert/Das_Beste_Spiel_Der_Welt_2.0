using System;
using System.Collections;
using UnityEngine;
using TMPro; // Text Mesh Pro Asset

[RequireComponent(typeof(PolygonCollider2D))]
public class DisplayText : MonoBehaviour {    
    [HideInInspector]
    public string[] descriptionText;

    // display text
    private GameObject text;
	private TextMeshPro textMP;
    private GameObject textInstance;

    [HideInInspector]
    public int iText;		// ammount of text instances
    private GameObject UI;

    private float textScale = 0.9f;

    public Vector3 offset = new Vector3(0,1,0);

	public AudioClip[] sound;
	private AudioSource primaryAS;
	private AudioSource secondaryAS;
    private InventoryController invCont;

    // get Text prefab from GM
    private void Start(){
        UI = GameObject.Find("UI").gameObject;
        text = GameObject.Find("GM").gameObject.GetComponent<GM>().text;

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
            textInstance = Instantiate(text, new Vector3 (transform.position.x, transform.position.y, -2.0f), Quaternion.Euler(0, 0, transform.rotation.z), transform); // parent rotation is correctet (0, 0, 0)
            textInstance.transform.localScale = new Vector3 ((textInstance.transform.localScale.x / transform.localScale.x) * textScale,
                                                             (textInstance.transform.localScale.y / transform.localScale.y) * textScale, 0f);
            textInstance.transform.position += offset;

			textMP = textInstance.GetComponent<TextMeshPro> ();
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
        iText = -1;
        Destroy(textInstance);
        //Destroy(textBG);
        UI.GetComponent<StartOptions>().inMainMenu = false;
        invCont.setItemColliders(true);

        // add to Inventory
        if (this.GetComponent<Item>() != null){
            invCont.addItem(this.GetComponent<Item>());
            Destroy(this.gameObject);
        }
    }

	// show where Text will be displayed
    public void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3 (transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z), new Vector3(0.2f, 1, 1));
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