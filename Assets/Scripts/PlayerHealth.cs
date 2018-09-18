using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

[RequireComponent(typeof(Rigidbody))]

public class PlayerHealth : NetworkBehaviour {

    public const int maxHealth = 100;
    //[SyncVar]
    System.Random rand = new System.Random();
    [SyncVar]
    public int currentHealth = maxHealth;
    public bool destroyOnDeath;
    public GameObject[] spawnPoints;

    private Rigidbody rb;
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    public void TakeDamage(int amount)
    {
        if (!isServer)
        {
            return;
        }
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        else
        {

            currentHealth -= amount;

            if (currentHealth <= 0)
            {
                currentHealth = maxHealth;
                Debug.Log("dead");
                RpcRespawn();
            }
        }
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            int randomInt = rand.Next(0, spawnPoints.Length -1);
            // move back to zero location
            transform.position = Vector3.zero;
            // spawnPoints.
            Vector3 vect = new Vector3();
            Quaternion quat = new Quaternion();
            vect = spawnPoints[randomInt].GetComponent<Transform>().position;
            vect.x = spawnPoints[randomInt].GetComponent<Transform>().position.x;
            vect.y = spawnPoints[randomInt].GetComponent<Transform>().position.y;
            vect.z = spawnPoints[randomInt].GetComponent<Transform>().position.z;
            transform.position = vect;

            quat.x = spawnPoints[randomInt].GetComponent<Transform>().rotation.x;
            //quat.y = spawnPoints[randomInt].GetComponent<Transform>().rotation.y;
            //quat.z = spawnPoints[randomInt].GetComponent<Transform>().rotation.z;
            //transform.rotation = quat;
            /**/
            //rb.MovePosition(Vector3.zero);
        }
    }

}
