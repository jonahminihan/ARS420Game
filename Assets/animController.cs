using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class animController : NetworkBehaviour
{

    public Animator anim;

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isLocalPlayer) //GetComponent<NetworkIdentity>()
        {
                // Need to set everything that isn't the camera stuff false so that it doesn't show for the player's camera.
                GameObject temp;
                GameObject gun12 = gameObject.transform.GetChild(0).gameObject; //grabs the graphics of this game object
                gun12 = gun12.transform.GetChild(0).gameObject; //get gun on GameObject
                temp = gun12.transform.GetChild(0).gameObject;
                temp.SetActive(false);
            
            /*
                temp = gun12.transform.GetChild(7).gameObject; //get RL on gameobject
                temp.SetActive(false);
                temp = gun12.transform.GetChild(5).gameObject; //get CB on gameobject
                temp.SetActive(false);
                temp = gun12.transform.GetChild(6).gameObject; //get ER on gameobject
                temp.SetActive(false);*/


            // animations that everyone else will see from the player
            if (Input.GetKey(KeyCode.W))
                {
                    anim.Play("Pistol Walk");
                }

                else if (Input.GetKey(KeyCode.A))
                {
                    anim.Play("Pistol Walk Left");
                }
                else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))
                {
                    anim.Play("Pistol Idle"); // need diagonal movement
                }

                else if (Input.GetKey(KeyCode.S))
                {
                    anim.Play("Pistol Walk Backward");
                }

                else if (Input.GetKey(KeyCode.D))
                {
                    anim.Play("Pistol Walk Right");
                }
                else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))
                {
                    anim.Play("Pistol Idle"); // need diagonal movement
                }

                else if (Input.GetKey(KeyCode.Space))
                {
                    anim.Play("Pistol Jump");
                }

                else
                {
                    anim.Play("Pistol Idle");
                }
        }
        if (!isLocalPlayer)
        {
            GameObject temp;
            GameObject gun12 = gameObject.transform.GetChild(1).gameObject; //grabs the graphics of this game object
            gun12 = gun12.transform.GetChild(0).gameObject; //get gun on GameObject
            temp = gun12.transform.GetChild(0).gameObject;
            temp.SetActive(false);
            temp = gun12.transform.GetChild(2).gameObject;
            temp.SetActive(false);
            temp = gun12.transform.GetChild(4).gameObject;
            temp.SetActive(false);
        }
    }
}
