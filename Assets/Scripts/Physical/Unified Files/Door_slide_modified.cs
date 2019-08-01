using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_slide_modified : MonoBehaviour
{
    public enum direction { Left, Right };
    public direction door_direction = direction.Right;

    private Vector3 closedposition;
    private bool over = false;
    private float length = 1.2f;
    private float offset = 0;
    private Vector3 target;//the next position for the virtual door
    private GameObject doorframe;
    // Start is called before the first frame update
    void Start()
    {
        closedposition = transform.localPosition;
        doorframe = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (over)
        {
            //if (ViveInput.GetPress(HandRole.LeftHand, ControllerButton.Trigger))
            //{
            if (door_direction == direction.Right)
            {
                if (closedposition.x - transform.localPosition.x <= 0.01f && closedposition.x - transform.localPosition.x >= -length + 0.01 - 1)
                {
                    transform.localPosition = target;
                    //print("open door");
                }
                //set the left boundry of the slide door
                /*if (closedposition.x - transform.localPosition.x < -length + 0.01)
                    transform.localPosition = closedposition + new Vector3(length, 0, 0);*/
                //set the right boundry of the slide door
                if (closedposition.x - transform.localPosition.x > 0.01f)
                    transform.localPosition = closedposition;
            }
            else
            {
                if (closedposition.x - transform.localPosition.x <= length && closedposition.x - transform.localPosition.x >= -0.01)
                {
                    transform.localPosition = target;
                    print("open door");
                }
                //set the left boundry of the slide door
                if (closedposition.x - transform.localPosition.x > length)
                    transform.localPosition = closedposition - new Vector3(length, 0, 0);
                //set the right boundry of the slide door
                if (closedposition.x - transform.localPosition.x < 0.01f)
                    transform.localPosition = closedposition;
            }
            //};
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.CompareTo("Hand") == 0)
        {
            print("get controller");
            GameObject controller = other.gameObject;
            Vector3 ctr_localPos = doorframe.transform.InverseTransformPoint(controller.transform.position);
            offset = ctr_localPos.x - transform.localPosition.x;
            over = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.tag.CompareTo("Hand") == 0)
        {
            GameObject controller = other.gameObject;
            print("hand enter ——" + gameObject.name);
            Vector3 ctr_localPos = doorframe.transform.InverseTransformPoint(controller.transform.position);
            target = new Vector3(ctr_localPos.x - offset, transform.localPosition.y, transform.localPosition.z);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.CompareTo("Hand") == 0)
            over = false;
    }
}
