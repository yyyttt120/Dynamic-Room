using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Allocator : MonoBehaviour
{
    public Robotic_Wall[] rWalls;
    public VirtualWall[] vWalls;
    public ServerObject dataRequester;
    public Detect_Area_forML detectBlock;
    private GameObject standbyPoints;
    GameObject user;

    private List<GameObject> standbyList;
    private int lastVirWallNum;
    private int lastFrameTarget;
    private int rWallNum;//the robotic wall which is going to match the target in this frame
    // Start is called before the first frame update
    void Start()
    {
        user = GameObject.Find("Camera (eye)");
        lastFrameTarget = 0;
        rWallNum = 0;
        lastVirWallNum = 0;
        standbyPoints = GameObject.Find("Stand_by");
        standbyList = new List<GameObject>();
        for(int i = 0; i < 4; i++)
        {
            standbyList.Add(standbyPoints.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int targetNum = Int32.Parse(dataRequester.result);
        print("target number ="+ targetNum);
        rWalls[0].wallToTarget_controller.Robot_Move_Switch(true);
       
        //change the robotic wall, once the target was changed
        if (targetNum != 0  && targetNum != lastFrameTarget)
        {
            SwitchRWall();
        }
        lastFrameTarget = targetNum;

        if (targetNum != 0)
        //rWalls[0].wallToTarget_controller.Set_Target(vWalls[targetNum-1].gameObject.transform.GetChild(0).gameObject);
        {
            foreach(VirtualWall vwall in vWalls)
            {
                vwall.SetMatchRWall(null);
            }
            //allocate a robotic wall for target virtual wall
            int targetRWall = AllocateRWall(vWalls[targetNum - 1].gameObject);
            vWalls[targetNum - 1].SetMatchRWall(rWalls[targetRWall].stateController);
            //rWalls[targetRWall].stateController.GetBehaviour<Wall_State>().SetTargetWall(vWalls[targetNum - 1].gameObject);
            //set virtual target for another robotic wall
            //int otherTarget = FindCorner(targetNum);
            //print($"other target = {otherTarget}");
            GameObject otherTarget = FindOtherTarget(targetNum);
            if (otherTarget != null)
                //rWalls[targetRWall == 0 ? 1 : 0].stateController.GetBehaviour<Wall_State>().SetTargetWall(otherTarget);     
                otherTarget.GetComponent<VirtualWall>().SetMatchRWall(rWalls[targetRWall == 0 ? 1 : 0].stateController);
            else
                rWalls[targetRWall == 0 ? 1 : 0].stateController.GetBehaviour<Wall_State>().SetTargetWall(FindStandbyPoint(rWalls[targetRWall == 0 ? 1 : 0]));
            /*if (vWalls[targetNum - 1].gameObject != vWalls[lastVirWallNum - 1].gameObject)
            {
                //if last target is not close to current target, set a close wall as the target of another robotic wall,else set the last target as the target of antoher robotic wall
                if(Mathf.Abs(lastVirWallNum - targetNum) >1)
                    vWalls[((lastVirWallNum - 1)==0 ? lastVirWallNum : lastVirWallNum-2)].SetMatchRWall(rWalls[targetRWall == 0 ? 1 : 0].stateController);
                else
                    vWalls[lastVirWallNum - 1].SetMatchRWall(rWalls[targetRWall == 0 ? 1 : 0].stateController);
            }*/
        }
        /*if(targetNum != 0)
            lastVirWallNum = targetNum;*/
    }
    private GameObject FindStandbyPoint(Robotic_Wall rwall)
    {
        standbyList.Sort(delegate (GameObject point1, GameObject point2) {
            Vector3 vec1 = point1.transform.position - rwall.transform.position;
            Vector3 vec2 = point2.transform.position - rwall.transform.position;
            vec1.y = 0;
            vec2.y = 0;
            if (vec1.magnitude > vec2.magnitude)
                return 1;
            else
                return -1;
        });
        print($"stand by point = {standbyList[0]}");
        return standbyList[0];
    }

    //besides the ML target, find a target for another robotic wall based on deteting block
    private GameObject FindOtherTarget(int MLtarget)
    {
        List<GameObject> WallsInArea = detectBlock.GetWallInAreaList();
        print($"amount of detected walls = {WallsInArea.Count}");
        foreach(GameObject wall in WallsInArea)
        {
            if (wall != vWalls[MLtarget - 1].gameObject)
            {
                print($"other target = {wall.name}");
                return wall;
            }
        }
        return null;
    }

    //find a virtual wall which could compose a corner with target virtual wall, return the ID of this virtual wall
    private int FindCorner(int target)
    {
        if (target < 3)
            return FindCloserWall(3, 4);
        else
            return FindCloserWall(1, 2);
    }

    private int FindCloserWall(int wall1,int wall2)
    {
        if (DistanceToVirWall(vWalls[wall1 - 1].gameObject, user) < DistanceToVirWall(vWalls[wall2 - 1].gameObject, user))
            return wall1;
        else
            return wall2;
    }

    //allocate a robotic wall to target virtual wall based on distance
    private int AllocateRWall(GameObject targetWall)
    {
        if (DistanceToVirWall(rWalls[0].transform.GetChild(0).gameObject, targetWall) >= DistanceToVirWall(rWalls[1].transform.GetChild(0).gameObject, targetWall))
            return 1;
        else
            return 0;
    }

    private float DistanceToVirWall(GameObject virWall, GameObject phyWall)
    {
        Vector3 vec = virWall.transform.position - phyWall.transform.position;
        vec = new Vector3(vec.x, 0, vec.z);
        return vec.magnitude;
    }
    private void SwitchRWall()
    {
        if (rWallNum == 0)
            rWallNum = 1;
        else
            rWallNum = 0;
    }
}
