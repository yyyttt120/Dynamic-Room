using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attached onto a wall
public class Slider_Controller : MonoBehaviour {
    public enum wallType { flat, curve};
    public wallType thisWallType = wallType.flat;
    public enum ControllerType { simulation,real};
    public ControllerType thisController = ControllerType.real;
    [Range(0,0.5f)]
    public float Boundary_offset = 0;
    private float wallSize;
    private GameObject slider;
    private GameObject user;
    private LayerMask layer = 1 << 8;
    private VirtualWall vwall;
    private bool visible = false;// if the wall can be seen by user (be hit by raycast), it's visible
    
    
    void Start()
    {
        slider = transform.GetChild(0).gameObject;
        if (thisController == ControllerType.real)
            user = GameObject.Find("Camera (eye)").gameObject;
        else
            user = GameObject.Find("Cylinder").gameObject;
        //print($"angle = {transform.eulerAngles.y}");
        if((transform.eulerAngles.y < 95 && transform.eulerAngles.y > 85) || (transform.eulerAngles.y < 275 && transform.eulerAngles.y > 265))
            wallSize = gameObject.GetComponent<Renderer>().bounds.size.z;
        else
            wallSize = gameObject.GetComponent<Renderer>().bounds.size.x;
        //user = GameObject.Find("Cylinder").gameObject;
        vwall = GetComponent<VirtualWall>();
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
        float range;
        if (thisController == ControllerType.real)
            range = 10f;
        else
            range = 10f;

        /* move the slider with user, if the user is not touching the correctly matched virtual wall  */
        if (!vwall.touching)
        {
            if (Physics.Raycast(user.transform.position, raydir, out hit, range, layer))
            {
                //print("hit point =" + hit.collider.gameObject.name);d
                if (hit.collider.gameObject == this.gameObject)
                {
                    //print("hit");
                    slider.SetActive(true);
                    //set a boundary for the silder, it it's 0.4m smaller than wall mesh both at left and right side
                    Vector3 userPos_local = transform.InverseTransformPoint(hit.point);
                    //print($"local pos = {userPos_local}");
                    //print($"wallsize = {wallSize}");
                    if (userPos_local.x > 0.5 - (0.4 / wallSize)-Boundary_offset)
                    {
                        Vector3 temp = slider.transform.localPosition;
                        temp.x = (float)(0.5 - (0.4 / wallSize)-Boundary_offset);
                        slider.transform.localPosition = temp;
                    }
                    else if (userPos_local.x < -0.5 + (0.4 / wallSize)+Boundary_offset)
                    {
                        Vector3 temp = slider.transform.localPosition;
                        temp.x = (float)(-0.5 + (0.4 / wallSize)+Boundary_offset);
                        slider.transform.localPosition = temp;
                    }
                    else
                        slider.transform.localPosition = new Vector3(userPos_local.x, slider.transform.localPosition.y, 0) - new Vector3(0, 0, 1);
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
