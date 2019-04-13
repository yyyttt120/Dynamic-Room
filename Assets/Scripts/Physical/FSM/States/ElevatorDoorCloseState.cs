using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoorCloseState : ElevatorState {
    private Robotic_Wall robotic_wall = new Robotic_Wall();
    private float counter;
    private GameObject eleva;
    private GameObject door;
    private SceneSwitchTest sceneSwitcher;
    private int timer = 0;
    private bool timer_flag;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        robotic_wall.Set_Robotic_Wall(animator.gameObject);
        sceneSwitcher = GameObject.Find("StatesController").GetComponent<SceneSwitchTest>();
        counter = 0;
        eleva = animator.GetBehaviour<ElevatorState>().GetElevator();
        door = eleva.transform.GetChild(0).gameObject;
        //reversal the door to make it face to the user
        //door.transform.eulerAngles = door.transform.eulerAngles - new Vector3(0, 180, 0);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        eleva = animator.GetBehaviour<ElevatorState>().GetElevator();
        door = eleva.transform.GetChild(0).gameObject;
        robotic_wall.wallToTarget_controller.Robot_Move_Switch(true);
        robotic_wall.wallToTarget_controller.Set_Target(eleva.transform.GetChild(0).gameObject);



        //if (AngleSigned(animator.gameObject.transform.forward, door.transform.forward, Vector3.up) > -10 && AngleSigned(animator.gameObject.transform.forward, door.transform.forward, Vector3.up) < 10)
        //{
            if (counter < 0.8)
            {
                door.gameObject.transform.position = door.gameObject.transform.position + eleva.transform.right * 0.008f;
                counter = counter + 0.008f;
                Debug.Log("close door");
            }
            else
              if (robotic_wall.wallToTarget_controller.Get_State())
                animator.SetBool("ElevatorDoorClosed", true);
        //}
        //else
            //robotic_wall.roomba_controller.Rotation(door.transform.forward,animator.gameObject, robotic_wall.wallToTarget_controller.wallnum, 12, 100, true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool("ElevatorDoorClosed", false);
        if(animator.GetBool("ElevatorUserEntered"))
            sceneSwitcher.NextScene();
    }

    public float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)//return the angle between two vectors
    {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
