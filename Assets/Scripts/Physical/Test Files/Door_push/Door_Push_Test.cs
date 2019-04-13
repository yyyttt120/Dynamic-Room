using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Push_Test : MonoBehaviour {
    public GameObject robotic_wall;
    public GameObject shaft;

    public float push_speed;

    private Robotic_Wall RWall;

    private bool start_door = false;
    private bool open_door = false;
    private bool pushing_door = false;
	// Use this for initialization
	void Start () {
        RWall = new Robotic_Wall();
        RWall.Set_Robotic_Wall(robotic_wall);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.K))
        {
            start_door = !start_door;
            RWall.wallToTarget_controller.Set_Target(this.gameObject);
            RWall.wallToTarget_controller.Robot_Move_Switch(start_door);
        }
        if (Input.GetKeyUp(KeyCode.I))
            open_door = !open_door;

        if (Input.GetKeyUp(KeyCode.O))
            pushing_door = !pushing_door;

        print("angle =" + this.transform.eulerAngles.y);
        if (pushing_door)
        {
            if (open_door)
            {
                if (this.transform.eulerAngles.y <= 270 && this.transform.eulerAngles.y >= 180)
                    this.transform.eulerAngles = new Vector3(transform.eulerAngles.x, 270, transform.eulerAngles.z);
                else
                    this.transform.RotateAround(shaft.transform.position, Vector3.up, -1*push_speed);
            }
            else
            {
                if (this.transform.eulerAngles.y >= 0 && this.transform.eulerAngles.y <= 90)
                    this.transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
                else
                    this.transform.RotateAround(shaft.transform.position, Vector3.up, push_speed);
            }
        }
	}
}
