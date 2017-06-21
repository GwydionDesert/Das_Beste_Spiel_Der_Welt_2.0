using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Karte : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (gameObject.name.Equals("karteHoehle") && GM.gm.karteHoehle){
			GetComponent<SpriteRenderer>().enabled = true;
		}
		if (gameObject.name.Equals("kartePilze") && GM.gm.kartePilze){
			GetComponent<SpriteRenderer>().enabled = true;
		}
		if (gameObject.name.Equals("karteWege") && GM.gm.karteWege){
			GetComponent<SpriteRenderer>().enabled = true;
		}
		if (gameObject.name.Equals("karteMitte") && GM.gm.karteMitte){
			GetComponent<SpriteRenderer>().enabled = true;
		}
	}

}
