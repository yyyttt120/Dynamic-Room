using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backup : MonoBehaviour {
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{

    public GameObject wall1;
    public GameObject wall2;
    public GameObject userFace;

    public float distance;
    public float speed;

    private float L1;//the first translation distance
    private float L2;//the second translation distance
    private float angle;
    private float rotationcount;
    private float translationCount;
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
    void Start()
    {
        start = false;
        rotationcount = 0;
        rotationStart1 = false;
        translationCount = 0;
        translationStart1 = false;
        rotationStart2 = false;
        translationStart2 = false;
        record = true;
        wall2Start = false;
    }

    // Update is called once per frame
    void Update()
    {
        //initiation
        if (Input.GetKeyUp(KeyCode.K))
        {
            userforward = userFace.transform.TransformDirection(Vector3.forward);
            userforward = new Vector3(userforward.x, 0, userforward.z).normalized;
            userleft = new Vector3(-userforward.z, userforward.y, userforward.x);
            userPosition = userFace.transform;
            rotationStart1 = true;
            record = true;
            start = true;
        }
        if (start)
            WallMotion(userleft, wall1, userPosition);
        /*if (Input.GetKeyUp(KeyCode.L))
        {
            rotationStart1 = true;
            wall2Start = true;
        }
        if(wall2Start)
            WallMotion(userforward, wall2, userPosition);
    }
    // wall moving function
    public void VMoveForward(GameObject wall)
    {
        wall.transform.Translate(Vector3.right * speed * Time.deltaTime / 100);
    }

    public void VMoveBack(GameObject wall)
    {
        wall.transform.Translate(Vector3.left * speed * Time.deltaTime / 100);

    }

    public void VTureLeft(GameObject wall)
    {
        wall.transform.Rotate(new Vector3(0, 1f, 0) * -speed * Time.deltaTime);
    }

    public void VTureRight(GameObject wall)
    {
        wall.transform.Rotate(new Vector3(0, 1f, 0) * speed * Time.deltaTime);
    }

    void WallRotation(GameObject wall, float angle, int type)//wall rotation controlling function
    {

        switch (type)
        {
            case 1:
                rotationcount = rotationcount + speed * Time.deltaTime;
                if (rotationcount <= angle)
                {
                    VTureRight(wall);
                }
                else
                {
                    rotationcount = 0;
                    rotationStart1 = false;
                    translationStart1 = true;
                }
                break;

            case 2:
                rotationcount = rotationcount + speed * Time.deltaTime;
                if (rotationcount <= 180 - angle)
                {
                    VTureLeft(wall);
                }
                else
                {
                    rotationcount = 0;
                    rotationStart1 = false;
                    translationStart1 = true;
                }
                break;

            case 3:
                rotationcount = rotationcount + speed * Time.deltaTime;
                if (rotationcount <= 180 + angle)
                {
                    VTureRight(wall);
                }
                else
                {
                    rotationcount = 0;
                    rotationStart1 = false;
                    translationStart1 = true;
                }
                break;

            case 4:
                rotationcount = rotationcount + speed * Time.deltaTime;
                if (rotationcount <= -angle)
                {
                    VTureLeft(wall);
                }
                else
                {
                    rotationcount = 0;
                    rotationStart1 = false;
                    translationStart1 = true;
                }
                break;

            default:
                rotationStart1 = false;
                break;

        }
    }
    void WallTranslation(GameObject wall, float length, int type)
    {
        switch (type)
        {
            case 1:
            case 4:
                translationCount = translationCount + speed * Time.deltaTime / 100;
                if (translationCount <= length)
                {
                    VMoveBack(wall);
                }
                else
                {
                    translationCount = 0;
                    translationStart1 = false;
                    rotationStart2 = true;
                };
                break;

            case 2:
            case 3:
                translationCount = translationCount + speed * Time.deltaTime / 100;
                if (translationCount <= length)
                {
                    VMoveForward(wall);
                }
                else
                {
                    translationCount = 0;
                    translationStart1 = false;
                    rotationStart2 = true;
                };
                break;
        }


    }

    public float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
    {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }

    public static Vector2 IgnoreYAxisa(Vector3 vector3)
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

    public Vector3 FindThePosition(Vector3 direcion, Vector3 userposition, float distance)
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

    private int MotionType(float angle)
    {
        int type;
        if (angle > 0 && angle <= 90)
            type = 1;
        else
            if (angle > 90 && angle <= 180)
            type = 2;
        else
            if (angle > -180 && angle <= -90)
            type = 3;
        else
            type = 4;
        return type;
    }

    void WallMotion(Vector3 epwDirection, GameObject wall, Transform user)// epwDirection: expected parallel direction of the wall
    {

        Vector3 evwDirection = new Vector3(epwDirection.z, 0, epwDirection.x);
        Vector3 wallposition;
        wallposition = Vector3.back;
        Vector3 wallTargetPosition;
        if (record)
        {
            angle = AngleSigned(wall.transform.forward, epwDirection, new Vector3(0, 1, 0));
            wallposition = wall.transform.position;
            wallTargetPosition = FindThePosition(evwDirection, user.position, distance);
            Debug.Log(wallTargetPosition);
            L1 = DistanceOfPointToVector(epwDirection, wallTargetPosition, wallposition);
            Debug.Log(L1);
            L2 = DistanceOfPointToVector(evwDirection, wallposition, wallTargetPosition);
            Debug.Log(L2);
            type = MotionType(angle);
            record = false;
        };
        //rotation STEP1
        if (rotationStart1)
        {
            WallRotation(wall, angle, type);
        }

        //translation STEP2
        if (translationStart1)
        {
            WallTranslation(wall, L1, type);
        }

        //rotation STEP3
        if (rotationStart2)
        {
            switch (type)
            {
                case 1:
                case 4:
                    rotationcount = rotationcount + speed * Time.deltaTime;
                    if (rotationcount <= 90)
                    {
                        VTureLeft(wall);
                    }
                    else
                    {
                        rotationcount = 0;
                        rotationStart2 = false;
                        translationStart2 = true;
                    }
                    break;

                case 2:
                case 3:
                    rotationcount = rotationcount + speed * Time.deltaTime;
                    if (rotationcount <= 90)
                    {
                        VTureRight(wall);
                    }
                    else
                    {
                        rotationcount = 0;
                        rotationStart2 = false;
                        translationStart2 = true;
                    }
                    break;
            }
        }

        //translation STEP4
        if (translationStart2)
        {
            translationCount = translationCount + speed * Time.deltaTime / 100;
            if (translationCount <= L2)
            {
                VMoveForward(wall);
            }
            else
            {
                translationCount = 0;
                translationStart2 = false;
            };
        }
    }
}*/
}
