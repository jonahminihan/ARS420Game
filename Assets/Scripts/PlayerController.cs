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
    public GameObject crossBowBolt;
    private GameObject currentBullet; //corresponds to which gun is equiped
    public GameObject bulletExplosPrefab;
    public GameObject teamObj;
    public Transform bulletSpawn;
    public Transform bulletSpawnRL;
    public Transform currentBulletSpawn;
    public int team = 0;
    public GameObject hitObj;
    private float timer = 1.5f;
    public float fireRate = 1.0f; // how fast someone can shoot
    public float fireRateTimer = 0; //timer to see if enough time has passed to shoot again
    public float currCountdownValue = 1.5f;
    public float bulletVelocity = 12;
    public IEnumerator coroutine;
    public bool[] gunCollection = new bool [4];

    //RL = 0, CB = 1, Sword = 2, Sniper = 3
    enum gunName { rocketLauncher, crossBow, sword, sniper };
    gunName currentGun = gunName.crossBow;



    void Start()
    {
        gunCollection[0] = false;
        gunCollection[0] = false;
        gunCollection[0] = false;
        gunCollection[0] = false;
        fireRateTimer = fireRate;
       teamObj = GameObject.Find("Teams");
        team = teamObj.GetComponent<TeamScript>().getTeam();  
        motor = GetComponent<PlayerMotor>();
        currentBulletSpawn = bulletSpawn;
        changeGun(currentGun);
        hitObj = gameObject;

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
        fireRateTimer += Time.deltaTime; // this was in the function below, but wasnt updating
        if (Input.GetKeyDown(KeyCode.Mouse0)){ //check to shoot

            //fireRateTimer += Time.unscaledDeltaTime;
            if(fireRateTimer >= fireRate){//check if enough time has passed to shoot again
                CmdFire(); //call the shoot function
                fireRateTimer = 0;
                Debug.Log("shoot");

            }
        }
        if(currentGun == gunName.crossBow){

            if (Input.GetKey(KeyCode.Mouse0))
            { //check to shoot

                //fireRateTimer += Time.unscaledDeltaTime;
                if (fireRateTimer >= fireRate)
                {//check if enough time has passed to shoot again
                    CmdFire(); //call the shoot function
                    fireRateTimer = 0;
                    Debug.Log("shoot");

                }
            }
        }

        if (hitObj != null && hitObj.name[0] == 'K')
        { //check that character is on the floor
            if (hitObj.name[1] == 'i')
            {
                //Debug.Log("killbox");
                //var health = gameObject.GetComponent<PlayerHealth>();
                //  health.TakeDamage(10);
                //gameObject.GetComponent<PlayerHealth>().TakeDamage(1000);

                //GetComponent<PlayerHealth>().TakeDamage(100);
                CmdKillBox();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (gunCollection[0] == true)
            {
                changeGun(gunName.rocketLauncher);
                Debug.Log("RL");
            }


        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            changeGun(gunName.crossBow);
            Debug.Log("CB");


        }

    }

    [Command]
    void CmdFire()
    {
        //This Function is done on the Server
        //create the bullet fromt he bullet prefab
        //var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        /*Quaternion quat = new Quaternion();
        quat.x = bulletSpawn.rotation.x + 90f;
        quat.y = bulletSpawn.rotation.y;
        quat.z = bulletSpawn.rotation.z;*/
        var bullet = (GameObject)Instantiate(currentBullet, currentBulletSpawn.position, currentBulletSpawn.rotation);
        //Quaternion quat = new Quaternion();
        //quat.x = bulletSpawn.rotation.x;
        //quat.y = 90f;
        //quat.z = bulletSpawn.rotation.z;
        //bullet.GetComponent<Rigidbody>().MoveRotation(quat);// = quat.y;

        //var bulletExplos = (GameObject)Instantiate(bulletExplosPrefab, bulletSpawn.position, bulletSpawn.rotation);
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
        /*
         * Vector3 bulletPlace;
        bulletPlace.x = bullet.transform.position.x;
        bulletPlace.y = bullet.transform.position.y;
        bulletPlace.z = bullet.transform.position.z;

        */Destroy(bullet, 2.0f);
        //var bulletExplos = (GameObject)Instantiate(bulletExplosPrefab, bulletPlace, bulletSpawn.rotation);
        //NetworkServer.Spawn(bulletExplos);

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
    [Command]
    public void CmdKillBox(){
        gameObject.GetComponent<PlayerHealth>().TakeDamage(1000);
    }

    private void changeGun(gunName gun){
        if (gun == gunName.rocketLauncher){ // RL
            fireRate = 2.0f; // how fast someone can shoot
            fireRateTimer = 0; //timer to see if enough time has passed to shoot again
            currentBullet = bulletPrefab;
            currentBulletSpawn = bulletSpawnRL;
            bulletVelocity = 12f;
            GameObject temp;
            GameObject gun1 = gameObject.transform.GetChild(1).gameObject; //grabs the camera of this game object
            gun1 = gun1.transform.GetChild(0).gameObject; //get gun on GameObject
            temp = gun1.transform.GetChild(2).gameObject; //get RL on gameobject
            temp.SetActive(true);
            temp = gun1.transform.GetChild(0).gameObject; //get CB on gameobject
            temp.SetActive(false);
        }
        if (gun == gunName.crossBow) // CB
        {
            fireRate = 0.1f; // how fast someone can shoot
            fireRateTimer = 0; //timer to see if enough time has passed to shoot again
            currentBullet = crossBowBolt;
            currentBulletSpawn = bulletSpawn;
            bulletVelocity = 20f;
            GameObject temp;
            GameObject gun1 = gameObject.transform.GetChild(1).gameObject; //grabs the camera of this game object
            gun1 = gun1.transform.GetChild(0).gameObject; //get gun on GameObject
            temp = gun1.transform.GetChild(2).gameObject; //get RL on gameobject
            temp.SetActive(false);
            temp = gun1.transform.GetChild(0).gameObject; //get CB on gameobject
            temp.SetActive(true);
            //currentBullet.gameObject.transform.rotation.y = 90f;
        }
        if (gun == gunName.sword) // Sword
        {
            fireRate = 1; // how fast someone can shoot
            fireRateTimer = 0; //timer to see if enough time has passed to shoot again
        }
        if (gun == gunName.sniper) // Sniper
        {
            fireRate = 1; // how fast someone can shoot
            fireRateTimer = 0; //timer to see if enough time has passed to shoot again
        }
    }
}
