using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorFrameState : StateMachineBehaviour {
    private GameObject frame;
    //private GameObject slider;
    //private GameObject user;
    private Robotic_Wall roboticWall = new Robotic_Wall();
    private Wall_Requester wall_requester;
    private GameObject doorWall;//the door where the wall settle
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        roboticWall.Set_Robotic_Wall(animator.gameObject);
        wall_requester = GameObject.Find("User_Encounter_Area").GetComponent<Wall_Requester>();
        //user = GameObject.Find("Camera (eye)").gameObject;
        //roboticWall.wallToTarget_controller.Robot_Move_Switch(true);
        //doorWall = GameObject.Find("DoorWall");
        Debug.Log("doorwall =" + doorWall.name);
        wall_requester.SetWallSolved(doorWall);
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        /*if (Judge_Direction(slider))
            slider.transform.position = new Vector3(user.transform.position.x, slider.transform.position.y, slider.transform.position.z);
        else
            slider.transform.position = new Vector3(slider.transform.position.x, slider.transform.position.y, user.transform.position.z);*/
        Debug.Log("doorwall =" + doorWall.name);
        //ensure this robotic wall is in solved list
        wall_requester.SetWallSolved(doorWall);
        //disable obstacle avoidance module
        animator.gameObject.GetComponent<Obstacle_Avoid>().enabled = false;
        roboticWall.wallToTarget_controller.Robot_Move_Switch(true);
        roboticWall.wallToTarget_controller.Set_Target(frame);
        if (roboticWall.wallToTarget_controller.Get_State())
        {
            animator.SetBool("DoorInitiated", true);
            
        }
        Debug.Log("door =" + frame.transform.GetChild(0).gameObject.name);

        //if the slider of door wall inactivated, transform into standby state
        if (!doorWall.transform.GetChild(0).gameObject.activeSelf)
            animator.SetBool("SliderEnterDoor", false);

        //send the paramenter to next state
        animator.GetBehaviour<DoorState>().SetDoor(frame.transform.GetChild(0).gameObject);
        //animator.GetBehaviour<DoorState>().SetSlider(slider);
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        roboticWall.wallToTarget_controller.Robot_Move_Switch(false);
        //reset obstacle avoidance module
        animator.gameObject.GetComponent<Obstacle_Avoid>().enabled = true;
        //release the wall the door settle in
        wall_requester.ReleaseWall(animator.GetBehaviour<Wall_State>().GetWall());
        //animator.GetBehaviour<DoorState>().SetDoor(frame.transform.GetChild(0).gameObject);
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
