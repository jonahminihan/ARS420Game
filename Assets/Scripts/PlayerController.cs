using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerMotor))]

public class PlayerController : NetworkBehaviour {

    [SerializeField]// makes show up in inspector
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;


    //HandleSound
    public AudioClip shootSoundCB;
    public AudioClip shootSoundRL;
    public AudioClip shootSoundER;
    public AudioClip jumpSound;
    public AudioClip runSound;
    private float gunSoundVol;
    private float runSoundVol;
    public AudioSource source;



    /// <summary>
    /// 
    /// </summary>

    private PlayerMotor motor;
    public Texture red;
    public Texture blue;
    private Renderer rendererSkin;
    private GameObject playerModel;
    public GameObject bulletPrefab;
    public GameObject crossBowBolt;
    public GameObject energyRifleBullet;
    public GameObject localBulletPrefab;
    public GameObject localCrossBowBolt;
    public GameObject localEnergyRifleBullet;
    private GameObject currentBullet; //corresponds to which gun is equiped
    private GameObject localCurrentBullet;
    public GameObject bulletExplosPrefab;
    public GameObject teamObj;
    public Transform bulletSpawn;
    public Transform bulletSpawnRL;
    public Transform bulletSpawnER;
    public Transform currentBulletSpawn;
    public int team = 0;
    public GameObject hitObj;
    private float timer = 1.5f;
    public float fireRate = 1.0f; // how fast someone can shoot
    public float fireRateTimer = 0; //timer to see if enough time has passed to shoot again
    public float reloadTime = 3.0f; // how long it takes to reload
    public float reloadTimer = 0.0f; //timer to see if enough time has passed to reload
    public float currCountdownValue = 1.5f;

    private float bulletVelocity = 5;
    public float bulletVelocityRL = 40;
    private float bulletVelocityCB = 55.0f;
    public float bulletVelocityER = 70;


    public int ammoCount = 30;
    public int maxAmmoCount = 30;
    public IEnumerator coroutine;
    public bool[] gunCollection = new bool [4];

    //RL = 0, CB = 1, Sword = 2, Sniper = 3
    enum gunName { rocketLauncher, crossBow, sword, sniper };
    gunName currentGun = gunName.crossBow;

    // character UI
    private GameObject characterUI;// = GameObject.Find("EndGame");
    private GameObject ammoUI;
    private GameObject healthUI;
    private GameObject redTeamScoreUI;
    private GameObject blueTeamScoreUI;
    private GameObject scoreboard;
    private Canvas canv;
    private GameObject lobbyWait;
    public bool lobbyActive = false;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        bulletVelocity = bulletVelocityCB;
        gunCollection[0] = false;
        gunCollection[1] = true;
        gunCollection[2] = false;
        gunCollection[3] = false;
        fireRateTimer = fireRate;
        teamObj = GameObject.Find("Teams");
        team = teamObj.GetComponent<TeamScript>().getTeam(); 
        //change texture---------------------------------------------------------------------
        motor = GetComponent<PlayerMotor>();
        currentBulletSpawn = bulletSpawn;
        changeGun(currentGun);
        CmdchangeGun(currentGun);
        hitObj = gameObject;
        characterUI = GameObject.Find("characterUI");
        //characterUI = gameObject.transform.GetChild(1).gameObject; //grabs the camera of this game object
        //characterUI = characterUI.transform.GetChild(1).gameObject; //get character UI on GameObject
        characterUI = characterUI.transform.GetChild(3).gameObject; //get image on gameobject
        ammoUI = characterUI.transform.GetChild(0).gameObject; //get ammo on gameobject
        healthUI = characterUI.transform.GetChild(1).gameObject; //get ammo on gameobject
        ammoUI.GetComponent < Text > ().text = ammoCount.ToString();

        playerModel = gameObject.transform.GetChild(0).gameObject;
        playerModel = playerModel.transform.GetChild(0).gameObject;
        playerModel = playerModel.transform.GetChild(0).gameObject;
        playerModel = playerModel.transform.GetChild(1).gameObject;
        rendererSkin = playerModel.GetComponent<Renderer>();
        if(team == 0){
            rendererSkin.material.SetTexture("_MainTex", blue);
        }
        else{
            rendererSkin.material.SetTexture("_MainTex", red);

        }



