using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//attach on the door frame object
public class Door_Requester : MonoBehaviour {
    //private float timer;
    //private bool start;//when it's true, start to initiate the door state
    // Use this for initialization
    public enum direction {left,right};
    public direction openDirection = direction.right;
    private bool start_vel_con = false;
    private GameObject doorFrame;
    private float openDistance; 
    void Start () {
        doorFrame = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 displacement = transform.position - doorFrame.transform.position;
        openDistance = Vector3.Dot(displacement, transform.right);
        //if(openDistance > -0.05f && openDistance < 0.05f)

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.CompareTo("Hand") == 0)
        {
            if (openDirection == direction.right)
            {
                if (openDistance > 0.8f || openDistance < 0)
                    return;
            }
            else
            {
                if (openDistance < -0.8f || openDistance > 0)
                    return;
            }
            start_vel_con = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.CompareTo("Hand") == 0)
        {
            start_vel_con = false;
        }
    }

    public bool GetStartVelCon()
    {
        return start_vel_con;
    }


}

