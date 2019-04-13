using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Standby_State : StateMachineBehaviour {

    private Standby_Requester standby_requester;
    private GameObject standby_point;
    private Robotic_Wall roboticWall = new Robotic_Wall();
    private int counter;
    private bool count;
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        standby_requester = GameObject.Find("Stand_by").GetComponent<Standby_Requester>();
        roboticWall.Set_Robotic_Wall(animator.gameObject);
        standby_point = standby_requester.Allocate_StandbyPoint(roboticWall.robotic_wall);
        roboticWall.roomba_controller.SetErrDistance(0.15f);
        counter = 200;
        count = true;
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //Debug.Log(animator.gameObject.name +":" +standby_point.gameObject.name);
        /*if (counter < 0)
        {
            //counter = 200;
            count = false;
            counter = 200;
            //Debug.Log("allocate");
            standby_point = standby_requester.Allocate_StandbyPoint(roboticWall.robotic_wall);
        }*/
        
        roboticWall.wallToTarget_controller.Set_Target(standby_point);
        roboticWall.wallToTarget_controller.Robot_Move_Switch(true);
        if(count)
            counter -= 1;
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        roboticWall.wallToTarget_controller.Robot_Move_Switch(false);
        standby_requester.Release_StandbyPoint(standby_point);
        roboticWall.roomba_controller.SetErrDistance(0.1f);
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
