using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robotic_Wall: MonoBehaviour
{

    // Use this for initialization
    public GameObject robotic_wall;
    public Wall_To_Target wallToTarget_controller;
    public RoombaFeedback_Test roomba_controller;
    public RoombaFeedback_Velocity roomba_controller_vel;
    public Animator stateController;

    private void Start()
    {
        Set_Robotic_Wall(this.gameObject);
    }

    public bool Set_Robotic_Wall(GameObject r_wall)
    {
        bool allSet = true;
        this.robotic_wall = r_wall;
        if (r_wall.GetComponent<Wall_To_Target>() != null)
            this.wallToTarget_controller = r_wall.GetComponent<Wall_To_Target>();
        else
            allSet = false;

        if (r_wall.GetComponent<Animator>() != null)
            this.roomba_controller_vel = r_wall.GetComponent<RoombaFeedback_Velocity>();
        else
            allSet = false;

        if (r_wall.GetComponent<RoombaFeedback_Test>() != null)
            this.roomba_controller = r_wall.GetComponent<RoombaFeedback_Test>();
        else
            allSet = false;

        if (r_wall.GetComponent<RoombaFeedback_Velocity>() != null)
            this.roomba_controller_vel = r_wall.GetComponent<RoombaFeedback_Velocity>();
        else
            allSet = false;

        return allSet;
    }
}
