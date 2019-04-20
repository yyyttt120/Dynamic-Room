﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attach onto virtual wall objects
public class Robotic_Wall_Requester : MonoBehaviour
{
    public FSMSystem statesController;
    public Wall_Requester solvedListController;// the user's reference

    private Animator matchRoboticWall;
    private List<Animator> statesList;

    private int waitCount = 0;

    private bool waitAllocate = true;
    private bool userClose = false;//true when user get close to the wall

    // Start is called before the first frame update
    void Start()
    {
        statesController = GameObject.Find("StatesController").GetComponent<FSMSystem>();
        solvedListController = GameObject.Find("User_Encounter_Area").GetComponent<Wall_Requester>();
        statesList = new List<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        List<Animator> list = new List<Animator>();
        list = statesController.GetStatesList();
        foreach (Animator ani in list)
        {
            if (!statesList.Contains(ani))
                statesList.Add(ani);
        }
        /*print( gameObject.name + " user closed = " + userClose);
        if (!userClose)
        {
            print(gameObject.name + " ***************user closed = " + userClose);
            matchRoboticWall.GetBehaviour<Wall_State>().SetReadyRelease(true);
            matchRoboticWall = null;
        }*/

        /*if (userClose)
        {
            if (gameObject.transform.GetChild(0).gameObject.activeSelf)
            {
                if (!solvedListController.GetSolvedList().Contains(this.gameObject) && gameObject.transform.GetChild(0).gameObject.activeSelf)
                {
                    print("wall should ready " + gameObject.name);
                    Animator states = null;
                    //if (waitAllocate)
                    {
                        //waitAllocate = false;
                        states = Allocate_wall(gameObject);
                    }
                    if (states != null)
                        print("robot wall =" + states.name);
                    matchRoboticWall = states;
                    int counter = states.GetInteger("NearWallCounter") + 4;
                    if (counter >= 70)
                    {
                        states.GetBehaviour<Wall_State>().SetTargetWall(this.gameObject);
                        counter = 70;
                    }
                    states.SetInteger("NearWallCounter", counter);
                }
            }
        }
        else
        {
            matchRoboticWall.GetBehaviour<Wall_State>().SetReadyRelease(true);
            waitAllocate = true;
            waitCount = 0;
        }*/

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag.CompareTo("User_Detect_Area") == 0)
        {
            //print("wall in:" + gameObject.name);
            //make sure the encountered wall woulden't be released
            //if (releasedWall == other.gameObject)
            //releasedWall = null;
            //check if this wall isn't covered by other walls
            //Slider_Controller slider_Con = gameObject.GetComponent<Slider_Controller>();
            userClose = true;
            if (gameObject.transform.GetChild(0).gameObject.activeSelf)
            {
                if (!solvedListController.GetSolvedList().Contains(this.gameObject) && gameObject.transform.GetChild(0).gameObject.activeSelf)
                {
                    userClose = true;
                    //print("wall should ready " + gameObject.name);
                    Animator states = null;
                    //if (waitAllocate)
                    {
                        //waitAllocate = false;
                        states = Allocate_wall(gameObject);
                    }
                    if (states != null)
                    {
                        //states.SetBool("AllocateRW", true);
                        //print("robot wall =" + states.name);
                        matchRoboticWall = states;
                        int counter = states.GetInteger("NearWallCounter") + 1;
                        if (counter >= 30)
                        {
                            userClose = true;
                            solvedListController.SetWallSolved(this.gameObject);
                            states.GetBehaviour<Wall_State>().SetTargetWall(this.gameObject);
                            counter = 30;
                        }
                        states.SetInteger("NearWallCounter", counter);
                    }
                    //print(gameObject.name + "need a RW");
                }
            }


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.CompareTo("User_Detect_Area") == 0)
        {
            //matchRoboticWall.SetBool("AllocateRW", false);
            solvedListController.ReleaseWall(this.gameObject);
            userClose = false;
            //matchRoboticWall.GetBehaviour<Wall_State>().SetReadyRelease(true);
            matchRoboticWall.GetBehaviour<Wall_State>().SetReadyRelease(true);
            waitAllocate = true;
            waitCount = 0;
        }
    }

    public Animator Allocate_wall(GameObject targetWall)
    {
        string distanceList = "distance =";
        Animator result = null;
        GameObject slider = gameObject.transform.GetChild(0).gameObject;
        // sort the robotic walls based on the distance from target virtual wall
        statesList.Sort(delegate (Animator phyW1, Animator phyW2)
        {
            if (DistanceToVirWall(slider, phyW1.gameObject) > DistanceToVirWall(slider, phyW2.gameObject))
                return 1;
            else
                return -1;
        });
        //StartCoroutine(Wait2Frame());
        //if (waitCount > 5)
        foreach (Animator ani in statesList)
        {
            distanceList += ani.gameObject.name + DistanceToVirWall(slider, ani.gameObject) + " ";
        }
        //print(distanceList);
        foreach (Animator ani in statesList)
        {
            if (ani.GetCurrentAnimatorStateInfo(0).IsName("Standby"))
            {
                //...do transform from stand_by to wall condition
                /* int counter = states.GetInteger("NearWallCounter") + 2;
                 if (counter > 100)
                     counter = 100;
                 states.SetInteger("NearWallCounter", counter);*/
                //wall = targetWall;
                //Transform_StandbyToWall(states, wall);
                //print("find robotic wall");
                //return ani;
                //print("***************");
                result = ani;
                break;
            }
            else
                print(ani.name + " is not stand by");
        }
        /*for(int i=0;i<3;i++)
        {
            if (statesList[i].GetCurrentAnimatorStateInfo(0).IsName("Standby"))
            {

                return statesList[i];
            }
        }*/

        waitCount++;
        if (result == null)
            Debug.Log("no robotic wall is in standby state");
        return result;
    }

    public Animator GetMatchRoboWall()
    {
        return matchRoboticWall;
    }

    private float DistanceToVirWall(GameObject virWall, GameObject phyWall)
    {
        Vector3 vec = virWall.transform.position - phyWall.transform.position;
        vec = new Vector3(vec.x, 0, vec.z);
        return vec.magnitude;
    }

    IEnumerator Wait2Frame()
    {
        yield return 2;
        /*foreach (Animator states in statesList)
        {
            if (states.GetCurrentAnimatorStateInfo(0).IsName("Standby"))
            {
                    
                matchRoboticWall = states;
            }
        }*/
    }
}
