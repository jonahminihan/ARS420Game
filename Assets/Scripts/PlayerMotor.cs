﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private GameObject BulletSpawnMotor;
    public Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;


    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //Gets a movement Vector
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;

        
    }

    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;


    }

    public void RotateCamera(Vector3 _cameraRotation)
    {
        cameraRotation = _cameraRotation;


    }

    public void RotateBulletSpawn(Vector3 _cameraRotation)
    {
        cameraRotation = _cameraRotation;


    }

    //Run every physics iteration
    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    //Perform movement based on velocity var

    void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);//takes postition wants to move to and move there as long as it doesnt collide, does physics check, easier than add force
        }
    }
    //Perform rotation
    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler (rotation));
        if (cam != null)
        {
            cam.transform.Rotate(-cameraRotation);
        }
        /*if (BulletSpawnMotor != null)
        {
            BulletSpawnMotor.transform.Rotate(-cameraRotation);
        }*/
    }



}
