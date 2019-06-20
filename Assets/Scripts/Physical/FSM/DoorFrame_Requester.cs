using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//attach on the door frame object
public class DoorFrame_Requester : MonoBehaviour
{
    private FSMSystem statesController;
    private Animator robotic_wall_states;
    private GameObject slider;
    // Use this for initialization
    void Start()
    {
        statesController = GameObject.Find("StatesController").GetComponent<FSMSystem>();
        robotic_wall_states = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(slider != null)
            if(!slider.activeSelf)
                robotic_wall_states.SetBool("SliderEnterDoor", false);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.CompareTo("Slider") == 0)
        {
            print("frame**************");
            slider = other.gameObject;
            //print("slider enter:" + other.gameObject.name);
            GameObject wall = other.gameObject.transform.parent.gameObject;
            robotic_wall_states = wall.GetComponent<VirtualWall>().GetMatchRWall();
            robotic_wall_states.SetBool("SliderEnterDoor", true);
            robotic_wall_states.GetBehaviour<DoorFrameState>().SetFrame(this.gameObject.transform.GetChild(0).gameObject);
            robotic_wall_states.GetBehaviour<DoorFrameState>().SetDoorWall(wall);
            //List<Animator> statesList = statesController.GetStatesList();

            /*foreach (Animator states in statesList)
            {
                if (states.GetCurrentAnimatorStateInfo(0).IsName("Wall"))
                    if (states.GetBehaviour<Wall_State>().GetWall() == wall)
                    {
                        print("door wall is " +wall.name);
                        robotic_wall_states = states;
                        states.SetBool("SliderEnterDoor", true);
                        states.GetBehaviour<DoorFrameState>().SetFrame(this.gameObject.transform.GetChild(0).gameObject);
                        states.GetBehaviour<DoorFrameState>().SetDoorWall(wall);
                        //states.GetBehaviour<DoorFrameState>().SetSlider(other.gameObject);
                    }
            }*/

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.CompareTo("Slider") == 0)
        {
            print("frame**************");
            slider = other.gameObject;
            //print("slider enter:" + other.gameObject.name);
            GameObject wall = other.gameObject.transform.parent.gameObject;
            robotic_wall_states = wall.GetComponent<VirtualWall>().GetMatchRWall();
            robotic_wall_states.SetBool("SliderEnterDoor", true);
            robotic_wall_states.GetBehaviour<DoorFrameState>().SetFrame(this.gameObject.transform.GetChild(0).gameObject);
            robotic_wall_states.GetBehaviour<DoorFrameState>().SetDoorWall(wall);
            /*List<Animator> statesList = statesController.GetStatesList();
            foreach (Animator states in statesList)
            {
                if (states.GetCurrentAnimatorStateInfo(0).IsName("Wall"))
                    if (states.GetBehaviour<Wall_State>().GetWall() == wall)
                    {
                        print("door wall is " + wall.name);
                        robotic_wall_states = states;
                        states.SetBool("SliderEnterDoor", true);
                        states.GetBehaviour<DoorFrameState>().SetFrame(this.gameObject.transform.GetChild(0).gameObject);
                        states.GetBehaviour<DoorFrameState>().SetDoorWall(wall);
                        //states.GetBehaviour<DoorFrameState>().SetSlider(other.gameObject);
                    }
            }*/

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.CompareTo("Slider") == 0)
        {
            robotic_wall_states.SetBool("SliderEnterDoor", false);
        }
    }
}

