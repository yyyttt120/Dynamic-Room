using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Avoid : MonoBehaviour
{

    private SteamVR_TrackedObject tracker;
    private SteamVR_Controller.Device device;
    private GameObject target;
    private Robotic_Wall rWall;
    private int layMask = (1 << 9) | (1 << 10);
    private RaycastHit hit;
    private Vector3 wallPredict;//the moving direction of the robotic wall
    private Vector3 avoidanceVector;
    public float detectRange;//the range of predicted area
    public float force;//the force to avoid obstacle
    public float nonAvoidRange;//range of area where no need to avoid

    private List<bool> closeStateThisFrame;//record is this robotic wall get close to target in this frame

    private bool closeToTarget;//true when this robotic wall is close to it's target virtual wall
                               // Use this for initialization
    void Start()
    {
        rWall = new Robotic_Wall();
        rWall.Set_Robotic_Wall(this.gameObject);
        //tracker = transform.parent.gameObject.GetComponent<SteamVR_TrackedObject>();
        //device = SteamVR_Controller.Input((int)tracker.index);
        wallPredict = Vector3.zero;
    }

    private void FixedUpdate()
    {
        //closeStateThisFrame = new List<bool>();
        closeToTarget = false;
        try
        {
            target = rWall.wallToTarget_controller.GetTarget();
            //print("target =" + target.name + " " + target.transform.parent.name);
        }

        catch (System.NullReferenceException e1)
        {
            print("error" + e1.Message);
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //print("tracker" + tracker.gameObject.name);
        //device = SteamVR_Controller.Input((int)tracker.index);
        // *************

        /*foreach (bool close in closeStateThisFrame)
        {
            closeToTarget = false;
            print("**********" + close);
            closeToTarget = close || closeToTarget;
        }*/
        //print("close to target = " + closeToTarget);
        //wallPredict = GetMovingDir();
        //float angle = rWall.roomba_controller.AngleSigned(-this.transform.forward, device.velocity, Vector3.up);
        //print("angle = " + angle);
        //print("distance ="+ gameObject.name + wallPredict.magnitude);
        //print(gameObject.name + " target distance = " + wallPredict.magnitude);
        Vector3 moveDirection;
        if (Vector3.Dot(wallPredict.normalized, gameObject.transform.right) > 0)
            moveDirection = gameObject.transform.right;
        else
            moveDirection = -gameObject.transform.right;


        if (/*wallPredict.magnitude > nonAvoidRange*/!closeToTarget)
        //if (wallPredict.magnitude > nonAvoidRange)
        {
            //if (angle > -20 && angle < 20 || angle > 160 && angle < 180 || angle > -180 && angle < -160)// when the wall are moving ahead (not rotating)
            //{
            //print("huo" + gameObject.name);
            Color color = Color.red;
            Debug.DrawRay(this.transform.position, moveDirection.normalized * Mathf.Min(detectRange, wallPredict.magnitude), color, 0.1f, true);
            if (Physics.Raycast(this.transform.position, moveDirection.normalized, out hit, Mathf.Min(detectRange, wallPredict.magnitude), layMask))
            {
                //
                Color color_y = Color.yellow;
                Debug.DrawRay(hit.point, hit.normal, color_y, 0.1f, true);
                if (hit.collider.gameObject != gameObject)
                {
                    Vector3 hitNormal = hit.normal;
                    hitNormal.y = 0;
                    Vector3 paraSpeed = Vector3.Cross(hitNormal, Vector3.up).normalized;
                    Animator ani = gameObject.GetComponent<Animator>();
                    //when the robotic wall is in wall_state, force it always avoiding obstacle from the direction which is away from the virutal wall
                    if (ani.GetCurrentAnimatorStateInfo(0).IsName("Wall"))
                    {
                        Vector3 forward = gameObject.GetComponent<Wall_To_Target>().GetTarget().transform.forward;
                        if (Vector3.Dot(forward, paraSpeed) > 0)
                            paraSpeed = -paraSpeed;
                        else
                            paraSpeed = paraSpeed;
                    }
                    //when the robotic wall is not in wall_state, choose the most efficient direction
                    else
                    {
                        if (Vector3.Dot(moveDirection.normalized, paraSpeed) > 0)
                            paraSpeed = paraSpeed;
                        else
                            paraSpeed = -paraSpeed;
                    }
                    //print(gameObject.name + "ray hit" + hit.collider.gameObject.name);
                    avoidanceVector += paraSpeed * force;
                    avoidanceVector.y = 0;
                }
                /*else
                    avoidanceVector = Vector3.zero;*/
                double maxAvoidanceVector = 1 / hit.distance * 20;
                if (avoidanceVector.magnitude > maxAvoidanceVector)
                    avoidanceVector = avoidanceVector.normalized * (float)maxAvoidanceVector;

            }
            else
                avoidanceVector /= 1.05f;
            //}
            //else
            // avoidanceVector = Vector3.zero;
        }
        else
            avoidanceVector = Vector3.zero;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.CompareTo("Wall") == 0)
        {
            //print("close thing is =" + other.gameObject.name);
            if (other.gameObject == target.transform.parent.gameObject)
            {
                //print("closed");
                closeToTarget = true;
                //closeStateThisFrame.Add(true);
            }
            /*else
            {
                closeToTarget = false;
                closeStateThisFrame.Add(false);
            }*/

        }
        /*else
        {
            closeToTarget = false;
            closeStateThisFrame.Add(false);
        }*/
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (other.tag.CompareTo("Wall") == 0)
            if (other.gameObject == target.transform.parent.gameObject)
            {
                closeToTarget = false;
                closeStateThisFrame.Add(false);
            }
    }*/


    private Vector3 GetMovingDir()
    {
        double angle;//the angle between wall's right direction and velocity of the wall
        angle = rWall.roomba_controller.AngleSigned(-this.transform.forward, device.velocity, Vector3.up);
        if (angle < 90 && angle > -90)
            return this.transform.right;
        else
            return -this.transform.right;
    }

    public void SetDetectDirection(GameObject target)
    {
        Vector3 dir = target.transform.position - this.transform.position;
        dir.y = 0;
        wallPredict = dir;
    }

    public Vector3 GetAviodanceVector()
    {
        return avoidanceVector;
    }
}
