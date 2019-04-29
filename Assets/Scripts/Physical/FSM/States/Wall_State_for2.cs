using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_State : StateMachineBehaviour {
    private FSMSystem fsmController;
    private GameObject targetWall;
    private Robotic_Wall roboticWall = new Robotic_Wall();
    private GameObject slider;
    private GameObject user;
    private Wall_Requester wall_requester;

    private bool readyToRelease = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        fsmController = GameObject.Find("StatesController").GetComponent<FSMSystem>();
        roboticWall.Set_Robotic_Wall(animator.gameObject);
        slider = targetWall.transform.GetChild(0).gameObject;
        user = GameObject.Find("Camera (eye)").gameObject;
        wall_requester = GameObject.Find("User_Encounter_Area").GetComponent<Wall_Requester>();
        wall_requester.SetWallSolved(targetWall);
        readyToRelease = false;
        //Debug.Log("wall enter");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //************************
        if(/*wall_requester.GetReleasedWall() == targetWall ||*/readyToRelease|| !slider.activeSelf || slider == null)
        //***********************
        {
            int counter = animator.GetInteger("NearWallCounter") - 2;
            if (counter < -31)
            {
                counter = -31;

                if (!animator.GetBool("SliderEnterDoor") && !animator.GetBool("SlideEnterElevator"))
                    wall_requester.ReleaseWall(targetWall);
                else
                {
                    Debug.Log("dont release" + targetWall.name);
                    wall_requester.SetWallSolved(targetWall);
                }
            }
            animator.SetInteger("NearWallCounter", counter);
        }
        roboticWall.wallToTarget_controller.Set_Target(slider);
        roboticWall.wallToTarget_controller.Robot_Move_Switch(true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        roboticWall.wallToTarget_controller.Robot_Move_Switch(false);
        if (!animator.GetBool("SliderEnterDoor") && !animator.GetBool("SlideEnterElevator"))
        wall_requester.ReleaseWall(targetWall);
        else
        {
            wall_requester.SetWallSolved(targetWall);
        }

    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    private bool Judge_Direction(GameObject wall)
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

    public void SetTargetWall(GameObject wall_)
    {
        this.targetWall = wall_;
    }

    public GameObject GetWall()
    {
        return targetWall;
    }

    public void SetReadyRelease(bool readyornot)
    {
        readyToRelease = readyornot;
    }
}
