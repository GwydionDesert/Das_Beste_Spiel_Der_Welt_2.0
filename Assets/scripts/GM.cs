using UnityEngine;

public class GM : MonoBehaviour {


    public static GM gm;

    private void Awake()
    {
        keepObject();
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

    public GameObject text;
    //public Sprite cursorActive;
    //public Sprite cursorInactive;
    public Texture2D cursorActive;
    public Texture2D cursorInactive;

    // space for object states like already interacted with ... or picked up
}
