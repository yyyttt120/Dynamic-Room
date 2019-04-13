using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Slide : MonoBehaviour {
    public Wall_To_Target rob_wall;
    public GameObject slider;

    private bool start = false;
    private bool state_door = false;
	// Use this for initialization
	void Start () {
        rob_wall.Set_Target(slider);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.K))
        {
            start = !start;
            rob_wall.Robot_Move_Switch(start);
        }
        if (!state_door)
        {
            slider.transform.position = this.transform.position;
            slider.transform.rotation = this.transform.rotation;
        }
        else
        {
            slider.transform.position = new Vector3(slider.transform.position.x, slider.transform.position.y, rob_wall.gameObject.transform.position.z);
        }
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.CompareTo("Robotic_Wall") == 0)
        {
            state_door = true;
        }
    }
}
