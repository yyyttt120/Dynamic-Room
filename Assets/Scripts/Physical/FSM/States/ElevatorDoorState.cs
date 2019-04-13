using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoorState : ElevatorState {

    private Robotic_Wall robotic_wall = new Robotic_Wall();
    private FSMSystem statesController;
    private List<Animator> statesList;
    private GameObject eleva;
    private bool elevatorInitiated = true;
    private int timer = 0;
    private Wall_Requester wall_requester;
    private Elevator_Requester ele_requester;
    GameObject ele_wall;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        robotic_wall.Set_Robotic_Wall(animator.gameObject);
        statesController = GameObject.Find("StatesController").GetComponent<FSMSystem>();
        statesList = statesController.GetStatesList();
        wall_requester = GameObject.Find("User_Encounter_Area").GetComponent<Wall_Requester>();
        //put the wall which the elevator setteled in to sloved List
        ele_wall = GameObject.Find("Wall_Elevator_True");
        eleva = animator.GetBehaviour<ElevatorState>().GetElevator();
        ele_requester = eleva.GetComponent<Elevator_Requester>();
        Debug.Log("eleva =" + ele_wall.name);
        wall_requester.SetWallSolved(ele_wall);
        ele_requester.AllocateElevator(animator);
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        wall_requester.SetWallSolved(ele_wall);
        eleva = animator.GetBehaviour<ElevatorState>().GetElevator();
        //Debug.Log("elevator in door state = " + eleva.name);
        List<GameObject> solvedlist = wall_requester.GetSolvedList();
        Debug.Log("is ele wall solved =" + solvedlist.Contains(ele_wall));
        elevatorInitiated = true;
        robotic_wall.wallToTarget_controller.Robot_Move_Switch(true);
        robotic_wall.wallToTarget_controller.Set_Target(eleva.transform.GetChild(0).gameObject);
        //since wall is over to move to target, this elevator door is initiated
        if (robotic_wall.wallToTarget_controller.Get_State())
        {
            animator.SetBool("ElevatorDoorInitiated", true);

        }
        foreach(Animator states in statesList)
        {
            elevatorInitiated = elevatorInitiated & states.GetBool("ElevatorDoorInitiated");
        }
        if(elevatorInitiated)
        {
            timer = timer + 1;
        }

        if (timer > 200)
        {
            animator.SetBool("ElevatorInitiated", elevatorInitiated);
            timer = 0;
        }

    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        robotic_wall.wallToTarget_controller.Robot_Move_Switch(false);
        animator.SetBool("ElevatorInitiated", false);
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
