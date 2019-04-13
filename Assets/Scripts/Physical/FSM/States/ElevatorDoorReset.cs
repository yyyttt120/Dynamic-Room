using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoorReset : StateMachineBehaviour {

    private GameObject eleva;
    private GameObject door;
    private float counter;
    private Wall_Requester wall_requester;
    private GameObject targetWall;// the wall the elevator settled in
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        eleva = animator.GetBehaviour<ElevatorState>().GetElevator();
        door = eleva.transform.GetChild(0).gameObject;
        counter = 0;
        door.gameObject.transform.position = door.gameObject.transform.position - eleva.transform.right * 0.8f;
        wall_requester = GameObject.Find("User_Encounter_Area").GetComponent<Wall_Requester>();
        targetWall = eleva.transform.GetChild(4).gameObject;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        /*if (counter < 0.8)
        {
            door.gameObject.transform.position = door.gameObject.transform.position - elevator.transform.right * 0.008f;
            counter = counter + 0.008f;
            Debug.Log("close door");
        }
        else
            animator.SetBool("ElevatorOut", true);*/
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool("ElevatorOut", false);
        door.transform.position = new Vector3(eleva.transform.position.x, door.transform.position.y, door.transform.position.z);
        wall_requester.ReleaseWall(targetWall);
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
