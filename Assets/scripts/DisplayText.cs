using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayText : MonoBehaviour {

    [TextAreaAttribute(5, 20)]
    public string descriptionText;

    private GameObject text;
    private GameObject textInstance;

    // get Text prefab from GM
    private void Start()
    {
        text = GameObject.Find("GM").gameObject.GetComponent<GM>().text;
    }

    // display description above object
    public void display()
    {
        textInstance = Instantiate(text, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.rotation, transform);
        textInstance.GetComponent<TextMesh>().text = descriptionText;
    }

    // hide description
    public void stop()
    {
        Destroy(textInstance);
    }
}
