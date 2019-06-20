using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButton : MonoBehaviour {
    private Material red;
    public Material green;
    public string buttonName;
    public bool button = false;
    private FSMSystem statesController;
    private List<Animator> statesList;
    // Use this for initialization
    void Start () {
        statesController = GameObject.Find("StatesController").GetComponent<FSMSystem>();
        statesList = statesController.GetStatesList();
        red = GetComponent<Renderer>().material;
    }
	
	// Update is called once per frame
	void Update () {
        foreach (Animator states in statesList)
            states.SetBool(buttonName, button);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag.CompareTo("Hand") == 0)
        {
            button = true;
            GetComponent<Renderer>().material = green;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.CompareTo("Hand") == 0)
        {
            button = false;
            GetComponent<Renderer>().material = red;
        }
    }

    public bool GetButton()
    {
        return button;
    }
}
