using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User_follower : MonoBehaviour {
    public GameObject user;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(user.transform.position.x, transform.position.y, user.transform.position.z);
	}
}
