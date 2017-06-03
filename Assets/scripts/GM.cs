using System;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour {


    public static GM gm;

    private void Awake()
    {
        keepObject();
        readTable(objects, description);
        readTable(combinations, combo);

        Cursor.lockState = CursorLockMode.Confined;
    }

    // check if more than one GM is active per scene
    private void keepObject()
    {
        if (gm == null)
        {
            DontDestroyOnLoad(gameObject);
            gm = this;
        }
        else
        {
            if (gm != this)
            {
                Destroy(gameObject);
            }
        }
    }

    private void readTable(TextAsset t, Dictionary<string, string[]> d)
    {
        // Split text by rows
        String[] s = t.text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < s.Length; i++)
        {
            // Dict Key = first word in row
            String key = s[i].Split('\t')[0];
            // Dict Value = rest of row, split by Tabs
            String[] text = s[i].Substring(key.Length + 1).Split('\t');

			// make escape characters useable (| -> \n)
			for (int j = 0; j < text.Length; j++) {
				text [j] = text [j].Replace ("|", "\n");
			}
			// add key to dictionary
            d.Add(key, text);
        }
    }

//********************************************************************************************************
	// public variables

    public GameObject text;
	public GameObject textBackground;
    public Texture2D cursorActive;
    public Texture2D cursorInactive;

	// options
    public float music_volume;
    public float effect_volume;

    // object description
    [HideInInspector]
    public Dictionary<string, string[]> description = new Dictionary<string, string[]>();
    public Dictionary<string, string[]> combo = new Dictionary<string, string[]>();
    public TextAsset objects;
    public TextAsset combinations;

    public Dictionary<String, bool> objectInInventory = new Dictionary<string, bool>();
}
