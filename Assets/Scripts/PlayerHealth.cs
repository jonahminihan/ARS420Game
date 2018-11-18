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


    //getting hit
    /*public AudioClip hitSound;
    private float hitSoundVol;
    private AudioSource source;
*/





    public void Start()
    {
        Debug.Log(spawnPoints[0].GetComponent<Transform>().position.x);
        Debug.Log(spawnPoints[0].GetComponent<Transform>().position.y);
        Debug.Log(spawnPoints[0].GetComponent<Transform>().position.z);
        rb = GetComponent<Rigidbody>();
        //source = GetComponent<AudioSource>();
        //source = GetComponent<PlayerController>().source;
        //hitSoundVol = 0.4f;
    }


    public void TakeDamage(int amount)
    {
        Debug.Log("take damage1");
        if (!isServer)
        {
            Debug.Log("take damage is server");
            return;
        }
        if (currentHealth <= 0)
        {
            //Destroy(gameObject);
            // Getting the player team from the PlayerController script and passes team into scoreboard script
            // As the current health = the player is destroyed and the Scoreboardscript adds a counter to the opposing team
            //int team = gameObject.GetComponent<PlayerController>().team;                      // gameObject is for the game object your script is on
            //GameObject.Find("Scoreboard").GetComponent<Scoreboardscript>().updatescore(team);       // GameObject any game object
           
        }
        else
        {

            currentHealth -= amount;
            //source = GetComponent<AudioSource>();
            //source.PlayOneShot(hitSound, hitSoundVol);
            Debug.Log("take damage2");
            if (currentHealth <= 0)
            {
                Debug.Log("take damage3");
                currentHealth = maxHealth;
                Debug.Log("dead");
                RpcRespawn();
                int team = gameObject.GetComponent<PlayerController>().team;                      // gameObject is for the game object your script is on
                GameObject.Find("Scoreboard").GetComponent<Scoreboardscript>().updatescore(team);
            }
        }
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            int randomInt = rand.Next(0, spawnPoints.Length);
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
            quat.y = spawnPoints[randomInt].GetComponent<Transform>().rotation.y;
            quat.z = spawnPoints[randomInt].GetComponent<Transform>().rotation.z;
            transform.rotation = quat;
            /**/
            //rb.MovePosition(Vector3.zero);
        }
    }

}
