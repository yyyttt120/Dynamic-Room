using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Avoidance : MonoBehaviour {
    private Robotic_Wall rWall;
    public GameObject roboWall;
    public GameObject target;

    private bool start = false;
	// Use this for initialization
	void Start () {
        rWall = new Robotic_Wall();
        rWall.Set_Robotic_Wall(roboWall);
	}
	
	// Update is called once per frame
	void Update () {
        rWall.wallToTarget_controller.Robot_Move_Switch(true);
        rWall.wallToTarget_controller.Set_Target(target);
	}
}
