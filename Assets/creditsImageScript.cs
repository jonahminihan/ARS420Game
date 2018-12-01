using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class creditsImageScript : MonoBehaviour {
    bool creditsOn = false;
	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Image>().enabled=false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void setCreditsActive(){
        gameObject.GetComponent<Image>().enabled = true;
    }

    void setCreditsOff()
    {
        gameObject.GetComponent<Image>().enabled = false;
    }

    void toggleCredits(){
        if (creditsOn == false){
            gameObject.GetComponent<Image>().enabled = true;
        }
        else{
            gameObject.GetComponent<Image>().enabled = false;
        }
    }
}
