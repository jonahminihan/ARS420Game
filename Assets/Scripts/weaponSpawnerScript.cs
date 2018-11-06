using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class weaponSpawnerScript : NetworkBehaviour {
    //Rocket Launcher info
    public GameObject RLPrefab;
    public float RLMaxRespawnTime = 10f;
    public float RLRespawnTime = 0f;
    public Transform RLRespawnPoint;

    //Crossbow info
    public GameObject CBPrefab;
    public float CBMaxRespawnTime = 10f;
    public float CBRespawnTime = 0f;
    public Transform CBRespawnPoint;
    // Use this for initialization
    void Start () {
        //RLPrefab = gameObject.transform.GetChild(0).gameObject;
        CmdSpawnRL();
    }
	
	// Update is called once per frame
	void Update () {
        RLRespawnTime += Time.deltaTime;
        if(RLRespawnTime >= RLMaxRespawnTime){
            CmdSpawnRL();
            RpcSpawnRL();
            RLRespawnTime = 0;
        }

        CBRespawnTime += Time.deltaTime;
        if (CBRespawnTime >= CBMaxRespawnTime)
        {
            CmdSpawnCB();
            CBRespawnTime = 0;
        }
        //CmdSpawnRL();
    }


    [Command]
    public void CmdSpawnRL(){
        //This Function is done on the Server

        var RL = (GameObject)Instantiate(RLPrefab, RLRespawnPoint.position, RLRespawnPoint.rotation);

        NetworkServer.Spawn(RL);


    }
    [ClientRpc]
    public void RpcSpawnRL()
    {
        //This Function is done on the Server

        var RL = (GameObject)Instantiate(RLPrefab, RLRespawnPoint.position, RLRespawnPoint.rotation);

        NetworkServer.Spawn(RL);


    }
    [Command]
    public void CmdSpawnCB()
    {
        //This Function is done on the Server

        var CB = (GameObject)Instantiate(CBPrefab, CBRespawnPoint.position, CBRespawnPoint.rotation);

        NetworkServer.Spawn(CB);


    }
}
