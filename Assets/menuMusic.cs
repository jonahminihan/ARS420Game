using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuMusic : MonoBehaviour {


    public AudioClip menuSound;
    private float menuMusicVol;
    private AudioSource source;
    
    // Use this for initialization


    void Start () {
        menuMusicVol = 0.1f;
        source = GetComponent<AudioSource>();
        source.PlayOneShot(menuSound, menuMusicVol);
        //Debug.Log("music");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