        healthUI.GetComponent<Text>().text = gameObject.GetComponent<PlayerHealth>().currentHealth.ToString();
        scoreboard = GameObject.Find("Scoreboard");
        redTeamScoreUI = characterUI.transform.GetChild(2).gameObject; //get ammo on gameobject
        blueTeamScoreUI = characterUI.transform.GetChild(3).gameObject;
        redTeamScoreUI.GetComponent<Text>().text = scoreboard.GetComponent<Scoreboardscript>().teams0score.ToString();
        blueTeamScoreUI.GetComponent<Text>().text = scoreboard.GetComponent<Scoreboardscript>().teams1score.ToString();

        lobbyWait = GameObject.Find("lobbyWait");
        gunSoundVol = 0.05f;
        runSoundVol = 0.5f;
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //Character UI
        ammoUI.GetComponent<Text>().text = ammoCount.ToString();
        healthUI.GetComponent<Text>().text = gameObject.GetComponent<PlayerHealth>().currentHealth.ToString();
        redTeamScoreUI.GetComponent<Text>().text = scoreboard.GetComponent<Scoreboardscript>().teams0score.ToString();
        blueTeamScoreUI.GetComponent<Text>().text = scoreboard.GetComponent<Scoreboardscript>().teams1score.ToString();
        //CmdUpdateScore();


        //Calculate movement veloxity as a 3d vector
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");
        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;
        //final movement vector
        Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;// normalized keeps speed consistent
        //apply movement
        if(lobbyActive == true){
            motor.Move(_velocity);
           
        }



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


        //check lobby full
        if (lobbyWait.GetComponent<lobbyWait>().lobbyGood == true){
            lobbyActive = true;
        }


