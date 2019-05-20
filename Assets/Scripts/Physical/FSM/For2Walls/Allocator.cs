using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Allocator : MonoBehaviour
{
    public Robotic_Wall[] rWalls;
    public VirtualWall[] vWalls;
    public ServerObject dataRequester;
    private GameObject user;

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

            //set virtual target for another robotic wall
            int otherTarget = FindCorner(targetNum);
            print($"other target = {otherTarget}");
            vWalls[otherTarget - 1].SetMatchRWall(rWalls[targetRWall == 0 ? 1 : 0].stateController);
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
