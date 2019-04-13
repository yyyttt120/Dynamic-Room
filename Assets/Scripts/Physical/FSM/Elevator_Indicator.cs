using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator_Indicator : MonoBehaviour {
    public Material dark;
    public Material green;
    private bool elevatorInitiated;
    private FSMSystem statesController;
    private List<Animator> statesList;
    // Use this for initialization
    void Start () {
        statesController = GameObject.Find("StatesController").GetComponent<FSMSystem>();
        statesList = statesController.GetStatesList();
    }
	
	// Update is called once per frame
	void Update () {
        elevatorInitiated = true;
        foreach (Animator states in statesList)
        {
            //print(states.gameObject.name + "elevatorInitiated =" + elevatorInitiated);
            //print(states.gameObject.name + states.GetBool("ElevatorDoorInitiated"));
            elevatorInitiated = elevatorInitiated & states.GetBool("ElevatorDoorInitiated");
            //print(states.gameObject.name + "NelevatorInitiated =" + elevatorInitiated);
        }

        //print("elevatorInitiated =" + elevatorInitiated);
        if(elevatorInitiated)
            GetComponent<Renderer>().material = green;
        else
            GetComponent<Renderer>().material = dark;
    }
}
