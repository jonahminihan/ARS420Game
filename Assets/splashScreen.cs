using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class splashScreen : MonoBehaviour {
    private GameObject mmCanv;
    //private Canvas mmCanv;
    private CanvasGroup splashCanv;
    private CanvasGroup mmCanvGroup;
    private bool splashOn = false;
    private bool splashOff = false;
    private bool mmOff = true;
    private float splashOnTimer = 0;
    private float splashMaxTime = 5;
    private float mmOnTimer = 0;
    private float mmMaxTime = 0;
    private float incrementAlpha; // amount to add to alpha each tick

    // Use this for initialization
    void Start () {
        mmCanv = GameObject.Find("Canvas");
        mmCanv.GetComponent<CanvasGroup>().alpha = 0;
        mmCanvGroup = mmCanv.GetComponent<CanvasGroup>();
        splashCanv = gameObject.GetComponent<CanvasGroup>();
        splashCanv.alpha = 0;
        incrementAlpha = 1 / (splashMaxTime * 60);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
        Cursor.lockState = CursorLockMode.None;
       
       
            if (Input.GetKey("escape"))
            {
                Application.Quit();
            }
       

    }

    private void FixedUpdate()
    {
        ////////////////////////
        /// Splash Screen
        /// ////////////////////
        if (splashOnTimer <= splashMaxTime && splashOn == false)
        {
            splashOnTimer = splashOnTimer + Time.deltaTime;
            splashCanv.alpha = splashCanv.alpha + incrementAlpha;
           //Debug.Log("1");
            //Debug.Log(splashOnTimer);
        }
        else if (splashOnTimer > splashMaxTime && splashOn == false)
        {
            splashOnTimer = 0;
            splashOn = true;
           // Debug.Log("2");
        }
        if (splashOnTimer <= splashMaxTime && splashOn == true && splashOff == false)
        {
            splashOnTimer = splashOnTimer + Time.deltaTime;
            splashCanv.alpha = splashCanv.alpha - incrementAlpha;
           //Debug.Log("3");
        }
        else if (splashOnTimer > splashMaxTime && splashOn == true && splashOff == false)
        {
            splashOnTimer = 0;
            splashOff = true;
            //Debug.Log("4");
        }
        ////////////////////////
        /// Main menu
        /// ////////////////////


        if (splashOnTimer <= (splashMaxTime + 5) && splashOn == true && splashOff == true && mmOff == true)
        {
            splashOnTimer = splashOnTimer + Time.deltaTime;
            mmCanvGroup.alpha = mmCanvGroup.alpha + incrementAlpha;
            //Debug.Log("1");
            //Debug.Log(splashOnTimer);
        }
        else if (splashOnTimer > splashMaxTime && splashOn == true && splashOff == true && mmOff == true)
        {
            splashOnTimer = 0;
            mmOff = false;
           //Debug.Log("2");
        }
        //if (splashOnTimer <= splashMaxTime && splashOn == true && splashOff == false)
        //{
        //    splashOnTimer = splashOnTimer + Time.deltaTime;
        //    splashCanv.alpha = splashCanv.alpha - incrementAlpha;
        //    Debug.Log("3");
        //}
        //else if (splashOnTimer > splashMaxTime && splashOn == true && splashOff == false)
        //{
        //    splashOnTimer = 0;
        //    splashOff = true;
        //    Debug.Log("4");
        //}

    }

    public void QuitGame(){
        Application.Quit();
    }
}
