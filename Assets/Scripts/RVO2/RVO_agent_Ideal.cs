using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RVO;
using Vector2 = RVO.Vector2;
using Random = System.Random;
using Functions;

public class RVO_agent_Ideal : MonoBehaviour
{
    [HideInInspector] public int sid = -1;
    private GameObject _target;//the target of this robotic wall
    private Vector3 posLastFrame;
    private float time;
    private RaycastHit hit;
    private bool closeToTarget;//true when this robotic wall is close to it's target virtual wall
    private float avoidThreshold = 0.20f;//when the robotic wall is closer to target than this value, disable avoidance module
    private bool flagTooFar;//true when flag is too far from the robotic wall
    /** Random number generator. */
    private Random m_random = new Random();
    public GameObject target
    {
        get
        {
            return _target;
        }
        set
        {
            _target = value;
        }
    }
    private float maxSpeed = 1f;//max speed of this robotic wall

    private Robotic_Wall thisRWall;
    private GameObject flag;// a temp gameobject to record the next position for robotic wall in rvo simulation

    // Start is called before the first frame update
    void Start()
    {
        thisRWall = GetComponent<Robotic_Wall>();
        flag = new GameObject();
        flag.transform.position = transform.position;
        maxSpeed = GameObject.Find("StatesController").GetComponent<FSMSystem>().maxSpeed;
    }

    /*private void OnDrawGizmos()
    {
        // Draw a semitransparent blue cube at the transforms position
        if (sid > 0)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(flag.transform.position, new Vector3(0.1f, 0.1f, 0.1f));
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawSphere(simulatedPos_roomba, 0.1f);
            Gizmos.color = new Color(0, 0, 1, 0.5f);
            Gizmos.DrawSphere(targetPos_roomba, 0.1f);
        }
    }*/

    private void Update()
    {
        if (sid > 0)
        {
            float speed = (transform.position - posLastFrame).magnitude / Time.deltaTime;
            //print($"{this.name} speed = {speed}");
            posLastFrame = transform.position;
            _target = GetComponent<RVO_agent>().target;
            Rotation(_target.transform.forward, this.gameObject, 0, 0, 0, true);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (sid > 0)
        {
            //calibrate the position of rvo agent of robotic wall in simulation based on the current position of this robotic wall
            //Simulator.Instance.setAgentPosition(sid, new Vector2(transform.position.x, transform.position.z));

            //reset the detection range of rvo algorithm for this agent based on whether this robotic wall is close to it's target
            closeToTarget = DisableThisModule();
            if (closeToTarget)
                Simulator.Instance.setAgentNeighborDist(sid, 0.2f);
            else
                Simulator.Instance.setAgentNeighborDist(sid, 3f);

            //set prefered velocity to the the velocity toward the target with max speed
            Vector3 vel_3 =  FindRoomba(_target.transform.position,_target.transform.forward) - this.transform.position;
            Vector2 prefVel = new Vector2(vel_3.x, vel_3.z);
            if (RVOMath.abs(prefVel) > 0.2)
                prefVel = RVOMath.normalize(prefVel) * maxSpeed;
            else
                prefVel = prefVel * maxSpeed * 5;
            Simulator.Instance.setAgentPrefVelocity(sid, prefVel);
            /* Perturb a little to avoid deadlocks due to perfect symmetry. */
            float angle = (float)m_random.NextDouble() * 2.0f * (float)Mathf.PI;
            float dist = (float)m_random.NextDouble() * 0.0001f;

            /*Simulator.Instance.setAgentPrefVelocity(sid, Simulator.Instance.getAgentPrefVelocity(sid) +
                                                         dist *
                                                         new Vector2((float)Mathf.Cos(angle), (float)Mathf.Sin(angle)));*/
        }
        //reset the position of flag to the location of robotic wall, if it's too far from robotic wall
        
        //print($"distance = {dis_vec.magnitude}")
    }

    private void LateUpdate()
    {
        if (sid > 0)
        {//use the simulated position by rvo to guide this robotic wall
            Vector2 velocity = Simulator.Instance.getAgentVelocity(sid);
            Vector3 velocity_3 = new Vector3(velocity.x(), 0, velocity.y());
            //print($"{this.name} velocity = {velocity_3*10}");
            Debug.DrawRay(transform.position, velocity_3);
            Vector2 pos = Simulator.Instance.getAgentPosition(sid);
            //if (!flagTooFar)
            {
                this.transform.position = new Vector3(pos.x(), transform.position.y, pos.y());
            }
            //else
            {
                //Simulator.Instance.setAgentPosition(sid, new Vector2(flag.transform.position.x, flag.transform.position.z));
            }
            //flag.transform.position = transform.position + velocity_3 * 20;
            //flag.transform.forward = _target.transform.forward;
            //print($"{gameObject.name} flag pos = {transform.position}");

            //thisRWall.wallToTarget_controller.Set_Target(flag);
            Vector3 dis_vec = _target.transform.position - transform.position;
            dis_vec.y = 0;
            /*if (dis_vec.magnitude > 0.3f)
                transform.right = velocity_3.normalized;
            else*/
                
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.CompareTo("Wall") == 0)
        {
            //print("close thing is =" + other.gameObject.name);
            if (other.gameObject == _target.transform.parent.gameObject)
            {
                closeToTarget = true;
            }

        }
 
    }

    public bool Rotation(Vector3 targetdirection, GameObject wall, int wallnum, double p1, double d, bool on)
    {
        double angle = SomeFunctions.AngleSigned(wall.transform.forward, targetdirection, Vector3.up);
        bool rotationOn = true;
        //print($"{this.gameObject.name} angle =" + angle);
        if (angle > 2 || angle < -2)
        {
            if (angle <= 0)
            {
                rotationOn = true;
                gameObject.transform.Rotate(new Vector3(0, -60 * Time.deltaTime, 0));
            }
            else
            {
                rotationOn = true;
                gameObject.transform.Rotate(new Vector3(0, 60 * Time.deltaTime, 0));
            }
        }
        else
            rotationOn = false;
        return !rotationOn;
    }

    public Vector3 FindRoomba(Vector3 wallposition, Vector3 walldirection)
    {
        /*Vector2 directionVe2 = IgnoreYAxisa(-walldirection);
        Vector2 wallVe2 = IgnoreYAxisa(wallposition);
        double a = directionVe2.x;
        double b = directionVe2.y;
        double x1 = a * roomba_distance / Math.Sqrt(a * a + b * b) + wallVe2.x;
        double x2 = b * roomba_distance / Math.Sqrt(a * a + b * b) + wallVe2.y;
        //GameObject roomba =  Instantiate(roomba_reference,new Vector3(x1, 0, x2),transform.rotation);//show the position of the roomba
        //roomba.transform.SetParent(wall1.transform, true);
        return new Vector3((float)x1, 0,(float) x2);*/
        wallposition.y = 0;
        walldirection.y = 0;
        Vector3 roombaPos = wallposition - walldirection.normalized * 0.12f;
        return roombaPos;

    }

    private bool DisableThisModule()
    {
        bool close = false;

        if (_target != null)
        {
            Vector3 rtov = _target.transform.position - this.transform.position;
            rtov.y = 0;
            Vector3 raydir;
            if (Vector3.Dot(rtov, _target.transform.forward) > 0)
                raydir = _target.transform.forward;
            else
                raydir = -_target.transform.forward;
            if (Physics.Raycast(this.transform.position, raydir, out hit, avoidThreshold, (1 << 8)))
            {
                if (hit.collider.gameObject == _target.transform.parent.gameObject)
                {
                    close = true;
                }
            }
        }
        return close;
    }

}
