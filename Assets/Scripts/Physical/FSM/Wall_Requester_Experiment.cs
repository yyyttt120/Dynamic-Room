using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Requester_Experiment : MonoBehaviour {

    public GameObject user;
    public GameObject wall_x;
    public GameObject wall_z;
    public GameObject[] locatePoints_obj;
    public GameObject[] targetWalls;

    private Animator x;
    private Animator y;
    private locatePoint[] locatePoints;
    // Use this for initialization

    public struct locatePoint
    {
        public GameObject obj;
        bool lastFrameState_x;
        bool thisFrameState_x;//did user cross this x axix boundary in this frame
        public bool trigger_x;
        bool lastFrameState_z;
        bool thisFrameState_z;//did user cross this y axix boundary in this frame
        public bool trigger_z;

        public locatePoint(GameObject obj,GameObject user)
        {
            this.obj = obj;
            lastFrameState_x = false;
            lastFrameState_z = false;
            if (user.transform.position.x > obj.transform.position.x)
                this.thisFrameState_x = true;
            else
                this.thisFrameState_x = false;
            if (user.transform.position.z > obj.transform.position.z)
                this.thisFrameState_z = true;
            else
                this.thisFrameState_z = false;
            trigger_x = false;
            trigger_z = false;
        }

        public void UpdatePoint(GameObject user)
        {
            //update x
            lastFrameState_x = thisFrameState_x;
            if (user.transform.position.x > obj.transform.position.x)
                this.thisFrameState_x = true;
            else
                this.thisFrameState_x = false;
            //print("thisframe =" + thisFrameState_x);
            //print("lastframe =" + lastFrameState_x);
            if (thisFrameState_x != lastFrameState_x)
                trigger_x = true;
            else
                trigger_x = false;

            //update z
            lastFrameState_z = thisFrameState_z;
            if (user.transform.position.z > obj.transform.position.z)
                this.thisFrameState_z = true;
            else
                this.thisFrameState_z = false;
            if (thisFrameState_z != lastFrameState_z)
                trigger_z = true;
            else
                trigger_z = false;
        }

    }
    void Start () {
        x = wall_x.GetComponent<Animator>();
        y = wall_z.GetComponent<Animator>();
        locatePoints = new locatePoint[4];
        for (int i = 0; i < 4; i++)
        {
            locatePoints[i] = new locatePoint(locatePoints_obj[i], user);
        }
    }
	
	// Update is called once per frame
	void Update () {
        for(int i=0;i<3;i++)
            locatePoints[i].UpdatePoint(user);
        //x
        DoSwitch(locatePoints[0], wall_x, targetWalls[1], targetWalls[0]);
        DoSwitch(locatePoints[1], wall_x, targetWalls[1], targetWalls[0]);
        //z
        DoSwitch(locatePoints[2], wall_z, targetWalls[3], targetWalls[2]);
        DoSwitch(locatePoints[0], wall_z, targetWalls[3], targetWalls[2]);
    }

    private void SwitchWall(GameObject wall,GameObject[] virtualWalls,int targetID)
    {

        Wall_To_Target wallController = wall.GetComponent<Wall_To_Target>();
        wallController.Set_Target(virtualWalls[targetID].transform.GetChild(0).gameObject);
        wall.GetComponent<Animator>().SetInteger("NearWallCounter", 100);
    }

    private void DoSwitch(locatePoint point, GameObject wall, GameObject target1, GameObject target2)
    {
        
        GameObject[] virtualWalls = new GameObject[] { target1, target2 };
        //do x
        if (wall == wall_x)
        {
            if (point.trigger_x)
            {
                print("trigger_x on");
                if (user.transform.position.x > point.obj.transform.position.x)
                    SwitchWall(wall, virtualWalls, 0);
                else
                    SwitchWall(wall, virtualWalls, 1);
            }
        }
        else
        { 
        //do z
            if (point.trigger_z)
            {
                print("trigger_z on");
                if (user.transform.position.z > point.obj.transform.position.z)
                    SwitchWall(wall, virtualWalls, 0);
                else
                    SwitchWall(wall, virtualWalls, 1);
            }
        }
    }
}
