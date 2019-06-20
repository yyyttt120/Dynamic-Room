using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_State : StateMachineBehaviour {
    private FSMSystem fsmController;
    private GameObject targetWall;
    private Robotic_Wall roboticWall;
    private GameObject slider;
    private GameObject user;
    private Wall_Requester wall_requester;

    private bool readyToRelease = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        fsmController = GameObject.Find("StatesController").GetComponent<FSMSystem>();
        //if it's from door state, keep the target wall same, else get target wall by controller
        //if (!animator.GetBool("SliderEnterDoor"))
        //targetWall = fsmController.SendToWallState();
        roboticWall = animator.gameObject.GetComponent<Robotic_Wall>();
        slider = targetWall.transform.GetChild(0).gameObject;
        user = GameObject.Find("Camera (eye)").gameObject;
        wall_requester = GameObject.Find("User_Encounter_Area").GetComponent<Wall_Requester>();
        //wall_requester.SetWallSolved(targetWall);
        readyToRelease = false;
        //Debug.Log("wall enter");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        /*if (Judge_Direction(targetWall))
            slider.transform.position = new Vector3(user.transform.position.x, targetWall.transform.position.y, targetWall.transform.position.z);
        else
            slider.transform.position = new Vector3(targetWall.transform.position.x, targetWall.transform.position.y, user.transform.position.z);*/
        //************************
        try
        {
            slider = targetWall.transform.GetChild(0).gameObject;
        }
        catch(UnityException err)
        {
            slider = targetWall;
            Debug.Log(animator.name + err);
        }
        roboticWall.wallToTarget_controller.Set_Target(slider);
        roboticWall.wallToTarget_controller.Robot_Move_Switch(true);
        //Debug.Log("wall_state");
        try
        {
            if (/*wall_requester.GetReleasedWall() == targetWall ||*/readyToRelease || !slider.activeSelf || slider == null)
            //***********************
            {
                int counter = animator.GetInteger("NearWallCounter") - 2;
                if (counter < -51)
                {
                    counter = -51;

                    /*if (!animator.GetBool("SliderEnterDoor") && !animator.GetBool("SlideEnterElevator"))
                        wall_requester.ReleaseWall(targetWall);
                    else
                    {
                        Debug.Log("dont release" + targetWall.name);
                        wall_requester.SetWallSolved(targetWall);
                    }*/
                }
                animator.SetInteger("NearWallCounter", counter);
            }
        }
        catch(System.NullReferenceException err)
        {
            Debug.Log(animator.gameObject.name + err.Message);
        }
        //if wall is keeping matching the slider, set P1 to 80 to improve the ability of tracking trajactory
        //and inactive the obstacle avoidance module to keep the wall slide smoothly 
        /*if (roboticWall.wallToTarget_controller.Get_State())
        {
            roboticWall.wallToTarget_controller.SetP1(70);
            animator.gameObject.GetComponent<Obstacle_Avoid>().enabled = false;
        }
        Debug.Log("slider active: " + slider.activeSelf);*/
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        roboticWall.wallToTarget_controller.Robot_Move_Switch(false);
        //if next state is door state, keep the target wall in sloved list, else release the target wall
        //Debug.Log("slider enter ele =" + animator.GetBool("SlideEnterElevator"));
        //Debug.Log("wall exit");
        //if (!animator.GetBool("SliderEnterDoor") && !animator.GetBool("SlideEnterElevator"))
        //wall_requester.ReleaseWall(targetWall);
        /*else
        {
            //Debug.Log("dont release" + targetWall.name);
            wall_requester.SetWallSolved(targetWall);
        }*/

        //reset p1 and obstacle avoidance module
        /*roboticWall.wallToTarget_controller.SetP1(40);
        animator.gameObject.GetComponent<Obstacle_Avoid>().enabled = true;*/
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
