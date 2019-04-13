using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//states of robotic walls
public enum State
{
    stand_by,
    door,
    wall,
    elevator
}
public class FSMState: MonoBehaviour
{

    private List<State> list = new List<State>();
    private State curState;
    private Robotic_Wall robotic_Wall;
    private GameObject target;

    private void Start()
    {
        //initiate this FSM of the robotic wall
        Addstate(State.stand_by);
        Addstate(State.door);
        Addstate(State.wall);
        Addstate(State.elevator);
        curState = State.stand_by;
        robotic_Wall = new Robotic_Wall();
        robotic_Wall.Set_Robotic_Wall(this.gameObject);

    }

    public void Addstate(State state)
    {
        if (list.Contains(state))
        {
            return;
        }

        list.Add(state);
    }

    public bool SetCurState(State state)
    {
        if (list.Contains(state))
        {
            curState = state;
            return true;
        }
        else
            return false;
    }

    public State GetCurState()
    {
        return this.curState;
    }

    public void SetTarget(GameObject targ)
    {
        target = targ;
    }
}

