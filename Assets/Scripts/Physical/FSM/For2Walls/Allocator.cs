using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Allocator : MonoBehaviour
{
    public Robotic_Wall[] rWalls;
    public VirtualWall[] vWalls;
    public ServerObject dataRequester;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int targetNum = Int32.Parse(dataRequester.result);
        print("target number ="+ targetNum);
        rWalls[0].wallToTarget_controller.Robot_Move_Switch(true);
        if(targetNum != 0)
            rWalls[0].wallToTarget_controller.Set_Target(vWalls[targetNum-1].gameObject);
    }
}
