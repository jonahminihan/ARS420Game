using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class lobbyWait : NetworkBehaviour {

    private GameObject lobbyUI;
    private Canvas canv;
    [SyncVar]
    public bool lobbyGood = false; // check if lobby is ready for player to start
    [SyncVar]
    public bool canvActive;
    private int minPlayerCount = 2;
    // Use this for initialization
    void Start () {
        lobbyUI = gameObject.transform.GetChild(0).gameObject; //grabs the camera of this game object
        canv = lobbyUI.GetComponent<Canvas>();
        canvActive = true;
    }
	
	// Update is called once per frame
	void Update () {
        //if (!isServer)

       //Debug.Log(NetworkServer.connections.Count);
        /* if (NetworkServer.connections.Count < 4)
         {
             canv.enabled = true;
         }*/
        if (canvActive == true && isServer)
        {

            Debug.Log("canActive true");
            Debug.Log("place1");
            if (NetworkServer.connections.Count < minPlayerCount)
            {
                canv.enabled = true;
                Debug.Log("place2 " + NetworkServer.connections.Count);
                Debug.Log("minPC " + minPlayerCount);
            }
            else {
                canv.enabled = false;
                lobbyGood = true;
                Debug.Log("place3 " + NetworkServer.connections.Count);
                RpcturnCanvasOff();
                }

        }
        else{
            //Debug.Log("canActive false");
            canv.enabled = false;
            lobbyGood = true;
            //RpcturnCanvasOff();
            }
	}

    [ClientRpc] //should fix probs?
    public void RpcturnCanvasOff(){
        canv.enabled = false;
        lobbyGood = true;
    }
    [Command]
    public void CmdturnCanvasOff()
    {
        canv.enabled = false;
        lobbyGood = true;
    }
}
