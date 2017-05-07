using System;
using System.Collections.Generic;
using UnityEngine;

public class DisplayText : MonoBehaviour {
   
    private string[] descriptionText;

    // display text
    private GameObject text;
    private GameObject textInstance;

    [HideInInspector]
    public int iText;		// ammount of text instances

    // get Text prefab from GM
    private void Start()
    {
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
            textInstance = Instantiate(text, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.rotation, transform);
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
        Cursor.visible = true;
    }
}
