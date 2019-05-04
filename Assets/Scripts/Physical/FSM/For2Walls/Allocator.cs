using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Allocator : MonoBehaviour
{
    public Robotic_Wall[] rWalls;
    public VirtualWall[] vWalls;
    public ServerObject dataRequester;

    private int lastFrameTarget;
    private int rWallNum;//the robotic wall which is going to match the target in this frame
    // Start is called before the first frame update
    void Start()
    {
        lastFrameTarget = 0;
        rWallNum = 0;
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
            rWalls[rWallNum].wallToTarget_controller.Set_Target(vWalls[targetNum-1].gameObject.transform.GetChild(0).gameObject);
    }

    private void SwitchRWall()
    {
        if (rWallNum == 0)
            rWallNum = 1;
        else
            rWallNum = 0;
    }
}
