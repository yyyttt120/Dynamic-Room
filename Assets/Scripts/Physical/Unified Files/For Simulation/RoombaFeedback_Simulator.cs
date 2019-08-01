using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaFeedback_Simulator : RoombaFeedback_Test
{
    // Start is called before the first frame update
    public float speed;
    public float accele;
    public float angleSpeed;

    private Vector3 target_pos;

    bool step1on = false;
    bool step2on = false;
    bool step3on = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        // Draw a semitransparent blue cube at the transforms position
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(target_pos, new Vector3(0.1f, 0.1f, 0.1f));
            /*Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawSphere(simulatedPos_roomba, 0.1f);
            Gizmos.color = new Color(0, 0, 1, 0.5f);
            Gizmos.DrawSphere(targetPos_roomba, 0.1f);*/
        }
    }

    public override bool Translation_LR(Vector3 targetposition, GameObject wall, int wallnum, bool on, double p1, double p2)
    {
        //print("simulation start");
        Vector3 roombaPos;
        int moveDirection;
        double angle;
        double distance;

        target_pos = targetposition;
        roombaPos = wall.transform.position; /*FindRoomba(wall.transform.position,wall.transform.forward)*/;
        roombaPos.y = 0;
        angle = AngleSigned(wall.transform.right, targetposition - roombaPos, Vector3.up);
        if (angle >= -90 && angle <= 90)
            moveDirection = 1;
        else
            moveDirection = -1;
        Vector3 walltotarget = targetposition - roombaPos;
        walltotarget.y = 0;
        distance = walltotarget.magnitude;
        //wall.transform.Rotate(new Vector3(0, angleSpeed * Time.deltaTime, 0));
        //print("angle =" + angle);
        //step 1: if the wall need to move(not at the target position) turn the head or end to the target

        //print("step2 =" + step2on);
        if (distance > err_distance)
        {
            //if (!step2on)
            {
                
                if (angle > -90 && angle <= 0)
                    if (angle < -2)
                    {
                        //print("rotating");
                        gameObject.transform.Rotate(new Vector3(0, -angleSpeed * Time.deltaTime, 0));
                        step1on = true;
                    }
                    else
                        step1on = false;
                if (angle > 90 && angle <= 180)
                    if (angle < 178)
                    {
                        //print("rotating");
                        gameObject.transform.Rotate(new Vector3(0, -angleSpeed * Time.deltaTime, 0));
                        step1on = true;
                    }
                    else
                        step1on = false;
                if (angle > 0 && angle <= 90)
                    if (angle > 2)
                    {
                        //print("rotating");
                        gameObject.transform.Rotate(new Vector3(0, angleSpeed * Time.deltaTime, 0));
                        step1on = true;
                    }
                    else
                        step1on = false;
                if (angle > -180 && angle <= -90)
                    if (angle > -178)
                    {
                        //print("rotating");
                        gameObject.transform.Rotate(new Vector3(0, angleSpeed * Time.deltaTime, 0));
                        step1on = true;
                    }
                    else
                        step1on = false;
            }

            //step 2: move to the target(roomba) position
            //print("step1 =" + step1on);
            if (!step1on)
            {
                //print("translating");
                step2on = true;
                gameObject.transform.Translate(gameObject.transform.right * moveDirection * Time.deltaTime * speed,Space.World);
            }
        }
        else
        {
            step2on = false;
            step1on = false;
        }

        //step 3: turn to the target direction*/
        return !step1on && !step2on;
    }

    public override bool Rotation(Vector3 targetdirection, GameObject wall, int wallnum, double p1, double d, bool on)
    {
        double angle = AngleSigned(wall.transform.forward, targetdirection, Vector3.up);
        bool rotationOn = true;
        //print("angle =" + angle);
        if (angle > 2 || angle < -2)
        {
            if (angle <= 0)
            {
                rotationOn = true;
                gameObject.transform.Rotate(new Vector3(0, -angleSpeed * Time.deltaTime, 0));
            }
            else
            {
                rotationOn = true;
                gameObject.transform.Rotate(new Vector3(0, angleSpeed * Time.deltaTime, 0));
            }
        }
        else
            rotationOn = false;
        return !rotationOn;
    }
}

