using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Functions;

public class Allocator_3Walls : Allocator
{
    private int targetNum = 1;
    private List<GameObject> avilable_standbyList;
    private List<Robotic_Wall> standby_rwalls;
    private Robotic_Wall primary_rwalls_lastFrame;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        user = GameObject.Find("Camera (eye)");
        standbyPoints = GameObject.Find("Stand_by");
        standbyList = new List<GameObject>();
        avilable_standbyList = new List<GameObject>();

        for (int i = 0; i < 4; i++)
        {
            standbyList.Add(standbyPoints.transform.GetChild(i).gameObject);
            avilable_standbyList.Add(standbyPoints.transform.GetChild(i).gameObject);

        }
        if (vWalls == null)
            vWalls = new VirtualWall[4];

        GameObject[] obj_vwalls = GameObject.FindGameObjectsWithTag("Wall");
        vWalls_all = new VirtualWall[obj_vwalls.Length];
        for (int i = 0; i < obj_vwalls.Length; i++)
            vWalls_all[i] = obj_vwalls[i].GetComponent<VirtualWall>();
        standby_rwalls = new List<Robotic_Wall>();
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        //if (timer > 0.5f)
        {
            timer = 0;
            UpdateCurrentVWalls();
            targetNum = Int32.Parse(dataRequester.result);
            print("target number =" + targetNum);
            if (targetNum != 0)
            {
                foreach (VirtualWall vwall in vWalls_all)
                {
                    //print($"********* {vwall.name}  ***************");
                    vwall.SetMatchRWall(null);
                }

            }
            /* initiate the standby robotic wall list at start of every frame */
            standby_rwalls.Clear();
            foreach (Robotic_Wall rwall in rWalls)
            {
                //print($"********* {rwall.name}");
                standby_rwalls.Add(rwall);
            }
            avilable_standbyList.Clear();
            foreach (GameObject standbyPoint in standbyList)
                avilable_standbyList.Add(standbyPoint);

            /* allocate robotic wall for primary target       */
            Allocate_primary(vWalls[targetNum - 1]);

            /* allocate robotic wall for sub target */
            FindOtherTarget(targetNum);

            /* for left robotic walls, find standby points for them */
            foreach (Robotic_Wall rwall in standby_rwalls)
                Allocate_standby(rwall);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //besides the ML target, find targets for other robotic walls based on deteting block
    private void FindOtherTarget(int MLtarget)
    {
        
        List<GameObject> WallsInArea = detectBlock.GetWallInAreaList()/*new List<GameObject>()*/;
        /*foreach (GameObject wall in detectBlock.GetWallInAreaList())
            WallsInArea.Add(wall);*/
        //WallsInArea.Reverse();
        //print($"amount of detected walls = {WallsInArea.Count}");
        foreach (GameObject wall in WallsInArea)
        {
            if (wall != vWalls[targetNum - 1].gameObject)
            {
                //print($"other target = {wall.name}");
                Allocate_sub(wall.GetComponent<VirtualWall>());
            }
        }
    }
    /* allocate standy point for standby robotic walls */
    private void Allocate_standby(Robotic_Wall standby_rwall)
    {
        avilable_standbyList.Sort(delegate (GameObject point1, GameObject point2) {
            if (DistanceToVirWall(point1, standby_rwall.gameObject) < DistanceToVirWall(point2, standby_rwall.gameObject))
                return -1;
            else
                return 1;
        });
        standby_rwall.stateController.GetBehaviour<Wall_State>().SetTargetWall(avilable_standbyList[0]);
        avilable_standbyList.Remove(avilable_standbyList[0]);
    }

    /* allocate robotic wall for sub target virtual surface  */
    private void Allocate_sub(VirtualWall sub_target)
    {
        standby_rwalls.Sort(delegate (Robotic_Wall rwall1, Robotic_Wall rwall2)
        {
            if (DistanceToVirWall(rwall1.gameObject, sub_target.gameObject.transform.GetChild(0).gameObject) < DistanceToVirWall(rwall2.gameObject, sub_target.gameObject.transform.GetChild(0).gameObject))
                return -1;
            else
                return 1;
        }
            );
        if (standby_rwalls.Count > 0)
        {
            //print($"{sub_target.name} matched by {standby_rwalls[0].name} ");
            sub_target.SetMatchRWall(standby_rwalls[0].stateController);
            
            standby_rwalls.Remove(standby_rwalls[0]);
        }
    }

    /* allocate robotic wall for primary target virtual surface  */
    private void Allocate_primary(VirtualWall primary_target)
    {
        if (lastVirWallNum != targetNum)
        {
            List<Robotic_Wall> rwalls_list = new List<Robotic_Wall>();
            foreach (Robotic_Wall rwall in rWalls)
                rwalls_list.Add(rwall);
            //sort the list of robotic walls according to the distance from primary target virtual wall, smaller one ahead 
            rwalls_list.Sort(delegate (Robotic_Wall rwall1, Robotic_Wall rwall2)
            {
                if (DistanceToVirWall(rwall1.gameObject, primary_target.gameObject.transform.GetChild(0).gameObject) < DistanceToVirWall(rwall2.gameObject, primary_target.gameObject.transform.GetChild(0).gameObject))
                    return -1;
                else
                    return 1;
            });
            primary_rwalls_lastFrame = rwalls_list[0];
            primary_target.SetMatchRWall(rwalls_list[0].stateController);
            //print("did ************");
            if (standby_rwalls.Contains(rwalls_list[0]))
                standby_rwalls.Remove(rwalls_list[0]);
        }
        else
        {
            primary_target.SetMatchRWall(primary_rwalls_lastFrame.stateController);
            //print("did ************");
            if (standby_rwalls.Contains(primary_rwalls_lastFrame))
                standby_rwalls.Remove(primary_rwalls_lastFrame);
        }
        lastVirWallNum = targetNum;
    }


}
