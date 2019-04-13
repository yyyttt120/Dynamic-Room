using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swithc_Elevator : MonoBehaviour {
    public enum quality { real,fake}
    public quality realorFake = quality.real;
    private int count;
    //*****************
    private bool switcher = true;
    //****************
    private GameObject message;
    private AudioSource accessed;
    private Elevator_Requester elevator_controller;
    // Use this for initialization
    void Start () {
        elevator_controller = GameObject.Find("PTK_Elevator_2Floors").GetComponent<Elevator_Requester>();
        if (realorFake == quality.real)
            accessed = transform.GetChild(1).GetComponent<AudioSource>();
        else
            accessed = transform.GetChild(2).GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        message = this.transform.GetChild(0).gameObject;
        //print("count_switch =" + count);
        if (count > 50)
        {
            switcher = true;
            //count = 200;
            accessed.Play();
        }
        else
            message.SetActive(false);
        ElevatorOn(switcher);
    }

    private void ElevatorOn(bool sw)
    {
        if (sw)
        {
    
                message.SetActive(true);
                if(realorFake == quality.real)
                    elevator_controller.SetSwitch(true);
            
        }
        else
        {
                message.SetActive(false);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag.CompareTo("Hand") == 0)
        {
            print("switch hand enter");
            count += 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.CompareTo("Hand") == 0)
        {
            count = 0;
        }
    }

    public bool GetSwitcher()
    {
        return switcher;
    }
}
