using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virtual_Switch : MonoBehaviour {
    public bool virtualSwitch;

    private GameObject doorFrame;
    private GameObject elevator;
	// Use this for initialization
	void Start () {
        elevator = GameObject.Find("PTK_Elevator_2Floors");
	}
	
	// Update is called once per frame
	void Update () {
        if (GameObject.Find("DoorFrame") != null)
        {
            doorFrame = GameObject.Find("DoorFrame");
            VirtualDoorSwitch();
        }

       if(elevator != null)
        {
            VirtualElevatorSwitch();
        }

    }

    void VirtualDoorSwitch()
    {
        Door_slide_Right door = doorFrame.transform.GetChild(0).GetComponent<Door_slide_Right>();
        //door.enabled = virtualSwitch;
    }

    void VirtualElevatorSwitch()
    {
        VirtualElevator ele = elevator.GetComponent<VirtualElevator>();
        ele.enabled = virtualSwitch;
    }
}
