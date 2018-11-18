﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animController : MonoBehaviour {

    public Animator anim;

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetKey(KeyCode.W))
        {
            anim.Play("Pistol Walk");
        }

        else if(Input.GetKey(KeyCode.A))
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
}
