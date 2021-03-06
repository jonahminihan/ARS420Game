﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Scoreboardscript : NetworkBehaviour {
    //[SyncVar]
    public int teams0score = 0;
    //[SyncVar]
    public int teams1score=0;
    private int WinScore = 25;
    private GameObject endGameUI;// = GameObject.Find("EndGame");
    private Canvas canv;

    // Use this for initialization
    void Start () {
        endGameUI = GameObject.Find("EndGame");
        canv = endGameUI.GetComponent<Canvas>();
        //canv.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
        //if(isServer){

       
        //if (teams0score >= WinScore){
        //    //endGameUI.enabled = true;
        //    canv.enabled = true;
        //        RpcTurnEndGameOn();
        //}
        //if (teams1score >=WinScore){
        //    canv.enabled = true;
        //        RpcTurnEndGameOn();
        //    }
        //}
        if (teams0score >= WinScore)
        {
            //endGameUI.enabled = true;
            canv.enabled = true;
            //RpcTurnEndGameOn();
        }
        if (teams1score >= WinScore)
        {
            canv.enabled = true;
           // RpcTurnEndGameOn();
        }
    }



    public void updatescore(int teams)
    {
        if (teams==0)
        {
            teams1score++;
        }
        else if(teams==1)
        {
            teams0score++;
        }
    }

    [ClientRpc]
    public void RpcTurnEndGameOn(){
        endGameUI = GameObject.Find("EndGame");
        canv = endGameUI.GetComponent<Canvas>();
        canv.enabled = true;

    }

    //[Command]
    //public void CmdTurn

    [ClientRpc]
    public void RpcUpdateScore(int teamss, int team0, int team1)
    {
        //if (teamss == 0)
        //{
        //    teams1score++;
        //}
        //else if (teamss == 1)
        //{
        //    teams0score++;
        //}
        teams0score = team0;
        teams1score = team1;

    }

    [Command]
    public void CmdUpdateScore(int teamsss){
        if (teamsss == 0)
        {
            teams1score++;
        }
        else if (teamsss == 1)
        {
            teams0score++;
        }
        RpcUpdateScore(teamsss, teams0score, teams1score);
    }
}
