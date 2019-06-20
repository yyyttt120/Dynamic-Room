using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Standby_Requester : MonoBehaviour {
    public Wall_Requester wall_requester;
    public GameObject user;
    //public GameObject test;

    private List<GameObject> slovedWallList;
    private List<GameObject> standby_list;
    private List<GameObject> standby_list_available;

    private GameObject center;//the center of the virtual room
    //private FSMSystem statesController;
	// Use this for initialization
	void Start () {
        slovedWallList = new List<GameObject>();
        standby_list = new List<GameObject>();
        standby_list_available = new List<GameObject>();
        //add all the standby points into the list as a reference
        for (int i = 0; i < 4; i++)
        {
            standby_list.Add(transform.GetChild(i).gameObject);
        }
        //add all available standby points into the list
        for (int i = 0; i < 4; i++)
        {
            standby_list_available.Add(transform.GetChild(i).gameObject);
        }
        //print(standby_list_available[0].name);
        //statesController = GameObject.Find("StatesController").GetComponent<FSMSystem>();
        center = GameObject.Find("Center");
    }
	
	// Update is called once per frame
	void Update () {
        /*if (Judge_Side(test, transform.GetChild(0).gameObject) == Judge_Side(test, transform.GetChild(2).gameObject))
            print("same side");
        else
            print("different side");*/
            
	}

    //allocate a standby point to the robotic wall
    public GameObject Allocate_StandbyPoint(GameObject roboticWall)
    {
        //print(roboticWall.name);
        //sort the standby points by the distance between robotic wall and standby point, from small to large
        foreach (GameObject stand in standby_list_available)
        {
            float dis = DistanceToVirWall(roboticWall, stand);
            //print(stand.name + "distance =" + dis);
        }
        slovedWallList = wall_requester.GetSolvedList();
        //sort with the distance from this robotic wall to standby points, the closet standby point shall be picked
        standby_list_available.Sort(delegate (GameObject phyW1, GameObject phyW2)
        {
            if (DistanceToVirWall(roboticWall, phyW1) < DistanceToVirWall(roboticWall, phyW2))
                return -1;
            else
                return 1;
        });

        //sort with the total distance from other robotic wall in wall state to standby points, the standby point with largest total distance shall be picked
        /*standby_list_available.Sort(delegate (GameObject stand1, GameObject stand2)
        {
            if (TotalDisToRWall(stand1,roboticWall) < TotalDisToRWall(stand2,roboticWall))
                return 1;
            else
                return -1;
        });*/
        /*List<GameObject> tempo_standby_list = standby_list_available;
        foreach(GameObject wall in slovedWallList)
        {
            print(wall.name);
            foreach(GameObject standbyPoint in tempo_standby_list)
            {
                if (Judge_Side(wall, wall) == Judge_Side(wall, standbyPoint))
                {
                    print("deleted point:" + standbyPoint.name);
                    tempo_standby_list.Remove(standbyPoint);
                }
            }
        }*/
        GameObject target;
        //if (standby_list_available.Count < 2)
            target = standby_list_available[0];
        /*else
        {
            if (DistanceToVirWall(standby_list_available[0], center) < DistanceToVirWall(standby_list_available[1], center))
                target = standby_list_available[0];
            else
            {
                print("better stand-by point");
                target = standby_list_available[1];
            }
        }*/
        //print("allocate_standbypoint");
        standby_list_available.Remove(target);
        return target;
        
    }

    //release the standby point and move it into the available list
    public void Release_StandbyPoint(GameObject standbyPoint)
    {
        if (standby_list_available.Contains(standbyPoint))
            Debug.Log("err: target standby point didn't be delete correctly:" + standbyPoint.name);
        else
            standby_list_available.Add(standbyPoint);
    }

    //return
    /*private float TotalDisToRWall(GameObject standbyPoint,GameObject rWall)
    {
        float totalDis = 0;
        foreach (Animator states in statesController.GetStatesList())
        {
            if (states.GetCurrentAnimatorStateInfo(0).IsName("Wall") || states.gameObject == rWall)
            {
                Vector3 vec = states.transform.position - standbyPoint.transform.position;
                vec.y = 0;
                totalDis += vec.magnitude;
            }
        }
        return totalDis;
    }*/

    private float DistanceToVirWall(GameObject virWall, GameObject phyWall)
    {
        Vector3 vec = virWall.transform.position - phyWall.transform.position;
        vec = new Vector3(vec.x, 0, vec.z);
        return vec.magnitude;
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

    //Judge which side of the user the obj is on, and the direction judged by the direction object
    //if it's on positive side, return true;
    //if it's on negtive side, return false
    private bool Judge_Side(GameObject direction,GameObject obj)
    {
        if (Judge_Direction(direction))
        {
            if (obj.transform.position.z > user.transform.position.z)
                return true;
            else
                return false;
        }
        else
        {
            if (obj.transform.position.x > user.transform.position.x)
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

}
