﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class healthPickUpScript : NetworkBehaviour{

    private GameObject thisObject;
    public string gunName;
    // Use this for initialization
    void Start()
    {
        thisObject = gameObject;
        gunName = gameObject.name;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        var controller = hit.GetComponent<PlayerController>();
        var healthScript = hit.GetComponent<PlayerHealth>();
        /*if (gameObject.name[0] == 'r')
        {
            if (gameObject.name[1] == 'o')
            {
                controller.gunCollection[0] = true;
            }
        }
        if (gameObject.name[0] == 'c')
        {
            if (gameObject.name[1] == 'r')
            {
                controller.gunCollection[1] = true;
            }
        }*/
        if (healthScript.currentHealth <= 1500)
        {

            healthScript.currentHealth += 10; //some reason this updates health twice

        }
        //controller.gunCollection[0] = true;
        //CmdDestroy();
        Destroy(gameObject);


    }
    // Update is called once per frame
    void Update()
    {

    }

    [Command]
    void CmdDestroy()
    {
        Destroy(thisObject);
    }

    [ClientRpc]
    void RpcDestroy()
    {
        Destroy(thisObject);
    }
}
