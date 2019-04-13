﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Avoid : MonoBehaviour {

    private SteamVR_TrackedObject tracker;
    private SteamVR_Controller.Device device;
    private Robotic_Wall rWall;
    private int layMask = (1 << 9) | (1 << 10);
    private RaycastHit hit;
    private Vector3 wallPredict;//the moving direction of the robotic wall
    private Vector3 avoidanceVector;
    public float detectRange;//the range of predicted area
    public float force;//the force to avoid obstacle
    public float nonAvoidRange;//range of area where no need to avoid
                        // Use this for initialization
    void Start() {
        rWall = new Robotic_Wall();
        rWall.Set_Robotic_Wall(this.gameObject);
        tracker = transform.parent.gameObject.GetComponent<SteamVR_TrackedObject>();
        device = SteamVR_Controller.Input((int)tracker.index);
        wallPredict = Vector3.zero;
    }

    // Update is called once per frame
    void Update() {
        //print("tracker" + tracker.gameObject.name);
        device = SteamVR_Controller.Input((int)tracker.index);
        //wallPredict = GetMovingDir();
        //float angle = rWall.roomba_controller.AngleSigned(-this.transform.forward, device.velocity, Vector3.up);
        //print("angle = " + angle);
        //print("distance ="+ gameObject.name + wallPredict.magnitude);
        if (wallPredict.magnitude > nonAvoidRange)
        {
            //if (angle > -20 && angle < 20 || angle > 160 && angle < 180 || angle > -180 && angle < -160)// when the wall are moving ahead (not rotating)
            //{
                //print("huo" + gameObject.name);
                Color color = Color.red;
                Debug.DrawRay(this.transform.position, wallPredict.normalized * Mathf.Min(detectRange, wallPredict.magnitude), color, 0.1f, true);
                if (Physics.Raycast(this.transform.position, wallPredict.normalized, out hit, Mathf.Min(detectRange, wallPredict.magnitude), layMask))
                {
                    //
                    if (hit.collider.gameObject != gameObject)
                    {
                        print(gameObject.name + "ray hit" + hit.collider.gameObject.name);
                        avoidanceVector += new Vector3(hit.normal.z, 0, -hit.normal.x).normalized * force;
                        avoidanceVector.y = 0;
                    }
                /*else
                    avoidanceVector = Vector3.zero;*/
                if (avoidanceVector.magnitude > 4)
                    avoidanceVector = avoidanceVector.normalized * 4;

                }
                else
                    avoidanceVector /= 1.05f;
            //}
            //else
               // avoidanceVector = Vector3.zero;
        }
        else
            avoidanceVector = Vector3.zero;
    }


    private Vector3 GetMovingDir()
    {
        double angle;//the angle between wall's right direction and velocity of the wall
        angle = rWall.roomba_controller.AngleSigned(-this.transform.forward, device.velocity, Vector3.up);
        if (angle < 90 && angle > -90)
            return this.transform.right;
        else
            return -this.transform.right;
    }

    public void SetDetectDirection(GameObject target)
    {
        Vector3 dir = target.transform.position - this.transform.position;
        dir.y = 0;
        wallPredict = dir;
    }

    public Vector3 GetAviodanceVector()
    {
        return avoidanceVector;
    }
}