        /////////////////////////////////////////////////////////////////////////////
        ////Movement and camera Done
        /////////////////////////////////////////////////////////////////////////////

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //CmdFire();
            if(hitObj.name[0] == 'F'){ //check that character is on the floor
                if(hitObj.name[1] == 'l'){
                    Jump();
                    source.PlayOneShot(jumpSound, runSoundVol);
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            source.PlayOneShot(runSound, runSoundVol);

        }
        if(Input.GetKeyUp(KeyCode.W)){
            source.Stop();
        }
        fireRateTimer += Time.deltaTime; // this was in the function below, but wasnt updating
        if (ammoCount > 0 && lobbyActive == true)
        {

            if (Input.GetKeyDown(KeyCode.Mouse0))
            { //check to shoot

                //fireRateTimer += Time.unscaledDeltaTime;
                if (fireRateTimer >= fireRate)
                {//check if enough time has passed to shoot again
                    CmdFire(); //call the shoot function
                    localFire();
                    RpcFire();
                    fireRateTimer = 0;
                    Debug.Log("shoot");
                    ammoCount--;
                    //source.PlayOneShot(shootSound, .5f);
                    if (!isServer)
                    {
                    if (currentGun == gunName.rocketLauncher)
                    { // RL
                        source.PlayOneShot(shootSoundRL, gunSoundVol);
                    }
                    if (currentGun == gunName.crossBow)
                    { // RL
                        source.PlayOneShot(shootSoundCB, gunSoundVol);
                    }
                    if (currentGun == gunName.sniper)
                    { // RL
                        source.PlayOneShot(shootSoundER, gunSoundVol);
                    }
                }

                }
            }
            if (currentGun == gunName.crossBow)
            {

                if (Input.GetKey(KeyCode.Mouse0))
                { //check to shoot

                    //fireRateTimer += Time.unscaledDeltaTime;
                    if (fireRateTimer >= fireRate)
                    {//check if enough time has passed to shoot again
                        CmdFire(); //call the shoot function
                        localFire();
                        RpcFire();
                        fireRateTimer = 0;
                        Debug.Log("shoot");
                        ammoCount--;


                    }
                }
            }
        }
        else{
            reloadTimer += Time.deltaTime;
            if (reloadTimer >= reloadTime){
                ammoCount = maxAmmoCount;
                reloadTimer = 0;
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
                CmdchangeGun(gunName.rocketLauncher);
                RpcchangeGun(gunName.rocketLauncher);
                Debug.Log("RL");
            }


        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (gunCollection[1] == true)
            {
                CmdchangeGun(gunName.crossBow);
                changeGun(gunName.crossBow);
                RpcchangeGun(gunName.crossBow);
                Debug.Log("CB");
            }


        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (gunCollection[3] == true)
            {
                CmdchangeGun(gunName.sniper);
                changeGun(gunName.sniper);
                RpcchangeGun(gunName.sniper);
                Debug.Log("ER");
            }


        }
        if (Input.GetKeyDown(KeyCode.Alpha0)){
           if(Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else{
                Cursor.lockState = CursorLockMode.None;
            }
        }
       

    }

    [Command]
    void CmdFire()
    {

        var bullet = (GameObject)Instantiate(currentBullet, currentBulletSpawn.position, currentBulletSpawn.rotation);

        //var bulletExplos = (GameObject)Instantiate(bulletExplosPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletVelocity;

        NetworkServer.Spawn(bullet);
        //source.PlayOneShot(shootSoundCB, .5f);
        if (currentGun == gunName.rocketLauncher)
        { // RL
            source.PlayOneShot(shootSoundRL, gunSoundVol);
        }
        if (currentGun == gunName.crossBow)
        { // RL
            source.PlayOneShot(shootSoundCB, gunSoundVol);
        }
        if (currentGun == gunName.sniper)
        { // RL
            source.PlayOneShot(shootSoundER, gunSoundVol);
        }
        Destroy(bullet, 2.0f);
        RpcGunShotSound();
    }

    [ClientRpc]
    void RpcGunShotSound(){
        if (currentGun == gunName.rocketLauncher)
        { // RL
            source.PlayOneShot(shootSoundRL, gunSoundVol);
        }
        if (currentGun == gunName.crossBow)
        { // RL
            source.PlayOneShot(shootSoundCB, gunSoundVol);
        }
        if (currentGun == gunName.sniper)
        { // RL
            source.PlayOneShot(shootSoundER, gunSoundVol);
        }
    }

    [ClientRpc]
    void RpcFire()
    {

        if (currentGun != gunName.rocketLauncher)
        {


            var bullet = (GameObject)Instantiate(localCurrentBullet, currentBulletSpawn.position, currentBulletSpawn.rotation);

            //var bulletExplos = (GameObject)Instantiate(bulletExplosPrefab, bulletSpawn.position, bulletSpawn.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletVelocity;

            NetworkServer.Spawn(bullet);
            //source.PlayOneShot(shootSound, .5f);
            Destroy(bullet, 2.0f);
        }



    }

    void localFire()
    {
        if(currentGun != gunName.rocketLauncher){

       
        var bullet = (GameObject)Instantiate(localCurrentBullet, currentBulletSpawn.position, currentBulletSpawn.rotation);

        //var bulletExplos = (GameObject)Instantiate(bulletExplosPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletVelocity;

       NetworkServer.Spawn(bullet);
            //source.PlayOneShot(shootSound, .5f);
            Destroy(bullet, 2.0f);
        }

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
            currentGun = gunName.rocketLauncher;
            reloadTime = 2.0f;
            maxAmmoCount = 1;
            ammoCount = maxAmmoCount;

            fireRate = 2.0f; // how fast someone can shoot
            fireRateTimer = 0; //timer to see if enough time has passed to shoot again
            currentBullet = bulletPrefab;
            localCurrentBullet = localBulletPrefab;
            currentBulletSpawn = bulletSpawnRL;
            bulletVelocity = bulletVelocityRL;
            GameObject temp;
            GameObject gun1 = gameObject.transform.GetChild(1).gameObject; //grabs the camera of this game object
            gun1 = gun1.transform.GetChild(0).gameObject; //get gun on GameObject
            temp = gun1.transform.GetChild(2).gameObject; //get RL on gameobject
            temp.SetActive(true);
            temp = gun1.transform.GetChild(0).gameObject; //get CB on gameobject
            temp.SetActive(false);
            temp = gun1.transform.GetChild(4).gameObject; //get ER on gameobject
            temp.SetActive(false);
        }
        if (gun == gunName.crossBow) // CB
        {
            currentGun = gunName.crossBow;
            reloadTime = 3.0f;
            maxAmmoCount = 30;
            ammoCount = maxAmmoCount;
            fireRate = 0.1f; // how fast someone can shoot
            fireRateTimer = 0; //timer to see if enough time has passed to shoot again
            currentBullet = crossBowBolt;
            localCurrentBullet = localCrossBowBolt;
            currentBulletSpawn = bulletSpawn;
            bulletVelocity = bulletVelocityCB;




            GameObject temp;
            GameObject gun1 = gameObject.transform.GetChild(1).gameObject; //grabs the camera of this game object
            gun1 = gun1.transform.GetChild(0).gameObject; //get gun on GameObject
            temp = gun1.transform.GetChild(2).gameObject; //get RL on gameobject
            temp.SetActive(false);
            temp = gun1.transform.GetChild(0).gameObject; //get CB on gameobject
            temp.SetActive(true);
            temp = gun1.transform.GetChild(4).gameObject; //get ER on gameobject
            temp.SetActive(false);
            //currentBullet.gameObject.transform.rotation.y = 90f;
        }
        if (gun == gunName.sword) // Sword
        {
            currentGun = gunName.sword;
            fireRate = 1; // how fast someone can shoot
            fireRateTimer = 0; //timer to see if enough time has passed to shoot again
        }
        if (gun == gunName.sniper) // Sniper
        {
            currentGun = gunName.sniper;
            reloadTime = 3.0f;
            maxAmmoCount = 10;
            ammoCount = maxAmmoCount;
            fireRate = 1; // how fast someone can shoot
            fireRateTimer = 0; //timer to see if enough time has passed to shoot again
            currentBullet = energyRifleBullet;
            localCurrentBullet = localEnergyRifleBullet;
            currentBulletSpawn = bulletSpawnER;
            bulletVelocity = bulletVelocityER;
            GameObject temp;
            GameObject gun1 = gameObject.transform.GetChild(1).gameObject; //grabs the camera of this game object
            gun1 = gun1.transform.GetChild(0).gameObject; //get gun on GameObject
            temp = gun1.transform.GetChild(2).gameObject; //get RL on gameobject
            temp.SetActive(false);
            temp = gun1.transform.GetChild(0).gameObject; //get CB on gameobject
            temp.SetActive(false);
            temp = gun1.transform.GetChild(4).gameObject; //get ER on gameobject
            temp.SetActive(true);
        }
    }

