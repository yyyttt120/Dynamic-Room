using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Single_Wall_Straight_Way : MonoBehaviour {
    public GameObject user;
    public GameObject straight_wall;
    public Wall_To_Target wall_controllor;
    private SteamVR_TrackedObject tracked_obj;
    private GameObject wall;
	// Use this for initialization
	void Start () {
        wall = wall_controllor.gameObject;
        tracked_obj = user.GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update () {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)tracked_obj.index);
        //print("velocity.x =" + device.velocity.x);
        //if (device.velocity.x > 0.2)
        transform.position = new Vector3(user.transform.position.x, transform.position.y, straight_wall.transform.position.z);
        /*if (device.velocity.x <  -0.2)
            transform.position = new Vector3(user.transform.position.x - 3, transform.position.y, straight_wall.transform.position.z);

        if (Mathf.Abs(wall.transform.position.x - user.transform.position.x) < 0.1)
            wall_controllor.Robot_Translate_Switch(false);*/
    }
}
