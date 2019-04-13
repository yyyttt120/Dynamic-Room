using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallState : MonoBehaviour {
    private GameObject wall;
    private GameObject slider;
    private GameObject user;
	// Use this for initialization
	void Start () {
        slider = wall.transform.GetChild(0).gameObject;
        user = transform.Find("Camera (eye)").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (Judge_Direction(wall))
            slider.transform.position = new Vector3(user.transform.position.x, wall.transform.position.y, wall.transform.position.z);
        else
            slider.transform.position = new Vector3(wall.transform.position.x, wall.transform.position.y, user.transform.position.z);
    }

    public void SetWall(GameObject targetwall)
    {
        wall = targetwall;
    }

    public GameObject GetWall()
    {
        if (wall == null)
            return wall;
        else
        {
            Debug.Log("no wall gameobject is set");
            return null;
        }
    }


    //Judge the direction the wall is in
    //if it's in x direction, return true
    //if it's in z direction, return false
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
}
