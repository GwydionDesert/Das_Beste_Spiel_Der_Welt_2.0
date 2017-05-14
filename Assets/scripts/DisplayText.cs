using System;
using System.Collections;
using UnityEngine;

public class DisplayText : MonoBehaviour {
   
    private string[] descriptionText;

    // display text
    private GameObject text;
    private GameObject textInstance;

    [HideInInspector]
    public int iText;		// ammount of text instances
    private GameObject UI;

    private float textScale = 0.9f;

    public Vector3 offset = new Vector3(0,1,0);

    // get Text prefab from GM
    private void Start()
    {
        UI = GameObject.Find("UI").gameObject;

        text = GameObject.Find("GM").gameObject.GetComponent<GM>().text;
        if (GameObject.Find("GM").gameObject.GetComponent<GM>().description.ContainsKey(gameObject.name))
        {
            descriptionText = GameObject.Find("GM").gameObject.GetComponent<GM>().description[gameObject.name];
        }
        else
        {
            Debug.Log("Objektname stimmt mit keinem Key überein!");
        }
    }

    // display description above object
    public void display()
    {
        // instantiate Text once
        if (iText == 0)
        {
            UI.GetComponent<StartOptions>().inMainMenu = true;
            textInstance = Instantiate(text, new Vector3 (transform.position.x, transform.position.y, transform.position.z), transform.rotation, transform);
            textInstance.transform.localScale = new Vector3 ((textInstance.transform.localScale.x / transform.localScale.x) * textScale,
                                                             (textInstance.transform.localScale.y / transform.localScale.y) * textScale, 0f);
            textInstance.transform.position += offset;
        }

        // cycle through texts
        if (iText < descriptionText.Length)
        {
            textInstance.GetComponent<TextMesh>().text = descriptionText[iText];
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
        UI.GetComponent<StartOptions>().inMainMenu = false;
        StartCoroutine("leaveInteractionMode");
    }

    public void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3 (transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z), new Vector3(0.2f, 1, 1));
    }

    // fix for: description loops when mouse on object
    IEnumerator leaveInteractionMode() {
        yield return new WaitForSeconds(0.1f);
        Cursor.visible = true;
    }
}
