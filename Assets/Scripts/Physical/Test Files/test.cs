using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {
    private RVO_agent mover;
    private bool start;
    public GameObject target;
    public SteamVR_TrackedObject tracker;
    SteamVR_Controller.Device device;
    // Use this for initialization
    void Start () {
        mover = GetComponent <RVO_agent> ();
        start = false;
        mover.target = target;
        //tracker = gameObject.transform.parent.GetComponent<SteamVR_TrackedObject>();
    }
	
	// Update is called once per frame
	void Update () {
        device = SteamVR_Controller.Input((int)tracker.index);
        //print("mover =" + mover.name);
        mover.target = target;
        //print("anglevel ="+device.angularVelocity);
        //print("speed =" + device.velocity.magnitude*1000);
        /*if (Input.GetKeyUp(KeyCode.K))
        {
            start = !start;
            mover.Robot_Move_Switch(start);
        }*/
    }
}
