using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Scoreboardscript : NetworkBehaviour {
    [SyncVar]
    public int teams0score=0, teams1score=0;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    public void updatescore(int teams)
    {
        if (teams==0)
        {
            teams1score++;
        }
        else if(teams==1)
        {
            teams0score++;
        }
    }
}
