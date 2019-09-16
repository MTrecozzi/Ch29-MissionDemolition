using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour {



    // // fields set in the Unity Inspector pane
    [Header("Set in Inspector")]
    public GameObject launchPoint;


    //// fields set dynamically
    //[Header("Set Dynamically")]

    public static Vector3 LAUNCH_POS = Vector3.zero;



    private void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");

        launchPoint = launchPointTrans.gameObject;

        launchPoint.SetActive(false);
    }

    private void OnMouseEnter()
    {
        print("SlingShot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        print("SlingShot:OnMOuseExit()");
        launchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {

    }

    private void Update()
    {
 


    }
}
