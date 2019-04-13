using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveWall : MonoBehaviour {
    private GameObject slider;
    private GameObject user;
    // Use this for initialization
    void Start()
    {
        slider = transform.GetChild(0).gameObject;
        user = GameObject.Find("Camera (eye)").gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.CompareTo("Player") == 0)
        {
            slider.SetActive(true);
            //keep the slider following user and keep the in the boundary of the wall
            /*if (Judge_Direction(slider))
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
                if (slider.transform.position.z > transform.position.z + transform.localScale.z / 2)
                    slider.transform.position = new Vector3(transform.position.z + transform.localScale.z / 2, slider.transform.position.y, user.transform.position.z);
                else if (slider.transform.position.z < transform.position.z - transform.localScale.z / 2)
                    slider.transform.position = new Vector3(transform.position.z - transform.localScale.z / 2, slider.transform.position.y, user.transform.position.z);
                else
                    slider.transform.position = new Vector3(slider.transform.position.x, slider.transform.position.y, user.transform.position.z);
            }*/
            slider.transform.position = GetBoundary(1.5f);
            Vector3 dir = transform.position - GetBoundary(1.5f);
            slider.transform.forward = -dir;
        }

    }

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
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.CompareTo("Player") == 0)
        {
            slider.SetActive(false);
        }
    }

    private Vector3 GetBoundary(float radius)
    {
        float err_x = user.transform.position.x - transform.position.x;
        float err_z = Mathf.Sqrt(radius * radius - err_x * err_x);
        float z = transform.position.z + err_z;
        return new Vector3(user.transform.position.x, transform.position.y, z);
    }
}
