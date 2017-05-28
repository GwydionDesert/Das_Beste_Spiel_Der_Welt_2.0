using System;
using System.Collections;
using UnityEngine;
using TMPro; // Text Mesh Pro Asset

public class DisplayText : MonoBehaviour {
   
    private string[] descriptionText;

    // display text
    private GameObject text;
	private TextMeshPro textMP;
    private GameObject textInstance;
	private GameObject textBG;

    [HideInInspector]
    public int iText;		// ammount of text instances
    private GameObject UI;

    private float textScale = 0.9f;

    public Vector3 offset = new Vector3(0,1,0);

	public AudioClip[] sound;
	private AudioSource primaryAS;
	private AudioSource secondaryAS;

    //private InventoryController ic = new InventoryController();
    private InventoryController invCont;

    // get Text prefab from GM
    private void Start()
    {
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
    public void display()
    {
        // instantiate Text once
        invCont.setItemColliders(false);
        if (iText == 0)
        {
            UI.GetComponent<StartOptions>().inMainMenu = true;
            textInstance = Instantiate(text, new Vector3 (transform.position.x, transform.position.y, -2.0f), transform.rotation, transform);
            textInstance.transform.localScale = new Vector3 ((textInstance.transform.localScale.x / transform.localScale.x) * textScale,
                                                             (textInstance.transform.localScale.y / transform.localScale.y) * textScale, 0f);
            textInstance.transform.position += offset;

			//textBG = Instantiate (GM.gm.textBackground, new Vector3(textInstance.transform.position.x, textInstance.transform.position.y, -1.0f), GM.gm.textBackground.transform.rotation, transform);

			textMP = textInstance.GetComponent<TextMeshPro> ();

			try {
                //primaryAS.PlayOneShot (sound [0], GM.gm.effect_volume);
            }
            catch (Exception e){
                Debug.Log(e.StackTrace);
            }
        }

        // cycle through texts
        if (iText < descriptionText.Length)
        {
			textMP.text = descriptionText [iText];
			// bg
			textMP.ForceMeshUpdate ();
			Bounds textBounds = textMP.textBounds;

			float marginX = 0.0f;
			float marginY = 1.0f;
			//Vector3 rectTransformSizeDelta = new Vector3 ((textBounds.size.x + marginX) / 4.2f, 0.1f, (textBounds.size.y + marginY) / 3f);
			//textBG.transform.localScale = rectTransformSizeDelta;
			//textBG.GetComponent<RectTransform> ().sizeDelta = rectTransformSizeDelta;

            if (sound.Length != 0){
                if (iText % 2 == 0) {
                    secondaryAS.PlayOneShot (sound [iText], GM.gm.effect_volume);
                    StartCoroutine (FadeOut (primaryAS, 0.3f));
                } else {
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
    public void stop()
    {
        iText = 0;
        Destroy(textInstance);
        //Destroy(textBG);
        UI.GetComponent<StartOptions>().inMainMenu = false; // -> Options Menu and Inventory can't be opened
        invCont.setItemColliders(true);
        StartCoroutine("leaveInteractionMode");

        // add to Inventory
        if (this.GetComponent<Item>() != null){
            Cursor.visible = true;  // leaveInteractionMode can't finish when object is destroyed
            invCont.addItem(this.GetComponent<Item>());
        }
    }

	// show where Text will be displayed
    public void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3 (transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z), new Vector3(0.2f, 1, 1));
    }

    // fix for: description loops when mouse on object
    IEnumerator leaveInteractionMode() {
        yield return new WaitForSeconds(0.1f);
        Cursor.visible = true;
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