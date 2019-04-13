using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User_Follower_Avoidance : MonoBehaviour {
    GameObject user;
	// Use this for initialization
	void Start () {
        user = GameObject.Find("Camera (eye)");
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(user.transform.position.x, 0f, user.transform.position.z);
	}
}
