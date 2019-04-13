using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {
    private Wall_To_Target mover;
    private bool start;
    public GameObject target;
	// Use this for initialization
	void Start () {
        mover = GetComponent < Wall_To_Target> ();
        start = false;
        mover.Set_Target(target);
    }
	
	// Update is called once per frame
	void Update () {
        //print("mover =" + mover.name);
        mover.Set_Target(target);
        /*if (Input.GetKeyUp(KeyCode.K))
        {
            start = !start;
            mover.Robot_Move_Switch(start);
        }*/
    }
}
