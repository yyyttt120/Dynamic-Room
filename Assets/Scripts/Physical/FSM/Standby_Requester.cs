using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Standby_Requester : MonoBehaviour {
    public Wall_Requester wall_requester;
    public GameObject user;
    public enum algorithm { ml,detection};
    public algorithm currentAlgorithm = algorithm.detection;
    //public GameObject test;

    private List<GameObject> slovedWallList;
    private List<GameObject> standby_list;
    private List<GameObject> standby_list_available;
    private List<Animator> rWall_states_list;

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
        if(rWall_states_list == null)
        {
            rWall_states_list = GameObject.Find("StatesController").GetComponent<FSMSystem>().GetStatesList();
        }
	}

    //allocate a standby point to the robotic wall
    public GameObject Allocate_StandbyPoint(GameObject roboticWall)
    {
        //print(roboticWall.name);
        //slovedWallList = wall_requester.GetSolvedList();
        //sort with the distance from this robotic wall to standby points, the closet standby point shall be picked
        standby_list_available.Sort(delegate (GameObject point1, GameObject point2)
        {
            if (DistanceToVirWall(roboticWall, point1) < DistanceToVirWall(roboticWall, point2))
                return -1;
            else
                return 1;
        });
        /* sort the standby points list by the distance to matched virtual walls, from large to small */
        /*standby_list_available.Sort(delegate (GameObject point1, GameObject point2)
        {
            if (TotalDisToRWall(point1) > TotalDisToRWall(point2))
                return -1;
            else
                return 1;
        });
        foreach (GameObject standbyP in standby_list_available)
        {
            float totalDis = TotalDisToRWall(standbyP);
            print($"{standbyP.name} summary dis = {totalDis}");
        }*/
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
        /* from the top 2 points, pick one which is closer to the center of the room as the target standby point. */
        GameObject target;
        if (standby_list_available.Count < 2)
            target = standby_list_available[0];
        else
        {
            if (DistanceToVirWall(standby_list_available[0], center) < DistanceToVirWall(standby_list_available[1], center))
                target = standby_list_available[0];
            else
            {
                print("better stand-by point");
                target = standby_list_available[1];
            }
        }
        //print("allocate_standbypoint");*/
        //target = standby_list_available[0];
        standby_list_available.Remove(target);
        return target;
        
    }

    /* re-allocate the standy points at real time */
    /* the standby point is most far from matched virtual walls should be chosen */
    public GameObject Allocate_StandbyPoint(GameObject current_standbyPoint,GameObject rWall)
    {
        List<GameObject> temp_standby_list = new List<GameObject>();
        foreach (GameObject standbyPoint in standby_list_available)
            temp_standby_list.Add(standbyPoint);
        temp_standby_list.Add(current_standbyPoint);
        /* sort the list of standby point according to the totoal distance from matched virtual walls, farther one be ahead */
        temp_standby_list.Sort(delegate (GameObject point1, GameObject point2)
        {
            if (TotalDisToRWall(point1) > TotalDisToRWall(point2))
                return -1;
            else
                return 1;
        });
        /*foreach(GameObject standbyP in temp_standby_list)
        {
            float totalDis = TotalDisToRWall(standbyP);
            print($"{standbyP.name} summary dis = {totalDis}");
        }*/

        /* if the total distance of top 2 points are close, pick the one close to the robotic wall */
        float error = Mathf.Abs(TotalDisToRWall(temp_standby_list[0]) - TotalDisToRWall(temp_standby_list[1]));
        GameObject target;
        if (error < 0.1f && DistanceToVirWall(temp_standby_list[0],rWall) > DistanceToVirWall(temp_standby_list[1],rWall))
        {
            target = temp_standby_list[1];
        }
        else
            target = temp_standby_list[0];
        if (target == current_standbyPoint)
            return target;
        else
        {
            standby_list_available.Remove(target);
            standby_list_available.Add(current_standbyPoint);
            return target;
        }
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
    private float TotalDisToRWall(GameObject standbyPoint)
    {
        float totalDis = 0;
        foreach (Animator states in rWall_states_list)
        {
            if (states.GetCurrentAnimatorStateInfo(0).IsName("Wall"))
            {
                GameObject vwall = states.GetBehaviour<Wall_State>().GetWall();
                float dis_point2wall = dis_Point2VWall(standbyPoint.transform.position,vwall);
                totalDis += dis_point2wall;
            }
        }
        return totalDis;
    }

    //calculate the distance from a point to a virtual wall by raycast,return the result
    private float dis_Point2VWall(Vector3 point,GameObject vWall)
    {
        RaycastHit hit;
        LayerMask mask = 1 << 8;
        /* judge which side of the virtual wall the point is at */
        /*Vector3 ray_dir;
        Vector3 point2Wall = point - vWall.transform.position;
        point2Wall.y = 0;
        if (Vector3.Dot(point2Wall, vWall.transform.forward) > 0)
        {
            ray_dir = -vWall.transform.forward;
        }
        else
            ray_dir = vWall.transform.forward;
        Color color = Color.red;
        Debug.DrawRay(point, ray_dir.normalized * 2, color);
        if (Physics.Raycast(point, ray_dir, out hit, 10f,mask))
        {
            if (hit.collider.gameObject == vWall)
            {
                print("******************** hit *******************");
                return hit.distance;
            }
            else
                return hit.distance + 4f;
            
        }
        else
            return 0;*/
        Vector3 a = vWall.transform.position - point;
        a.y = 0;
        Vector3 b = vWall.transform.right;
        float cosTheta = (Vector3.Dot(a, b)) / (a.magnitude * b.magnitude);
        float sinTheta = Mathf.Sqrt(1 - cosTheta * cosTheta);
        float c = a.magnitude * sinTheta;
        return c;
        
    }

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
