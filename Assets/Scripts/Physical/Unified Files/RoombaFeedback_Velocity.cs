using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaFeedback_Velocity : RoombaFeedback_Test {
    private RoombaControllerScript roomba_basic_controller = null;
    public float err_velocity;// acceptable error of the velocity
	// Use this for initialization
	void Start () {
        roomba_basic_controller = GameObject.Find("Roomba").GetComponent<RoombaControllerScript>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //PID controlling based on velocity
    public bool Translation_Velocity(Vector3 target_Velocity,GameObject wall, SteamVR_TrackedObject tracker,int wallnum,float p1,float p2)//moving to target position with feedback controlling
    {
        Debug.Log("start vel controlling");
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)tracker.index);
        bool finished = true;
        float vel_err;
        //float angle;
        int velocity_R;
        int velocity_L;
        float vel_err_input;
        //float angle_input;
        //float p_ang = p1;//controlling variable of angle
        float p_dis = p2;//controlling variable of distance
        //float angleDirection;// the angle between walltoTarget and the forward direction of the wall
        //int movingdirection;
        //angle
        target_Velocity = new Vector3(target_Velocity.x, 0, target_Velocity.z);
        Vector3 wall_Velocity = device.velocity;
        Vector3 wall_dir = -wall.transform.forward;
        //get the projecture of the velocity of wall and target on the moving direction of the robotic wall
        float wall_vel_proj = Vector3.Dot(wall_Velocity,wall_dir);
        float target_vel_proj = Vector3.Dot(target_Velocity, wall_dir);
        //if(wall_Velocity.magnitude > 0.05)
        //wall_Velocity = new Vector3(wall_Velocity.x, 0, wall_Velocity.z);
        //else
        //wall_Velocity = new Vector3(wall.transform.right.x, 0, wall.transform.right.z);
        //angleDirection = AngleSigned(wall_Velocity, target_Velocity, Vector3.up);
        /*if(angleDirection > -90 && angleDirection < 90)
            angle = AngleSigned(wall_Velocity, target_Velocity, Vector3.up);
        else
            angle = AngleSigned(-wall_Velocity, target_Velocity, Vector3.up);
        angle_input = p_ang * angle;*/
        //velocity discrimination
        vel_err = 1.3f * target_vel_proj - wall_vel_proj;
        /*if (angleDirection > -90 && angleDirection < 90)//when the moving direction is away from the target make the speed be minus
            movingdirection = 1;
        else
            movingdirection = -1;*/
        //print("vel_err = " + vel_err);
        vel_err_input = p_dis * vel_err;
        if (vel_err_input > 500)//amplitude limiting
        {
            vel_err_input = 500;
        }
        if (vel_err_input < -500)
            vel_err_input = -500;
        /*if (angle_input > 500)
        {
            angle_input = 500;
        }
        if (angle_input < -500)
            angle_input = -500;*/
        velocity_R = (int)(vel_err_input);
        velocity_L = (int)(vel_err_input);

        if (velocity_R > 500)//amplitude limiting
        {
            velocity_R = 500;
        }
        if (velocity_R < -500)
            velocity_R = -500;
        if (velocity_L > 500)
        {
            velocity_L = 500;
        }
        if (velocity_L < -500)
            velocity_L = -500;

        //print("vel_input = " + velocity_R);
        if (Mathf.Abs(vel_err_input) <= err_velocity)
        {
            roomba_basic_controller.Stop(wallnum);
            finished = true;

        }
        else
        {

                //print("velocity R =" + velocity_R);
                //print("velocity L =" + velocity_L);
            roomba_basic_controller.Move(velocity_R, velocity_L, wallnum);
            finished = false;

        }
        return finished;
    }

    public void Stop(int num)
    {
        roomba_basic_controller.Stop(num);
    }

    public bool Rotation(Vector3 targetpos,GameObject wall,int wallnum,float p,float d)
    {
        bool a = base.Rotation(targetpos, wall, wallnum, p, d, true);
        return a;
    }
}
