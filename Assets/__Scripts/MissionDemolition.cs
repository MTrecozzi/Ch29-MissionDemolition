﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;                                                       



public enum GameMode
{                                                         
    idle,
    playing,
    levelEnd
}



public class MissionDemolition : MonoBehaviour
{

    static public MissionDemolition S; 

    [Header("Set in Inspector")]

    public Text uitLevel;  
    public Text uitShots; 
    public Text uitButton; 
    public Vector3 castlePos; 
    public GameObject[] castles;   

    [Header("Set Dynamically")]
    public int level;     // The current level
    public int levelMax;  // The number of levels
    public int shotsTaken;
    public GameObject castle;    // The current castle
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; // FollowCam mode

    void Start()
    {
        S = this; // Define the Singleton
        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    public void Restart()
    {
        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void CreateCastle()
    {
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
    }

    void StartLevel()
    {

        if (castle != null)
        {
            Destroy(castle);
        }

        // Destroy old projectiles
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");

        foreach (GameObject pTemp in gos)
        {

            Destroy(pTemp);

        }

        Invoke("CreateCastle", 0.1f);
        shotsTaken = 0;


        SwitchView("Show Both");
        ProjectileLine.S.Clear();


        Goal.goalMet = false;


        UpdateGUI();

        mode = GameMode.playing;

    }



    void UpdateGUI()
    {

        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;

    }




    void Update()
    {

        UpdateGUI();

        if ((mode == GameMode.playing) && Goal.goalMet)
        {

            mode = GameMode.levelEnd;

            SwitchView("Show Both");

            Invoke("NextLevel", 2f);

        }

    }



    void NextLevel()
    {

        level++;

        if (level == levelMax)
        {

            level = 0;

        }

        StartLevel();

    }



    public void SwitchView(string eView = "")
    {                                   

        if (eView == "")
        {
            eView = uitButton.text;
        }

        showing = eView;

        switch (showing)
        {

            case "Show Slingshot":

                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;

            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;

            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;

        }

    }

    public static void ShotFired()
    {                                            
        S.shotsTaken++;
        if (S.shotsTaken >= 4)
        {
            S.Restart();
        }
    }



}