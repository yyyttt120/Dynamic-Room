using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using HTC.UnityPlugin.Vive;

public class Door_slide_Right : MonoBehaviour {
    public enum direction {Left, Right};
    public direction door_direction = direction.Right;

    RoombaControllerScript c2 = null;
    RoombaFeedback_Test RoombaMotionController = null;

    //public GameObject wall;
    private GameObject controller;

    private Vector3 closedposition;
    private Vector3 positionlastframe;
    private Vector3 positionnow;
    private float speed;//the speed of the controller in x axix
    private float offset;
    private float length;
    private bool over;//when it's true, update the position of the virtual slide door
    private bool start = false;// start the motion of dynamci wall
    private Collider doorlegth;

    private Vector3 target;//the next position for the virtual door
    private Vector3 target_wall;//the target of the dynamic wall
    // Use this for initialization
    void Start () {
        //doorlegth = GetComponent<Collider>();
        //c2 = GameObject.Find("Roomba").GetComponent<RoombaControllerScript>();
        //RoombaMotionController = GameObject.Find("Rooba_Test").GetComponent<RoombaFeedback_Test>();
        closedposition = transform.position;
        positionlastframe = Vector3.zero;
        length = 0.8f;//doorlegth.bounds.size.x;//length of the door
        controller = null;
    }
	
	// Update is called once per frame
	void Update () {

        positionnow = controller.transform.position;
        speed = positionnow.x - positionlastframe.x;
        positionlastframe = positionnow;
        //print("length =" + length);
        if (Input.GetKeyUp(KeyCode.L))
            start = !start;
        /*if (start)
        {
            target_wall = new Vector3(transform.position.x, wall.transform.position.y, wall.transform.position.z);
            RoombaMotionController.Translation_LR(target_wall, wall, 0, true, 0, 800);
        }*/
        //print("over =" + over);
        print("over =" + over);
        if (over)
        {
            //if (ViveInput.GetPress(HandRole.LeftHand, ControllerButton.Trigger))
            //{
            float o = closedposition.x - transform.position.x;
            if (door_direction == direction.Right)
            {
                if (closedposition.x - transform.position.x <= 0.01f && closedposition.x - transform.position.x >= -length + 0.01)
                {
                    transform.position = target;
                    print("open door");
                }
                //set the left boundry of the slide door
                if (closedposition.x - transform.position.x < -length + 0.01)
                    transform.position = closedposition + new Vector3(length, 0, 0);
                //set the right boundry of the slide door
                if (closedposition.x - transform.position.x > 0.01f)
                    transform.position = closedposition;
            }
            else
            {
                if (closedposition.x - transform.position.x <= length - 0.01 && closedposition.x - transform.position.x >= -0.01)
                {
                    transform.position = target;
                    print("open door");
                }
                //set the left boundry of the slide door
                if (closedposition.x - transform.position.x > length - 0.01)
                    transform.position = closedposition - new Vector3(length, 0, 0);
                //set the right boundry of the slide door
                if (closedposition.x - transform.position.x < 0.01f)
                    transform.position = closedposition;
            }
            //};
        }
        /*else
            c2.Stop();*/
       // if (Mathf.Abs(wall.transform.position.x - transform.position.x) < 0.01)
         //   c2.Stop();
        /*if (closedposition.x - transform.position.x < -length)
            transform.position = new Vector3(closedposition.x + length,closedposition.y,transform.position.z);
        if(closedposition.x - transform.position.x > 0)
            transform.position = closedposition;*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.CompareTo("Hand") == 0)
        {
            print("get controller");
            controller = other.gameObject;
            offset = controller.transform.position.x - transform.position.x;
            over = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.tag.CompareTo("Hand") == 0)
        {
            controller = other.gameObject;
            print("hand enter ——" + gameObject.name);
            target = new Vector3(controller.transform.position.x - offset, transform.position.y, transform.position.z);
            //print("target =" + target);
        }
            /*if (Mathf.Abs(speed) > 0.0007f && Mathf.Abs(speed) < 0.01)
        {
            over = true;
        }*/
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.CompareTo("Hand") == 0)
            over = false;
    }
}
