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
    public GameObject spawnedHealth1;
    public GameObject spawnedHealth2;
    //Crossbow info
    //public GameObject CBPrefab;
    //public float CBMaxRespawnTime = 10f;
    public float health2RespawnTime = 0f;
    public Transform health2RespawnPoint;
    // Use this for initialization
    void Start()
    {
        //RLPrefab = gameObject.transform.GetChild(0).gameObject;
       //CmdSpawnHealth(healthRespawnPoint.position, healthRespawnPoint.rotation, spawnedHealth1);
        CmdSpawnHealth1(healthRespawnPoint.position, healthRespawnPoint.rotation);
        RpcSpawnHealth1(healthRespawnPoint.position, healthRespawnPoint.rotation);
        //RpcSpawnHealth(healthRespawnPoint.position, healthRespawnPoint.rotation, ref spawnedHealth1);
    }

    // Update is called once per frame
    void Update()
    {
        healthRespawnTime += Time.deltaTime;
        if (healthRespawnTime >= healthMaxRespawnTime)
        {
            //CmdSpawnHealth(healthRespawnPoint.position, healthRespawnPoint.rotation, spawnedHealth1);
            CmdSpawnHealth1(healthRespawnPoint.position, healthRespawnPoint.rotation);
            //RpcSpawnHealth(healthRespawnPoint.position, healthRespawnPoint.rotation, ref spawnedHealth1);
            RpcSpawnHealth1(healthRespawnPoint.position, healthRespawnPoint.rotation);
            healthRespawnTime = 0;
        }

        health2RespawnTime += Time.deltaTime;
        if (health2RespawnTime >= healthMaxRespawnTime)
        {
  //          CmdSpawnHealth(health2RespawnPoint.position, health2RespawnPoint.rotation, spawnedHealth2);
//            RpcSpawnHealth(health2RespawnPoint.position, health2RespawnPoint.rotation, spawnedHealth2);
            CmdSpawnHealth2(health2RespawnPoint.position, health2RespawnPoint.rotation);
            RpcSpawnHealth2(health2RespawnPoint.position, health2RespawnPoint.rotation);
            health2RespawnTime = 0;
        }
        //CmdSpawnRL();
    }


    [Command]
    public void CmdSpawnHealth(Vector3 healthSpawnPos, Quaternion healthSpawnRot, GameObject spawnedHealth)
    {
        //This Function is done on the Server

        if (spawnedHealth == null)
        {
            var RL = (GameObject)Instantiate(healthPrefab, healthSpawnPos, healthSpawnRot);
            spawnedHealth = RL;
            NetworkServer.Spawn(RL);

        }


    }
    [Command]
    public void CmdSpawnHealth1(Vector3 healthSpawnPos, Quaternion healthSpawnRot)
    {
        //This Function is done on the Server

        if (spawnedHealth1 == null)
        {
            var RL = (GameObject)Instantiate(healthPrefab, healthSpawnPos, healthSpawnRot);
            spawnedHealth1 = RL;
            NetworkServer.Spawn(RL);

        }


    }
    [Command]
    public void CmdSpawnHealth2(Vector3 healthSpawnPos, Quaternion healthSpawnRot)
    {
        //This Function is done on the Server

        if (spawnedHealth2 == null)
        {
            var RL = (GameObject)Instantiate(healthPrefab, healthSpawnPos, healthSpawnRot);
            spawnedHealth2 = RL;
            NetworkServer.Spawn(RL);

        }


    }
    [ClientRpc]
    public void RpcSpawnHealth(Vector3 healthSpawnPos, Quaternion healthSpawnRot, GameObject spawnedHealth)
    {
        //This Function is done on the Server
        if (spawnedHealth == null){
            var RL = (GameObject)Instantiate(healthPrefab, healthSpawnPos, healthSpawnRot);
            spawnedHealth = RL;
            NetworkServer.Spawn(RL);

        }


    }
    [ClientRpc]
    public void RpcSpawnHealth1(Vector3 healthSpawnPos, Quaternion healthSpawnRot)
    {
        //This Function is done on the Server
        if (spawnedHealth1 == null)
        {
            var RL = (GameObject)Instantiate(healthPrefab, healthSpawnPos, healthSpawnRot);
            spawnedHealth1 = RL;
            NetworkServer.Spawn(RL);

        }


    }
    [ClientRpc]
    public void RpcSpawnHealth2(Vector3 healthSpawnPos, Quaternion healthSpawnRot)
    {
        //This Function is done on the Server
        if (spawnedHealth2 == null)
        {
            var RL = (GameObject)Instantiate(healthPrefab, healthSpawnPos, healthSpawnRot);
            spawnedHealth2 = RL;
            NetworkServer.Spawn(RL);

        }


    }
    /*[Command]
    public void CmdSpawnCB()
    {
        //This Function is done on the Server

        var CB = (GameObject)Instantiate(healthPrefab, health2RespawnPoint.position, health2RespawnPoint.rotation);

        NetworkServer.Spawn(CB);


    }*/
}
