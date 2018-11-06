using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class healthSpawnScript : NetworkBehaviour{

    //Rocket Launcher info
    public GameObject healthPrefab;
    public float healthMaxRespawnTime = 10f;
    public float healthRespawnTime = 0f;
    public Transform healthRespawnPoint;

    //Crossbow info
    //public GameObject CBPrefab;
    //public float CBMaxRespawnTime = 10f;
    public float health2RespawnTime = 0f;
    public Transform health2RespawnPoint;
    // Use this for initialization
    void Start()
    {
        //RLPrefab = gameObject.transform.GetChild(0).gameObject;
        CmdSpawnHealth(healthRespawnPoint.position, healthRespawnPoint.rotation);
        RpcSpawnHealth(healthRespawnPoint.position, healthRespawnPoint.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        healthRespawnTime += Time.deltaTime;
        if (healthRespawnTime >= healthMaxRespawnTime)
        {
            CmdSpawnHealth(healthRespawnPoint.position, healthRespawnPoint.rotation);
            RpcSpawnHealth(healthRespawnPoint.position, healthRespawnPoint.rotation);
            healthRespawnTime = 0;
        }

        health2RespawnTime += Time.deltaTime;
        if (health2RespawnTime >= healthMaxRespawnTime)
        {
            CmdSpawnHealth(health2RespawnPoint.position, health2RespawnPoint.rotation);
            RpcSpawnHealth(health2RespawnPoint.position, health2RespawnPoint.rotation);
            health2RespawnTime = 0;
        }
        //CmdSpawnRL();
    }


    [Command]
    public void CmdSpawnHealth(Vector3 healthSpawnPos, Quaternion healthSpawnRot)
    {
        //This Function is done on the Server

        var RL = (GameObject)Instantiate(healthPrefab, healthSpawnPos, healthSpawnRot);

        NetworkServer.Spawn(RL);


    }
    [ClientRpc]
    public void RpcSpawnHealth(Vector3 healthSpawnPos, Quaternion healthSpawnRot)
    {
        //This Function is done on the Server

        var RL = (GameObject)Instantiate(healthPrefab, healthSpawnPos, healthSpawnRot);

        NetworkServer.Spawn(RL);


    }
    /*[Command]
    public void CmdSpawnCB()
    {
        //This Function is done on the Server

        var CB = (GameObject)Instantiate(healthPrefab, health2RespawnPoint.position, health2RespawnPoint.rotation);

        NetworkServer.Spawn(CB);


    }*/
}
