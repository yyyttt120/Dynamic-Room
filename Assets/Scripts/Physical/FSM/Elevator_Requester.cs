using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attached onto elevator
public class Elevator_Requester : MonoBehaviour {
    private FSMSystem statesController;
    private List<Animator> statesList;
    private Animator robotic_wall_states;
    private GameObject elevator;
    private GameObject user;
    private bool switcher = false;
    // Use this for initialization
    void Start () {
        statesController = GameObject.Find("StatesController").GetComponent<FSMSystem>();
        statesList = statesController.GetStatesList();
        user = GameObject.Find("Camera (eye)").gameObject;
        robotic_wall_states = null;
        elevator = null;

    }
	
	// Update is called once per frame
	void Update () {

        //show the "accessed" message
        transform.GetChild(3).gameObject.SetActive(switcher);
        //print("stateslist count =" + statesList.Count);
        /*if (statesList.Count > 0)
        {
            foreach (Animator states in statesList)
            {
                //set elevator for all robotic walls
                //print("elevator =" + elevator);
                //states.SetBool("SliderEnterElevator", true);
                if (elevator != null)
                    states.GetBehaviour<ElevatorState>().SetElevator(elevator);
                else
                    print("No elevator detected!");
                states.SetBool("ElevatorUnlock", switcher);//set the lock state of elevator
                //allocate other robotic walls to elevator walls
                if (states != robotic_wall_states)
                {
                    if (Judge_Side(this.transform.GetChild(1), states.gameObject) == Judge_Side(this.transform.GetChild(1), this.transform.GetChild(1).gameObject))
                    {
                        states.SetInteger("ElevatorPartID", 2);//right wall
                    }
                    else
                        states.SetInteger("ElevatorPartID", 3);//left wall
                }
            }
        }*/
    }

    public void AllocateElevator(Animator doorWall)
    {
        if (statesList.Count > 0)
        {
            print("stateslist count =" + statesList.Count);
            List<Animator> rights = new List<Animator>();
            List<Animator> lefts = new List<Animator>();
            foreach (Animator states in statesList)
            {
                //set elevator for all robotic walls
                //print("elevator =" + elevator);
                //states.SetBool("SliderEnterElevator", true);
                /*if (elevator != null)
                    states.GetBehaviour<ElevatorState>().SetElevator(elevator);
                else
                    print("No elevator detected!");*/
                states.SetBool("ElevatorUnlock", switcher);//set the lock state of elevator
                //allocate other robotic walls to elevator walls
                
                if (states != doorWall)
                {
                    if (Judge_Side(this.transform.GetChild(1), states.gameObject) == Judge_Side(this.transform.GetChild(1), this.transform.GetChild(1).gameObject))
                    {
                        rights.Add(states);
                        //states.SetInteger("ElevatorPartID", 2);//right wall
                    }
                    else
                        lefts.Add(states);
                    //states.SetInteger("ElevatorPartID", 3);//left wall
                }
            }
                //decide which is right and which is left
                /*print("right =" + rights.Count);
                print("left =" + lefts.Count);
                if (rights.Count > 1)
                {
                    Animator s = rights[1];
                    rights.RemoveAt(1);
                    lefts.Add(s);
                }
                if (lefts.Count > 1)
                {
                    Animator s = lefts[1];
                    lefts.RemoveAt(1);
                    rights.Add(s);
                }
                rights[0].SetInteger("ElevatorPartID", 2);
                lefts[0].SetInteger("ElevatorPartID", 3);*/

            
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //when the silider enter elevator, elevator state start
        if (other.tag.CompareTo("Slider") == 0)
        {
            //print("slider enter:" + other.gameObject.name);
            GameObject wall = other.gameObject.transform.parent.gameObject;
            foreach (Animator states in statesList)
            {
                //set elevator object for all animators
                elevator = this.gameObject;
                if (elevator != null)
                    states.GetBehaviour<ElevatorState>().SetElevator(elevator);
                else
                    print("No elevator detected!");
                //states.SetBool("SliderEnterElevator", true);
                if (states.GetCurrentAnimatorStateInfo(0).IsName("Wall"))
                //find out which robotic wall should be the elevator door
                {
                    //print("slider enter elevator");
                    if (states.GetBehaviour<Wall_State>().GetWall() == wall)
                    {
                        robotic_wall_states = states;
                        elevator = this.gameObject;
                        //print("elevator = " + elevator);
                        states.SetInteger("ElevatorPartID", 1);
                        //states.GetBehaviour<DoorFrameState>().SetSlider(other.gameObject);

                    }
                }
            }
            //AllocateElevator();

        }
    }

    public void SetSwitch(bool sw)
    {
        switcher = sw;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.CompareTo("Slider") == 0)
        {
            //print("slider out");
            foreach (Animator states in statesList)
            {
                states.SetBool("SliderEnterElevator", true);
                //print("slider out");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //after the slider leave elevator, elevator state is over
        
        if (other.tag.CompareTo("Slider") == 0)
        {
            //print("slider out");
            foreach (Animator states in statesList)
            {
                states.SetBool("SliderEnterElevator", false);
                //print("slider out");
            }
        }
    }

    //Judge the direction the wall is in
    //if it's in x direction, return true
    //if it's in z direction, return false
    private bool Judge_Direction(Transform wall)
    {
        float angle = AngleSigned(transform.right, Vector3.right, Vector3.up);
        if (Mathf.Abs(angle) > 85 && Mathf.Abs(angle) < 105)
            return false;
        else
            return true;

    }

    //Judge which side of the user the obj is on, the direction is decided by dir
    //if it's on positive side, return true;
    //if it's on negtive side, return false
    private bool Judge_Side(Transform dir,GameObject obj)
    {
        if (Judge_Direction(dir))
        {
            if (obj.transform.position.z > user.transform.position.z)
                return true;
            else
                return false;
        }
        else
        {
            if (obj.transform.position.x > user.transform.position.x)
                return true;
            else
                return false;
        }
    }

    public float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)//return the angle between two vectors
    {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }
}
