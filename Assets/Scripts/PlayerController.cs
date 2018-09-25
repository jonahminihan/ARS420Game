using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerMotor))]

public class PlayerController : NetworkBehaviour {

    [SerializeField]// makes show up in inspector
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;

    private PlayerMotor motor;
    public GameObject bulletPrefab;
    public GameObject teamObj;
    public Transform bulletSpawn;
    public int team = 0;
    public GameObject hitObj;
    private float timer = 1.5f;
    public float currCountdownValue = 1.5f;
    public IEnumerator coroutine;

    void Start()
    {
       teamObj = GameObject.Find("Teams");
        team = teamObj.GetComponent<TeamScript>().getTeam();  
        motor = GetComponent<PlayerMotor>();
    }

    private void Update()
    {

        //Calculate movement veloxity as a 3d vector
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");
        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;
        //final movement vector
        Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;// normalized keeps speed consistent
        //apply movement
        motor.Move(_velocity);


        //calculate rotation as a 3d vector: (turning around)
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        //Apply rotation
        motor.Rotate(_rotation);


        //calculate camera rotation as a 3d vector: (turning around)
        float _xRot = Input.GetAxisRaw("Mouse Y");

        Vector3 _cameraRotation = new Vector3(_xRot, 0f, 0f) * lookSensitivity;

        //Apply rotation
        motor.RotateCamera(_cameraRotation);
        motor.RotateBulletSpawn(_cameraRotation);
        
        /////////////////////////////////////////////////////////////////////////////
        ////Movement and camera Done
        /////////////////////////////////////////////////////////////////////////////

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //CmdFire();
            if(hitObj.name[0] == 'F'){ //check that character is on the floor
                if(hitObj.name[1] == 'l'){
                    Jump();
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.Mouse0)){
            CmdFire();
        }

    }

    [Command]
    void CmdFire()
    {
        //This Function is done on the Server
        //create the bullet fromt he bullet prefab
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 12;

        NetworkServer.Spawn(bullet);
       //timer = timer - Time.deltaTime;
        //var timeTimer = new WaitForSeconds(1.5f);
        //yield WaitForSeconds(1.5f);
        //coroutine = StartCountdown();
        //StartCoroutine(StartCountdown());
        //StartCoroutine(coroutine);
        //if (timer <= 0){
           //bullet.GetComponent<BulletScript>().Death();
            //timer = 0.5f;
        //}

        Destroy(bullet, 2.0f);

    }
    void Jump(){ // lets player jump

        motor.PerformJump(); 
    }
    private void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject; //assign hit to collision object
        //var health = hit.GetComponent<PlayerHealth>();
        //var names = hit.name;
        hitObj = hit; // assign collision hit to hitObj for jump



    }
    private void OnCollisionExit(Collision collision)
    {
        hitObj = gameObject; // reassign hitobj so that jump works
    }

    public IEnumerator StartCountdown(){
        //currCountdownValue = countdownValue;
        //while (currCountdownValue > 0){
            yield return new WaitForSeconds(2.0f);
           //currCountdownValue--;
        //}
    }
}
