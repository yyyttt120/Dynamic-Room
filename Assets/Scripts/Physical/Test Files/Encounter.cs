using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter : MonoBehaviour {
    public GameObject stand_by_points;
    public GameObject user;
    public GameObject wall_obj_x;
    public GameObject wall_obj_z;
    public GameObject wall_slider_x;
    public GameObject wall_slider_z;

    private Wall_To_Target wall_x;
    private Wall_To_Target wall_z;
    private GameObject target_x;
    private GameObject target_z;
    private GameObject next_stand_by_point_x;
    private GameObject next_stand_by_point_z;

    private int count_x = 0;
    private int count_z = 0;
    private bool counter_x = false;
    private bool counter_z = false;

    private bool start = false;
    private bool switch_encounter = false;
	// Use this for initialization
	void Start () {
        //initialize the 2 robotic walls
        wall_x = wall_obj_x.GetComponent<Wall_To_Target>();
        wall_z = wall_obj_z.GetComponent<Wall_To_Target>();
        target_x = stand_by_points.gameObject.transform.GetChild(3).gameObject;
        target_z = stand_by_points.gameObject.transform.GetChild(2).gameObject;
        next_stand_by_point_x = stand_by_points.gameObject.transform.GetChild(3).gameObject;
        next_stand_by_point_z = stand_by_points.gameObject.transform.GetChild(2).gameObject;
        //wall_x.Set_Target(this.gameObject/*.transform.GetChild(0).gameObject*/);
        //wall_z.Set_Target(this.gameObject/*.transform.GetChild(1).gameObject*/);
    }

    // Update is called once per frame
    void Update() {
        //set range for the 2 counter

        print("encounter =" + switch_encounter);
        count_x = SetRange(count_x, 200, -200);
        count_z = SetRange(count_z, 200, -200);
        if (switch_encounter)
        {
            wall_x.Set_Target(target_x);
            wall_z.Set_Target(target_z);
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            start = !start;
            switch_encounter = true;
            wall_x.Robot_Move_Switch(start);
            wall_z.Robot_Move_Switch(start);
        }
        wall_slider_x.transform.position = new Vector3(user.transform.position.x, wall_slider_x.transform.position.y, wall_slider_x.transform.position.z);
        wall_slider_z.transform.position = new Vector3(wall_slider_z.transform.position.x, wall_slider_z.transform.position.y, user.transform.position.z);
        //print("start = " + start);

        //print("count_x = " + count_x);
        //print("count_z = " + count_z);
        count_x = Count(count_x, counter_x);
        count_z = Count(count_z, counter_z);

        //change the state of the robotic wall based on counter
        
        
            if (count_x >= 200)
            {
                target_x = wall_slider_x;
                wall_x.controll.SetErrDistance(0.1f);
            }
            else if (count_x <= -200)
            {
                target_x = next_stand_by_point_x;
                wall_x.controll.SetErrDistance(0.15f);
            }

            if (count_z >= 200)
            {
                target_z = wall_slider_z;
                wall_z.controll.SetErrDistance(0.1f);
            }
            else if (count_z <= -200)
            {
                target_z = next_stand_by_point_z;
                wall_z.controll.SetErrDistance(0.15f);
            }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.CompareTo("Wall") == 0)
        {
            //print("triggered");
            if (Judge_Direction(other.gameObject))
            {
                counter_x = true;
                wall_slider_x.transform.position = new Vector3(wall_slider_x.transform.position.x, wall_slider_x.transform.position.y, other.transform.position.z);
                wall_slider_x.transform.forward = other.transform.forward;
                next_stand_by_point_z = Find_Next_Standby_Point(other.gameObject);
                //target_x = wall_slider_x;
                //print("x slide on");
            }
            else
            {
                counter_z = true;
                wall_slider_z.transform.position = new Vector3(other.transform.position.x, wall_slider_z.transform.position.y, wall_slider_z.transform.position.z);
                wall_slider_z.transform.forward = other.transform.forward;
                next_stand_by_point_x = Find_Next_Standby_Point(other.gameObject);
                //target_z = wall_slider_z;
            }
            //target_wall = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.CompareTo("Wall") == 0)
        {
            //print("out trigger");
            //target_wall = next_stand_by_point;
            if (Judge_Direction(other.gameObject))
                counter_x = false;
            else if(!Judge_Direction(other.gameObject))
                counter_z = false;
        }

    }
    //Judge the direction the wall is in
    //if it's in x direction, return true
    //if it's in z direction, return false
    private bool Judge_Direction(GameObject wall)
    {
        float angle = AngleSigned(wall.transform.right, Vector3.right, Vector3.up);
        if (Mathf.Abs(angle) > 85 && Mathf.Abs(angle) < 105)
            return false;
        else
            return true;
            
    }

    //Judge which side of the user the wall is on
    //if it's on positive side, return true;
    //if it's on negtive side, return false
    private bool Judge_Side(GameObject wall)
    {
        if (Judge_Direction(wall))
        {
            if (wall.transform.position.z > user.transform.position.z)
                return true;
            else
                return false;
        }
        else
        {
            if (wall.transform.position.x > user.transform.position.x)
                return true;
            else
                return false;
        }
    }

    public float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)//return the angle between two vectors
    {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }

    //find the position of next stand by point, which is always on the other side of the user to the wall
    private GameObject Find_Next_Standby_Point(GameObject wall)
    {
        bool direction = Judge_Direction(wall);
        bool side = Judge_Side(wall);
        if (direction && !side)
            return stand_by_points.transform.GetChild(0).gameObject;
        if (direction && side)
            return stand_by_points.transform.GetChild(3).gameObject;
        if (!direction && !side)
            return stand_by_points.transform.GetChild(2).gameObject;
        if (!direction && side)
            return stand_by_points.transform.GetChild(1).gameObject;
        else
            return stand_by_points.transform.GetChild(0).gameObject;
    }

    //
    private int Count(int count, bool counter)
    {
        if (counter)
            count += 2;
        else
            count -= 1;
        return count;
    }

    private int SetRange(int number, int max,int min)
    {
        if (number > max)
            number = max;
        if (number < min)
            number = min;
        return number;
    }

    public void Switch_Encounter(bool switch_encoun)
    {
        switch_encounter = switch_encoun;
    }
}
