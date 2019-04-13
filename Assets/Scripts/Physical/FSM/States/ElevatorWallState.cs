using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorWallState : ElevatorState {
    private Robotic_Wall robotic_wall = new Robotic_Wall();
    private GameObject eleva;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        robotic_wall.Set_Robotic_Wall(animator.gameObject);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        eleva = animator.GetBehaviour<ElevatorState>().GetElevator();
        robotic_wall.wallToTarget_controller.Robot_Move_Switch(true);
        /*if (animator.GetInteger("ElevatorPartID") == 2)
            //*********************** always is left wall ******************
            robotic_wall.wallToTarget_controller.Set_Target(eleva.transform.GetChild(2).gameObject);
        else if (animator.GetInteger("ElevatorPartID") == 3)
            robotic_wall.wallToTarget_controller.Set_Target(eleva.transform.GetChild(2).gameObject);
        else
            Debug.Log("err: is not in door wall states");*/
        robotic_wall.wallToTarget_controller.Set_Target(eleva.transform.GetChild(2).gameObject);
        if (robotic_wall.wallToTarget_controller.Get_State())
        {
            animator.SetBool("ElevatorDoorInitiated", true);

        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        robotic_wall.wallToTarget_controller.Robot_Move_Switch(false);
        animator.SetBool("ElevatorDoorInitiated", false);
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
