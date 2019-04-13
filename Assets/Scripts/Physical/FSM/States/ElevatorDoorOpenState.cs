using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoorOpenState : ElevatorState {
    private Robotic_Wall robotic_wall = new Robotic_Wall();
    private float counter;
    private GameObject eleva;
    private GameObject door;
    private AudioSource elevatorArriveVoice;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        robotic_wall.Set_Robotic_Wall(animator.gameObject);
        counter = 0;
        animator.SetBool("ElevatorDoorInitiated", false);
        animator.SetBool("ElevatorDoorClosed", false);
        elevatorArriveVoice = animator.gameObject.transform.GetChild(4).GetComponent<AudioSource>();
        elevatorArriveVoice.Play();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        eleva = animator.GetBehaviour<ElevatorState>().GetElevator();
        door = eleva.transform.GetChild(0).gameObject;
        robotic_wall.wallToTarget_controller.Robot_Move_Switch(true);
        robotic_wall.wallToTarget_controller.Set_Target(eleva.transform.GetChild(0).gameObject);
        if (counter < 0.8)
        {
            door.gameObject.transform.position = door.gameObject.transform.position + eleva.transform.right * -0.008f;
            counter = counter + 0.008f;
        }
        else
        {
            animator.SetBool("ElevatorDoorOpened", true);
            animator.SetInteger("CloseCounter", animator.GetInteger("CloseCounter")-1);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        robotic_wall.wallToTarget_controller.Robot_Move_Switch(false);
        animator.SetBool("ElevatorDoorOpened", false);
        animator.SetInteger("CloseCounter", 500);
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
