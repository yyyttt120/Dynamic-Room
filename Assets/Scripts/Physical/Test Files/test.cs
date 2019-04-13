using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {
    private Wall_To_Target mover;
    private bool start;
    public GameObject target;
    SteamVR_TrackedObject tracker;
    SteamVR_Controller.Device device;
    // Use this for initialization
    void Start () {
        mover = GetComponent <Wall_To_Target> ();
        start = false;
        mover.Set_Target(target);
        tracker = gameObject.transform.parent.GetComponent<SteamVR_TrackedObject>();
        device = SteamVR_Controller.Input((int)tracker.index);
    }
	
	// Update is called once per frame
	void Update () {
        //print("mover =" + mover.name);
        mover.Set_Target(target);
        //print("anglevel ="+device.angularVelocity);
        print("speed =" + device.velocity.magnitude*1000);
        /*if (Input.GetKeyUp(KeyCode.K))
        {
            start = !start;
            mover.Robot_Move_Switch(start);
        }*/
    }
}
