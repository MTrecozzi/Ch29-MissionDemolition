using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour {

    static private Slingshot s;

    // // fields set in the Unity Inspector pane
    [Header("Set in Inspector")]

    public GameObject prefabProjectile;

    public float velocityMult = 8f;

    public Transform leftArm;
    public Transform RightArm;

    public LineRenderer lineRendy;


    //// fields set dynamically
    [Header("Set Dynamically")]

    public GameObject launchPoint;

    public Vector3 launchPos;

    public GameObject projectile;

    public bool aimingMode;

    private Rigidbody projectileRigidbody;

    // will be set to non-static


    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (s == null) return Vector3.zero;

            return s.launchPos;
        }
    }




    private void Awake()
    {
        s = this;

        Transform launchPointTrans = transform.Find("LaunchPoint");

        launchPoint = launchPointTrans.gameObject;

        launchPoint.SetActive(false);

        launchPos = launchPointTrans.position;

        lineRendy.enabled = false;
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
        aimingMode = true;

        projectile = Instantiate(prefabProjectile) as GameObject;

        //projectile.transform.position = launchPos;

        //projectile.GetComponent<Rigidbody>().isKinematic = true;

        projectileRigidbody = projectile.GetComponent<Rigidbody>();

        projectileRigidbody.isKinematic = true;

        lineRendy.enabled = true;
    }

    private void Update()
    {

        if (!aimingMode) {
            
            return;
        }

        Vector3 mousePos2D = Input.mousePosition;

        mousePos2D.z = -Camera.main.transform.position.z;

        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D - launchPos;

        // Limit mouse delta to the radius of hte slingshot sphere collider

        float maxMagnitude = this.GetComponent<SphereCollider>().radius;

        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();

            mouseDelta *= maxMagnitude;
        }

        // Move Projectile to the New Position

        Vector3 projPos = launchPos + mouseDelta;

        projectile.transform.position = projPos;

        if (projectile != null)
        {
            Vector3[] linePositions = new Vector3[] { leftArm.position, projPos, RightArm.position };
            lineRendy.SetPositions(linePositions);
        }

        if (Input.GetMouseButtonUp(0))
        {
            aimingMode = false;

            projectileRigidbody.isKinematic = false;

            projectileRigidbody.velocity = -mouseDelta * velocityMult;

            FollowCam.POI = projectile;

            projectile = null;

            MissionDemolition.ShotFired();

            ProjectileLine.S.poi = projectile;

            lineRendy.enabled = false;


        }

        

    }
}
