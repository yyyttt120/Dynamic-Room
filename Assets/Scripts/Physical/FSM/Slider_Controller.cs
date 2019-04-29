using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attached onto a wall
public class Slider_Controller : MonoBehaviour {
    public enum wallType { flat, curve};
    public wallType thisWallType = wallType.flat;
    private GameObject slider;
    private GameObject user;
    private LayerMask layer = 1 << 8;
    private bool visible = false;// if the wall can be seen by user (be hit by raycast), it's visible
	// Use this for initialization
	void Start () {
        slider = transform.GetChild(0).gameObject;
        user = GameObject.Find("Camera (eye)").gameObject;
        //user = GameObject.Find("Cylinder").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Vector3 raydir;
        //ray = -transform.forward;
        if (thisWallType == wallType.curve)
            raydir = transform.position - user.transform.position;
        else
            //inverse it's direction every frame to make sure the wall could be detected no matter which side the user is
            raydir = -transform.forward;
        raydir.y = 0;
        Color color = Color.green;
        //Debug.DrawRay(user.transform.position, raydir.normalized * 2, color, 0.1f, true);
        if (Physics.Raycast(user.transform.position, raydir, out hit, 2f, layer))
        {
            //print("hit point =" + hit.collider.gameObject.name);
            if (hit.collider.gameObject == this.gameObject)
            {
                //print("hit");
                slider.SetActive(true);
                //to avoid user and target stay in a staight line which will influece the obstacle avoidance
                slider.transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                slider.transform.forward = hit.normal;
                visible = true;
            }
            else
            {
                visible = false;
                slider.SetActive(false);
            }
        }
        else
        {
            visible = false;
            slider.SetActive(false);
        }
	}

    public bool GetVisible()
    {
        return visible;
    }
    /*private void OnTriggerStay(Collider other)
    {
        if (other.tag.CompareTo("Player") == 0)
        {
            slider.SetActive(true);
            //keep the slider following user and keep the in the boundary of the wall
            if (Judge_Direction(slider))
            {
                if (slider.transform.position.x > transform.position.x + transform.localScale.x / 2)
                    slider.transform.position = new Vector3(transform.position.x + transform.localScale.x / 2, slider.transform.position.y, slider.transform.position.z);
                else if (slider.transform.position.x < transform.position.x - transform.localScale.x / 2)
                    slider.transform.position = new Vector3(transform.position.x - transform.localScale.x / 2, slider.transform.position.y, slider.transform.position.z);
                else
                    slider.transform.position = new Vector3(user.transform.position.x, slider.transform.position.y, slider.transform.position.z);
            }
            else
            {
                if(slider.transform.position.z > transform.position.z + transform.localScale.z / 2)
                    slider.transform.position = new Vector3(transform.position.z + transform.localScale.z / 2, slider.transform.position.y, user.transform.position.z);
                else if(slider.transform.position.z < transform.position.z - transform.localScale.z / 2)
                    slider.transform.position = new Vector3(transform.position.z - transform.localScale.z / 2, slider.transform.position.y, user.transform.position.z);
                else
                    slider.transform.position = new Vector3(slider.transform.position.x, slider.transform.position.y, user.transform.position.z);
            }
        }
        
    }*/

    private bool Judge_Direction(GameObject wall)
    {
        float angle = AngleSigned(wall.transform.right, Vector3.right, Vector3.up);
        if (Mathf.Abs(angle) > 85 && Mathf.Abs(angle) < 105)
            return false;
        else
            return true;

    }

    public float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)//return the angle between two vectors
    {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }
    /*private void OnTriggerExit(Collider other)
    {
        if (other.tag.CompareTo("Player") == 0)
        {
            slider.SetActive(false);
        }
    }*/
}
