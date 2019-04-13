using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hall : MonoBehaviour
{

    public GameObject wall1;
    public GameObject wall2;
    public GameObject userFace;

    public float speed;
    public float distance;

    private GameObject wallRef;

    private float L1;//the first translation distance
    private float L2;//the second translation distance
    private float angle;
    private float rotationcount;
    private float translationCount;
    private float translationoffset = 0.02f;
    private float rotationoffset = 0.5f;
    private float originalAngle = 0;
    private int type;
    private int hallindex;

    private bool rotationStart1;
    private bool translationStart1;
    private bool rotationStart2;
    private bool translationStart2;
    private bool record;
    private bool start;
    private bool wall2Start;
    private bool hallStart;
    private bool rRecord;

    bool step0 ; //for hall moving
    bool step1 ;
    bool step2 ;
    bool step3 ;
    bool step4 ;
    bool step5 ;
    bool step6 ;
    bool roundOver ;

    private Vector3 userforward;
    private Vector3 userleft;
    private Vector3 hallforward;
    private Vector3 hallleft;
    private Vector3 firstwall;
    private Vector3 secondwall;


    // Use this for initialization
    void Start()
    {
        step0 = false; //for hall moving
        step1 = false;
        step2 = false;
        step3 = false;
        step4 = false;
        step5 = false;
        step6 = false;
        roundOver = false;


        start = false;
        rotationcount = 0;
        rotationStart1 = false;
        translationCount = 0;
        translationStart1 = false;
        rotationStart2 = false;
        translationStart2 = false;
        record = true;
        rRecord = true;
        wall2Start = false;
        hallforward = transform.forward;
        hallforward = new Vector3(hallforward.x, 0, hallforward.z).normalized;
        hallleft = new Vector3(-hallforward.z, 0, hallforward.x);
        firstwall = transform.GetChild(9).position;
        firstwall = new Vector3(firstwall.x, 0, firstwall.z);
        secondwall = transform.GetChild(8).position;
        secondwall = new Vector3(secondwall.x, 0, secondwall.z);

        hallindex = 2;
    }

    // Update is called once per frame
    void Update()
    {
        //initiation
        if (Input.GetKeyUp(KeyCode.K))
        {
            rotationStart1 = true;
            record = true;
            start = true;
        }
        if (start)
            WallMotion(hallleft, wall1, firstwall);
        if (Input.GetKeyUp(KeyCode.L))
        {
            rotationStart1 = true;
            record = true;
            wall2Start = true;
        }
        if (wall2Start)
            WallMotion(hallleft, wall2, secondwall);

        //dynamic hall
        if (Input.GetKeyUp(KeyCode.I))
        {
            hallStart = true;
            roundOver = true;
        }
        if (hallStart)
        {
            Vector3 userposition = userFace.transform.position;
            if (userposition.x < wall2.transform.position.x + 0.1)
            {
                if (roundOver)
                {
                    roundOver = false;
                    step0 = true;
                }

                HallMove(wall1);
            }
        }
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

    void WallMotion(Vector3 epwDirection, GameObject wall, Vector3 iniPosition)// epwDirection: expected parallel direction of the wall
    {

        Vector3 evwDirection = new Vector3(-epwDirection.z, 0, epwDirection.x);
        Vector3 wallposition;
        wallposition = Vector3.back;
        if (record)
        {
            angle = AngleSigned(wall.transform.forward, epwDirection, new Vector3(0, 1, 0));
            wallposition = wall.transform.position;
            L1 = DistanceOfPointToVector(epwDirection, iniPosition, wallposition);
            L2 = DistanceOfPointToVector(evwDirection, wallposition, iniPosition);
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
                        VTureRight(wall);
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
                        VTureLeft(wall);
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
                start = false;
                wall2Start = false;
            };
        }
    }

    void HallMove(GameObject wall)
    {

        if (step0)
        {

            //PhyWallrotation(hallforward, -90, wall1, step0, step1);
            float nowAngle1 = AngleSigned(wall1.transform.forward, hallforward, Vector3.up);
            if (nowAngle1 <= 91 && nowAngle1 >= 89)
            {
                step0 = false;
                step1 = true;
            }
            else
            {
                VTureLeft(wall);
            }
        }
        if (step1)
        {
            //PhyWalltranslation(hallleft, firstwall, 0.4f, wall1, step1, step2, false);
            float dis = DistanceOfPointToVector(hallleft, firstwall, wall.transform.position);
            if (dis < 0.4 - translationoffset || dis > 0.4 + translationoffset)
            {
                
                
                    VMoveBack(wall);
               
            }
            else
            {
                step1 = false;
                step2 = true;
            }
        }

        if (step2)
        {
            //PhyWallrotation(hallforward, 0, wall1, step2, step3);
            float nowAngle1 = AngleSigned(wall1.transform.forward, hallforward, Vector3.up);
            if (nowAngle1 <= 0 + rotationoffset && nowAngle1 >= 0- rotationoffset)
            {
                step2 = false;
                step3 = true;
            }
            else
            {
                VTureRight(wall);
            }
        }

        if (step3)
        {
            Vector3 targetline = transform.GetChild(hallindex).forward;
            //PhyWalltranslation(targetline, transform.GetChild(hallindex).position, 0, wall1, step3, step4, false);
            float dis = DistanceOfPointToVector(targetline, transform.GetChild(hallindex).position, wall.transform.position);
            if (dis < 0 - translationoffset || dis > 0 + translationoffset)
            {


                VMoveBack(wall);

            }
            else
            {
                step3 = false;
                step4 = true;
                hallindex = hallindex + 1;
                if (hallindex > 9)
                    hallindex = 9;
            }
        }

        if (step4)
        {
            float nowAngle1 = AngleSigned(wall1.transform.forward, hallforward, Vector3.up);
            if (nowAngle1 <= -90 + rotationoffset && nowAngle1 >= -90 - rotationoffset)
            {
                step4 = false;
                step5 = true;
            }
            else
            {
                VTureRight(wall);
            }
        }

        if (step5)
        {
            //PhyWalltranslation(hallleft, firstwall, 0, wall1, step1, step2, false);
            float dis = DistanceOfPointToVector(hallleft, firstwall, wall.transform.position);
            if (dis < -translationoffset || dis >  translationoffset)
            {


                VMoveBack(wall);

            }
            else
            {
                step5 = false;
                step6 = true;
            }
        }

        if (step6)
        {
            float angle1 = AngleSigned(wall1.transform.forward, hallforward, Vector3.up);
            if (angle1 < -rotationoffset || angle1 > rotationoffset)
            {
                VTureLeft(wall);
            }
            else
            {
                step6 = false;
                roundOver = true;
                wallRef = wall1;
                wall1 = wall2;
                wall2 = wallRef;

            }
        }


    }

    void PhyWalltranslation(Vector3 line, Vector3 point, float offset, GameObject wall, bool start1, bool start2, bool forward)
    {

        float dis = DistanceOfPointToVector(line, point, wall.transform.position);
        if (dis < offset - translationoffset || dis > offset + translationoffset)
        {
            if (forward)
            {
                VMoveForward(wall);
            }
            else
            {
                VMoveBack(wall);
            }
        }
        else
        {
            start1 = false;
            start2 = true;
        }
    }

    void PhyWallrotation(Vector3 direction, float angle, GameObject wall, bool start1, bool start2)
    {
        if (rRecord)
        {
            originalAngle = AngleSigned(wall.transform.forward, direction, Vector3.up) + angle;
            rRecord = false;
        }
        float nowAngle = AngleSigned(wall.transform.forward, direction, Vector3.up);

        if (originalAngle < 0 || originalAngle > 180)
        {
            if (nowAngle > -angle - rotationoffset && nowAngle < -angle + rotationoffset)
            {
                rRecord = true;
                start1 = false;
                start2 = true;
            }
            else
            {
                VTureLeft(wall);
            }
            Debug.Log(step0);
            Debug.Log(step1);

        }
        else
        {
            if (nowAngle > -angle - rotationoffset && nowAngle < -angle + rotationoffset)
            {
                rRecord = true;
                start1 = false;
                start2 = true;
            }
            else
            {
                VTureRight(wall);
            }
        }
    }
    
}


