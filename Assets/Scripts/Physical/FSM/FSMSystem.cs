using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMSystem : MonoBehaviour {
    private List<Animator> statesList = new List<Animator>();

    public GameObject r_wall1;
    public GameObject r_wall2;
    public GameObject r_wall3;
    public GameObject r_wall4;

    private GameObject wall;//virtual wall wait be allocated a robotic wall
                            // Use this for initialization

    private void Awake()
    {
        Application.targetFrameRate = 50;
    }
    void Start () {
        //AddState(r_wall1.GetComponent<FSMState>());
        //AddState(r_wall2.GetComponent<FSMState>());
        if (r_wall1.activeSelf)
            statesList.Add(r_wall1.GetComponent<Animator>());
        if(r_wall2.activeSelf)
            statesList.Add(r_wall2.GetComponent<Animator>());
        if (r_wall3.activeSelf)
            statesList.Add(r_wall3.GetComponent<Animator>());
        /*if (r_wall4.activeSelf)
            statesList.Add(r_wall4.GetComponent<Animator>());*/
    }

    // Update is called once per frame
    void Update() {
        /*foreach(Animator states in statesList)
        {
            MinusCounter("NearWallCounter", states);
        }*/
	}

    /*private void AddState(FSMState states)
    {
        
        if (statesList == null)
        {
            Debug.LogError("FSM ERROR");
        }

        
        if (statesList.Count == 0)
        {
            statesList.Add(states);
            return;
        }

        
        if (!statesList.Contains(states))
        {
            statesList.Add(states);
        }

    }*/

    //answer the request from Wall_requester and allocate robotic walls to this request

    /*public Animator Allocate_wall(GameObject targetWall,List<Animator> speStateList)
    {
        string distanceList = "distance =";
        speStateList = new List<Animator>();
        foreach(Animator ani in statesList)
        {
            if (!speStateList.Contains(ani))
                speStateList.Add(ani);
        }
        foreach (Animator ani in speStateList)
        {
            distanceList += ani.gameObject.name + DistanceToVirWall(targetWall, ani.gameObject)+ " ";
        }
        print(distanceList);
        // sort the robotic walls based on the distance from target virtual wall
        speStateList.Sort(delegate (Animator phyW1, Animator phyW2)
        {
            if (DistanceToVirWall(targetWall, phyW1.gameObject) > DistanceToVirWall(targetWall, phyW2.gameObject))
                return 1;
            else
                return -1;
        });
        foreach( Animator states in speStateList)
        {
            if (states.GetCurrentAnimatorStateInfo(0).IsName("Standby"))
            {
                //...do transform from stand_by to wall condition
               /* int counter = states.GetInteger("NearWallCounter") + 2;
                if (counter > 100)
                    counter = 100;
                states.SetInteger("NearWallCounter", counter);*/
    /* wall = targetWall;
     //Transform_StandbyToWall(states, wall);
     return states;
 }
}
Debug.Log("no robotic wall is in standby state");
return null;
}*/

    public List<Animator> GetStatesList()
    {
        return statesList;
    }

    public Animator FreeWall(GameObject wall)
    {
        foreach(Animator states in statesList)
        {
            if(states.GetCurrentAnimatorStateInfo(0).IsName("Wall"))
            {
                if(states.GetBehaviour<Wall_State>().GetWall() == wall)
                {
                    //transform from wall to standby condition
                    //states.SetBool("NearWall", false);
                    return states;
                }
            }
        }
        Debug.Log("no robotic wall is encountering target wall");
        return null;
    }

    public GameObject SendToWallState()
    {
        return wall;
    }

    private float DistanceToVirWall(GameObject virWall, GameObject phyWall)
    {
        Vector3 vec = virWall.transform.position - phyWall.transform.position;
        vec = new Vector3(vec.x, 0, vec.z);
        return vec.magnitude;
    }

    private void MinusCounter(string name,Animator states)
    {
        int counter = states.GetInteger(name) - 1;
        if (counter < -100)
            counter = -100;
        states.SetInteger(name, counter);
    }

    //****************** transform functions *************************
    private void Transform_StandbyToWall(FSMState states, GameObject wall)
    {
        states.SetCurState(State.stand_by);
        Destroy(states.gameObject.GetComponent<StandbyState>());
        states.gameObject.AddComponent<WallState>();
        WallState wallstate = states.gameObject.GetComponent<WallState>();
        wallstate.SetWall(wall);        
    }

}
