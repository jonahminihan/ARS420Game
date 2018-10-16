using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class lobbyWait : NetworkBehaviour {

    private GameObject lobbyUI;
    private Canvas canv;
    public bool lobbyGood = false; // check if lobby is ready for player to start
    public bool canvActive;
    public int minPlayerCount = 4;
    // Use this for initialization
    void Start () {
        lobbyUI = gameObject.transform.GetChild(0).gameObject; //grabs the camera of this game object
        canv = lobbyUI.GetComponent<Canvas>();
        canvActive = false;
    }
	
	// Update is called once per frame
	void Update () {
        //if (!isServer)

       //Debug.Log(NetworkServer.connections.Count);
        /* if (NetworkServer.connections.Count < 4)
         {
             canv.enabled = true;
         }*/
        if (canvActive == true)
        {

            Debug.Log("canActive true");
            if (NetworkServer.connections.Count < minPlayerCount)
            {
                canv.enabled = true;
            }
            else {
                canv.enabled = false;
                lobbyGood = true;
            }

        }
        else{
            canv.enabled = false;
            lobbyGood = true;
        }
	}
}
