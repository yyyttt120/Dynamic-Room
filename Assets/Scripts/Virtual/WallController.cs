using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour {
/*
    public GameObject wall1;
    public GameObject wall2;
    public GameObject userFace;

    public float distance;
    public float speed;

    private float L1;//the first translation distance
    private float L2;//the second translation distance
    private float angle;
    private float rotationcount1;
    private float rotationcount2;
    private float translationCount1;
    private float translationCount2;
    private int type;

    private bool rotationStart1;
    private bool translationStart1;
    private bool rotationStart2;
    private bool translationStart2;
    private bool record;
    private bool start;
    private bool wall2Start;

    private Vector3 userforward;
    private Vector3 userleft;

    private Transform userPosition;

	// Use this for initialization
	void Start () {
        start = false;
        rotationcount1 = 0;
        rotationcount2 = 0;
        rotationStart1 = false;
        translationCount1 = 0;
        translationCount2 = 0;
        translationStart1 = false;
        rotationStart2 = false;
        translationStart2 = false;
        record = true;
        wall2Start = false;
    }
	
	// Update is called once per frame
	void Update () {
        
        
    }
    // wall moving function
    public  void VMoveForward(GameObject wall)
    {
        wall.transform.Translate(Vector3.right * speed * Time.deltaTime/100);
    }

    public void VMoveBack(GameObject wall)
    {
        wall.transform.Translate(Vector3.left * speed * Time.deltaTime/100);

    }

    public void VTureLeft(GameObject wall)
    {
        wall.transform.Rotate(new Vector3(0, 1f, 0) * -speed * Time.deltaTime);
    }

    public void VTureRight(GameObject wall)
    {
        wall.transform.Rotate(new Vector3(0, 1f, 0) * speed * Time.deltaTime);
    }

    public void VStop(GameObject wall)
    {
        
    }


    public void WallRotation(GameObject wall, Vector3 targetDirection,float count)//wall rotation controlling function
    {
        float angle = AngleSigned(wall.transform.forward, targetDirection, Vector3.up);
        if(angle >= 0)
        {
            if (count <= angle)
            {
                VTureRight(wall);
                count = count + speed * Time.deltaTime;
            }
            else
                VStop(wall);
        }
        else
        {
            if (count >= angle)
            {
                VTureLeft(wall);
                count = count - speed * Time.deltaTime;
            }
            else
                VStop(wall);
        }
        

        
    }
    public void WallTranslation(GameObject wall,float length,float count)
    {
        if (length >= 0)
        {
            if(count <= length)
            {
                VMoveForward(wall);
                count = count + speed * Time.deltaTime / 100;
            }
            else
            {
                VStop(wall);
            }
        }
        else
        {
            if (count >= length)
            {
                VMoveBack(wall);
                count = count - speed * Time.deltaTime / 100;
            }
            else
                VStop(wall);
        }
    }

    public float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
    {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }

    public static Vector2 IgnoreYAxisa( Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.z);
    }

    public float DistanceOfPointToVector(Vector3 direction, Vector3 startpoint, Vector3 point)//ditance of point to vector
    {
        Vector2 directionVe2 = IgnoreYAxisa(direction);
        Vector3 startpiontVe2 = IgnoreYAxisa(startpoint);
        float A = directionVe2.y;
        float B = -directionVe2.x;
        float C = directionVe2.x * startpiontVe2.y - directionVe2.y * startpiontVe2.x;
        float denominator = Mathf.Sqrt(A * A + B * B);
        Vector2 pointVe2 = IgnoreYAxisa(point);
        return Mathf.Abs((A * pointVe2.x + B * pointVe2.y + C) / denominator); ;
    }

    public Vector3 FindThePosition(Vector3 direcion,Vector3 userposition,float distance)
    {
        Vector2 directionVe2 = IgnoreYAxisa(direcion);
        Vector2 userVe2 = IgnoreYAxisa(userposition);
        float a = directionVe2.x;
        float b = directionVe2.y;
        float x1 = a * distance / Mathf.Sqrt(a * a + b * b) + userVe2.x;
        float x2 = b * distance / Mathf.Sqrt(a * a + b * b) + userVe2.y;
        return new Vector3(x1, 0, x2);
    }

    private Vector2 FindUserForward(GameObject user)
    {
        Vector3 userfowardVe3 = user.transform.TransformDirection(Vector3.forward);
        userfowardVe3 = new Vector3(userforward.x, 0, userforward.z).normalized;
        Vector2 userfowardVe2 = IgnoreYAxisa(userfowardVe3);
        return userfowardVe2;
    }
    */
}
