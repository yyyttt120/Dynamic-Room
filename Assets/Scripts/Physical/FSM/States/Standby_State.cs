using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Threading;

public class Standby_State : StateMachineBehaviour {

    //private Thread reallocate_standbyPoint;
    private Standby_Requester standby_requester;
    private GameObject standby_point;
    private Robotic_Wall roboticWall;
    private int counter;
    private bool count;
    private float timer;
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        standby_requester = GameObject.Find("Stand_by").GetComponent<Standby_Requester>();
        roboticWall = animator.gameObject.GetComponent<Robotic_Wall>();
        standby_point = standby_requester.Allocate_StandbyPoint(roboticWall.robotic_wall);
        roboticWall.roomba_controller.SetErrDistance(0.15f);
        counter = 200;
        count = true;
        timer = 0;
        //reallocate_standbyPoint = new Thread(re_Allocate_StandbyPoint);
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        //without rvo ******************
        //roboticWall.wallToTarget_controller.Set_Target(standby_point);

        //with rvo **********************
        try
        {
            roboticWall.rvoAgent.target = standby_point;
        }
        catch(System.NullReferenceException e1)
        {
            Debug.Log($"{this.name} {e1.Message}");
            standby_point = standby_requester.Allocate_StandbyPoint(roboticWall.robotic_wall);
        }

        roboticWall.wallToTarget_controller.Robot_Move_Switch(true);
        if(count)
            counter -= 1;

        timer += Time.deltaTime;
        if(timer > 1f)
        {
            if(standby_requester.currentAlgorithm == Standby_Requester.algorithm.detection)
                standby_point = standby_requester.Allocate_StandbyPoint(standby_point,1);
            timer = 0;
        }
        /*if(reallocate_standbyPoint.ThreadState == ThreadState.Unstarted)
            reallocate_standbyPoint.Start();*/
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        roboticWall.wallToTarget_controller.Robot_Move_Switch(false);
        standby_requester.Release_StandbyPoint(standby_point);
        roboticWall.roomba_controller.SetErrDistance(0.15f);
        //reallocate_standbyPoint.Suspend();
    }

    private void re_Allocate_StandbyPoint()
    {
        standby_point = standby_requester.Allocate_StandbyPoint(standby_point, 1);
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