    [Command]
    private void CmdchangeGun(gunName gun)
    {
        if (gun == gunName.rocketLauncher)
        { // RL

            reloadTime = 2.0f;
            maxAmmoCount = 1;
            ammoCount = maxAmmoCount;

            fireRate = 2.0f; // how fast someone can shoot
            fireRateTimer = 0; //timer to see if enough time has passed to shoot again
            currentBullet = bulletPrefab;
            currentBulletSpawn = bulletSpawnRL;
            bulletVelocity = bulletVelocityRL;
            GameObject temp;
            GameObject gun1 = gameObject.transform.GetChild(1).gameObject; //grabs the camera of this game object
            gun1 = gun1.transform.GetChild(0).gameObject; //get gun on GameObject
            temp = gun1.transform.GetChild(2).gameObject; //get RL on gameobject
            temp.SetActive(true);
            temp = gun1.transform.GetChild(0).gameObject; //get CB on gameobject
            temp.SetActive(false);
            temp = gun1.transform.GetChild(4).gameObject; //get ER on gameobject
            temp.SetActive(false);
        }
        if (gun == gunName.crossBow) // CB
        {
            reloadTime = 3.0f;
            maxAmmoCount = 30;
            ammoCount = maxAmmoCount;
            fireRate = 0.1f; // how fast someone can shoot
            fireRateTimer = 0; //timer to see if enough time has passed to shoot again
            currentBullet = crossBowBolt;
            currentBulletSpawn = bulletSpawn;
            bulletVelocity = bulletVelocityCB;




            GameObject temp;
            GameObject gun1 = gameObject.transform.GetChild(1).gameObject; //grabs the camera of this game object
            gun1 = gun1.transform.GetChild(0).gameObject; //get gun on GameObject
            temp = gun1.transform.GetChild(2).gameObject; //get RL on gameobject
            temp.SetActive(false);
            temp = gun1.transform.GetChild(0).gameObject; //get CB on gameobject
            temp.SetActive(true);
            temp = gun1.transform.GetChild(4).gameObject; //get ER on gameobject
            temp.SetActive(false);
            //currentBullet.gameObject.transform.rotation.y = 90f;
        }
        if (gun == gunName.sword) // Sword
        {
            fireRate = 1; // how fast someone can shoot
            fireRateTimer = 0; //timer to see if enough time has passed to shoot again
        }
        if (gun == gunName.sniper) // Sniper
        {
            reloadTime = 3.0f;
            maxAmmoCount = 10;
            ammoCount = maxAmmoCount;
            fireRate = 1; // how fast someone can shoot
            fireRateTimer = 0; //timer to see if enough time has passed to shoot again
            currentBullet = energyRifleBullet;
            currentBulletSpawn = bulletSpawnER;
            bulletVelocity = bulletVelocityER;
            GameObject temp;
            GameObject gun1 = gameObject.transform.GetChild(1).gameObject; //grabs the camera of this game object
            gun1 = gun1.transform.GetChild(0).gameObject; //get gun on GameObject
            temp = gun1.transform.GetChild(2).gameObject; //get RL on gameobject
            temp.SetActive(false);
            temp = gun1.transform.GetChild(0).gameObject; //get CB on gameobject
            temp.SetActive(false);
            temp = gun1.transform.GetChild(4).gameObject; //get ER on gameobject
            temp.SetActive(true);
        }
    }

