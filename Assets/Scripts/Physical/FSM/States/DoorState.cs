using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using HTC.UnityPlugin.Vive;

public class DoorState : StateMachineBehaviour {
    private GameObject door;
    //private GameObject slider;
    //private GameObject user;
    private SteamVR_TrackedObject tracker;
    private SteamVR_TrackedObject controller;
    private Robotic_Wall robotic_wall = new Robotic_Wall();

    private bool start_vel_con = false;

    private Vector3 door_dir;
    private Vector3 door_origin_pos;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        /*//keep the slider following user
        if (Judge_Direction(slider))
            slider.transform.position = new Vector3(user.transform.position.x, slider.transform.position.y, slider.transform.position.z);
        else
            slider.transform.position = new Vector3(slider.transform.position.x, slider.transform.position.y, user.transform.position.z);*/

        animator.SetBool("DoorInitiated", false);
        robotic_wall.Set_Robotic_Wall(animator.gameObject);
        tracker = animator.gameObject.transform.parent.gameObject.GetComponent<SteamVR_TrackedObject>();
        controller = GameObject.Find("tracker_hand_R").gameObject.GetComponent<SteamVR_TrackedObject>();
        //user = GameObject.Find("Camera (eye)").gameObject;
        door_dir = door.transform.forward;       
        door_origin_pos = door.transform.position;
        //Debug.Log("controller =" /*+ controller.name*/);
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Debug.Log("door_Dir = " + door_dir);
        //controller = GameObject.Find("Controller (right)").gameObject.GetComponent<SteamVR_TrackedObject>();
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)controller.index);
        //Debug.Log("door xxx =" + door.name);
        start_vel_con = door.GetComponent<Door_Requester>().GetStartVelCon();
            //when the user try to open the door with controller, start velocity controlling 
        if (start_vel_con /*&& ViveInput.GetPress(HandRole.RightHand, ControllerButton.Trigger)*/)
        {
            Debug.Log("slide door");
            //connect the position of the door to robotic wall
            door.transform.position = new Vector3(animator.transform.position.x , door.transform.position.y, animator.transform.position.z);
            robotic_wall.roomba_controller_vel.Translation_Velocity(device.velocity, animator.gameObject, tracker, 0, 0, 800);
        }
            //when the contrller is draw back, calibarate the direction of the robotic wall
        else
            robotic_wall.roomba_controller.Rotation(door_dir, animator.gameObject, 0, 12, 100, true);
        
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
    public void SetDoor(GameObject doo)
    {
        door = doo;
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
    }*/
}
