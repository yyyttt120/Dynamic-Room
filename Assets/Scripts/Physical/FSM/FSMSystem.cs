using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RVO;
using Vector2 = RVO.Vector2;

public class FSMSystem : MonoBehaviour {
    private List<Animator> statesList = new List<Animator>();
    private float timer;
    private float timeStep = 0.04f;//time step in simulatin for RVO
    private GameObject[] virtualWalls;
    private VirtualWall[] vwalls;
    public float maxSpeed = 0.5f;//max speed of robotic walls

    public GameObject r_wall1;
    public GameObject r_wall2;
    public GameObject r_wall3;
    public GameObject r_wall4;

    //private GameObject wall;//virtual wall wait be allocated a robotic wall
                            // Use this for initialization

    private void Awake()
    {
        Application.targetFrameRate = 50;
        //add a rvo agent for user
    }
    void Start () {
        timer = 0;
        //default set for RVO agent
        Simulator.Instance.setTimeStep(timeStep);
        Simulator.Instance.setAgentDefaults(2.0f,4,5.0f,5.0f,0.5f,maxSpeed,new Vector2(0.0f,0.0f));
        Simulator.Instance.addAgent(new Vector2(0, 0));
        //prepare the robotic walls
        if (r_wall1.activeSelf)
            addRwall(r_wall1);
        if (r_wall2.activeSelf)
            addRwall(r_wall2);
        if (r_wall3.activeSelf)
            addRwall(r_wall3);
        if (r_wall4.activeSelf)
            addRwall(r_wall4);

        /* find all the virtual walls with tag */
        virtualWalls = GameObject.FindGameObjectsWithTag("Wall");
        vwalls = new VirtualWall[virtualWalls.Length];
        for(int i = 0;i< virtualWalls.Length; i++)
        {
            vwalls[i] = virtualWalls[i].GetComponent<VirtualWall>();
        }
    }

    //add the the robotic wall into robotic wall list,
    //add a rvo agent for this robotic wall and bind the sid to it's rvo_agent component
    private int addRwall(GameObject rwall)
    {
        statesList.Add(rwall.GetComponent<Animator>());
        int i = Simulator.Instance.addAgent(new Vector2(rwall.transform.position.x, rwall.transform.position.z));
        RVO_agent agent = rwall.GetComponent<RVO_agent>();
        if (agent != null && agent.enabled)
            agent.sid = i;
        else
            rwall.GetComponent<RVO_agent_Ideal>().sid = i;
        return i;
    }

    // Update is called once per frame
    void Update() {
        //start the simulation for rvo algorithm
        timer += Time.deltaTime;
        if (timer > timeStep)
        {
            Simulator.Instance.doStep();
            timer = 0;

            /*check if two virtual walls are asigned a same robotic wall*/
            for (int i = 0; i < vwalls.Length - 1; i++)
            {
                for (int j = i + 1; j < vwalls.Length; j++)
                {
                    if (vwalls[i].GetMatchRWall() == null || vwalls[j].GetMatchRWall() == null)
                        continue;
                    else if (vwalls[i].GetMatchRWall() == vwalls[j].GetMatchRWall())
                        vwalls[j].SetMatchRWall(null);

                }
            }
        }

        
	}

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

    /*public GameObject SendToWallState()
    {
        return wall;
    }*/

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