    [ClientRpc]
    private void RpcchangeGun(gunName gun)
    {
        if (gun == gunName.rocketLauncher)
        { // RL

            reloadTime = 2.0f;
            maxAmmoCount = 1;
            ammoCount = maxAmmoCount;

            fireRate = 2.0f; // how fast someone can shoot
            fireRateTimer = 0; //timer to see if enough time has passed to shoot again
            currentBullet = bulletPrefab;
            currentBulletSpawn = bulletSpawnRL;
            bulletVelocity = bulletVelocityRL;
            GameObject temp;
            GameObject gun1 = gameObject.transform.GetChild(1).gameObject; //grabs the camera of this game object
            gun1 = gun1.transform.GetChild(0).gameObject; //get gun on GameObject
            temp = gun1.transform.GetChild(2).gameObject; //get RL on gameobject
            temp.SetActive(true);
            temp = gun1.transform.GetChild(0).gameObject; //get CB on gameobject
            temp.SetActive(false);
            temp = gun1.transform.GetChild(4).gameObject; //get ER on gameobject
            temp.SetActive(false);
        }
        if (gun == gunName.crossBow) // CB
        {
            reloadTime = 3.0f;
            maxAmmoCount = 30;
            ammoCount = maxAmmoCount;
            fireRate = 0.1f; // how fast someone can shoot
            fireRateTimer = 0; //timer to see if enough time has passed to shoot again
            currentBullet = crossBowBolt;
            currentBulletSpawn = bulletSpawn;
            bulletVelocity = bulletVelocityCB;




            GameObject temp;
            GameObject gun1 = gameObject.transform.GetChild(1).gameObject; //grabs the camera of this game object
            gun1 = gun1.transform.GetChild(0).gameObject; //get gun on GameObject
            temp = gun1.transform.GetChild(2).gameObject; //get RL on gameobject
            temp.SetActive(false);
            temp = gun1.transform.GetChild(0).gameObject; //get CB on gameobject
            temp.SetActive(true);
            temp = gun1.transform.GetChild(4).gameObject; //get ER on gameobject
            temp.SetActive(false);
            //currentBullet.gameObject.transform.rotation.y = 90f;
        }
        if (gun == gunName.sword) // Sword
        {
            fireRate = 1; // how fast someone can shoot
            fireRateTimer = 0; //timer to see if enough time has passed to shoot again
        }
        if (gun == gunName.sniper) // Sniper
        {
            reloadTime = 3.0f;
            maxAmmoCount = 10;
            ammoCount = maxAmmoCount;
            fireRate = 1; // how fast someone can shoot
            fireRateTimer = 0; //timer to see if enough time has passed to shoot again
            currentBullet = energyRifleBullet;
            currentBulletSpawn = bulletSpawnER;
            bulletVelocity = bulletVelocityER;
            GameObject temp;
            GameObject gun1 = gameObject.transform.GetChild(1).gameObject; //grabs the camera of this game object
            gun1 = gun1.transform.GetChild(0).gameObject; //get gun on GameObject
            temp = gun1.transform.GetChild(2).gameObject; //get RL on gameobject
            temp.SetActive(false);
            temp = gun1.transform.GetChild(0).gameObject; //get CB on gameobject
            temp.SetActive(false);
            temp = gun1.transform.GetChild(4).gameObject; //get ER on gameobject
            temp.SetActive(true);
        }
    }

    [Command]
    private void CmdUpdateScore(){

        redTeamScoreUI.GetComponent<Text>().text = scoreboard.GetComponent<Scoreboardscript>().teams0score.ToString();
        blueTeamScoreUI.GetComponent<Text>().text = scoreboard.GetComponent<Scoreboardscript>().teams1score.ToString();


    }
}
