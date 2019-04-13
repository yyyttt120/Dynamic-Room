using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attach onto Elevator_Enter_Indicato object under elevator
public class Elevator_Enter_Indicator : MonoBehaviour {
    private GameObject indicator;
    private FSMSystem statesController;
    private List<Animator> statesList;
    private bool entered = false;
    // Use this for initialization
    void Start ()
    {
        statesController = GameObject.Find("StatesController").GetComponent<FSMSystem>();
        statesList = statesController.GetStatesList();

    }

	// Update is called once per frame
	void Update () {
        foreach (Animator states in statesList)
            states.SetBool("ElevatorUserEntered",entered);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag.CompareTo("Hero") == 0)
        {
            entered = true;
            print("user enter");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.CompareTo("Hero") == 0)
        {
            entered = false;
        }
    }

    public bool GetUserEntered()
    {
        return entered;
    }
}
