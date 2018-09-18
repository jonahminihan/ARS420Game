using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamScript : MonoBehaviour {


    public int team = 0;
	// Use this for initialization
	void Start () {
		
	}
	

    public int getTeam (){
        if (team == 1){
            team = 0;
        }
        else {
            team = 1;
        }
        return team;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
