﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorFrameState : StateMachineBehaviour {
    private GameObject frame;
    //private GameObject slider;
    //private GameObject user;
    private Robotic_Wall roboticWall;
    //private Wall_Requester wall_requester;
    private GameObject doorWall;//the door where the wall settle
    private SteamVR_TrackedObject vive_controller;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        roboticWall=animator.gameObject.GetComponent<Robotic_Wall>();
        Debug.Log("doorwall =" + doorWall.name);
        //wall_requester.SetWallSolved(doorWall);
        vive_controller = GameObject.Find("Controller (right)").GetComponent<SteamVR_TrackedObject>();
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        /*if (Judge_Direction(slider))
            slider.transform.position = new Vector3(user.transform.position.x, slider.transform.position.y, slider.transform.position.z);
        else
            slider.transform.position = new Vector3(slider.transform.position.x, slider.transform.position.y, user.transform.position.z);*/
        //Debug.Log("doorwall =" + doorWall.name);
        //ensure this robotic wall is in solved list
        //wall_requester.SetWallSolved(doorWall);
        //disable obstacle avoidance module
        //animator.gameObject.GetComponent<Obstacle_Avoid>().enabled = false;
        Debug.Log($"{frame.name} is door frame");
        roboticWall.wallToTarget_controller.Robot_Move_Switch(true);
        doorWall.GetComponent<VirtualWall>().SetMatchRWall(animator);
        doorWall.transform.GetChild(0).position = frame.transform.position;
        /*if (roboticWall.wallToTarget_controller.Get_State())
        {
            animator.SetBool("DoorInitiated", true);
            
        }*/
        Debug.Log("door =" + frame.transform.GetChild(0).gameObject.name);

        SteamVR_Controller.Device right = SteamVR_Controller.Input((int)vive_controller.index);
        if(right.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
            animator.SetBool("SliderEnterDoor", false);
        //if the slider of door wall inactivated, transform into standby state
        /*if (!doorWall.transform.GetChild(0).gameObject.activeSelf)
            animator.SetBool("SliderEnterDoor", false);*/

        //send the paramenter to next state
        //animator.GetBehaviour<DoorState>().SetDoor(frame.transform.GetChild(0).gameObject);
        //animator.GetBehaviour<DoorState>().SetSlider(slider);
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //roboticWall.wallToTarget_controller.Robot_Move_Switch(false);
        //reset obstacle avoidance module
        //animator.gameObject.GetComponent<Obstacle_Avoid>().enabled = true;
        //release the wall the door settle in
        //wall_requester.ReleaseWall(animator.GetBehaviour<Wall_State>().GetWall());
        //animGetBehaviator.our<DoorState>().SetDoor(frame.transform.GetChild(0).gameObject);
    }

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

    public void SetFrame(GameObject fra)
    {
        frame = fra;
    }

    public void SetDoorWall(GameObject doorw)
    {
        doorWall = doorw;
    }

    /*private bool Judge_Direction(GameObject wall)
    {
        float angle = AngleSigned(wall.transform.right, Vector3.right, Vector3.up);
        if (Mathf.Abs(angle) > 85 && Mathf.Abs(angle) < 105)
            return false;
        else
            return true;

    }

    public float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)//return the angle between two vectors
    {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }

    public void SetSlider(GameObject obj)
    {
        slider = obj;
    } */
}
