using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Single_Wall_Straight_V2 : MonoBehaviour {
    public GameObject user;
    public GameObject straight_wall;
    RoombaControllerScript c2 = null;
    public RoombaFeedback_Test controll;

    private bool start;
    // Use this for initialization

     
    void Start () {
        c2 = GameObject.Find("Roomba").GetComponent<RoombaControllerScript>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.K))
            start = !start;
        if (start)
        {
            if (user.transform.position.x > transform.position.x)
                c2.Move(300, 300, 0);
            else
                c2.Move(-300, 300, 0);
            if (Mathf.Abs(transform.position.x - user.transform.position.x) < 0.1)
            {
                c2.Stop();
                controll.Rotation(straight_wall.transform.forward, this.gameObject, 0, 12, 200, true);
            }
        }
    }
}
