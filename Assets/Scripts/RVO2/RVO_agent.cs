using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RVO;
using Vector2 = RVO.Vector2;
using Random = System.Random;

public class RVO_agent : MonoBehaviour
{
    [HideInInspector] public int sid = -1;
    private GameObject _target;//the target of this robotic wall
    private Vector3 posLastFrame;
    private Vector3 simulatedPos;//the position of this agent in simulation
    private float time;
    private float avoidThreshold = 0.20f;//when the robotic wall is closer to target than this value, disable avoidance module
    private RaycastHit hit;
    private bool closeToTarget;//true when this robotic wall is close to it's target virtual wall
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
    private float maxSpeed = 0.5f;//max speed of this robotic wall

    private Robotic_Wall thisRWall;
    private GameObject flag;// a temp gameobject to record the next position for robotic wall in rvo simulation

    // Start is called before the first frame update
    void Start()
    {
        thisRWall = GetComponent<Robotic_Wall>();
        flag = new GameObject();
        flag.name = this.name + " flag";
        flag.transform.position = transform.position;
        simulatedPos = transform.position;
        maxSpeed = GameObject.Find("StatesController").GetComponent<FSMSystem>().maxSpeed;
    }

    private void OnDrawGizmos()
    {
        // Draw a semitransparent blue cube at the transforms position
        if (sid > 0)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(flag.transform.position, new Vector3(0.1f, 0.1f, 0.1f));
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawSphere(simulatedPos, 0.1f);
        }
    }

    private void Update()
    {
        if (sid > 0)
        {
            float speed = (transform.position - posLastFrame).magnitude / Time.deltaTime;
            //print($"{this.name} speed = {speed}");
            posLastFrame = transform.position;

            /*reset the detection range of rvo algorithm for this agent based on whether this robotic wall is close to it's target*/
            closeToTarget = DisableThisModule();

            print($"{this.name} target is {_target.transform.parent.name}:{_target.name}");
        }


    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (sid > 0)
        {
            if (closeToTarget)
            {
                //print("closed");
                Simulator.Instance.setAgentNeighborDist(sid, 0.05f);
            }
            else
                Simulator.Instance.setAgentNeighborDist(sid, 3f);

            //calibrate the position of rvo agent of robotic wall in simulation based on the current position of this robotic wall
            //Simulator.Instance.setAgentPosition(sid, new Vector2(transform.position.x, transform.position.z));

            /*set prefered velocity to the the velocity toward the target with max speed*/
            Vector3 vel_3 = _target.transform.position - simulatedPos;
            Vector2 prefVel = new Vector2(vel_3.x, vel_3.z);
            if (RVOMath.abs(prefVel) > 0.2)
                prefVel = RVOMath.normalize(prefVel) * maxSpeed;
            else
                prefVel = prefVel * maxSpeed;
            Simulator.Instance.setAgentPrefVelocity(sid, prefVel);
            /* Perturb a little to avoid deadlocks due to perfect symmetry. */
            float angle = (float)m_random.NextDouble() * 2.0f * (float)Mathf.PI;
            float dist = (float)m_random.NextDouble() * 0.0001f;

            Simulator.Instance.setAgentPrefVelocity(sid, Simulator.Instance.getAgentPrefVelocity(sid) +
                                                         dist *
                                                         new Vector2((float)Mathf.Cos(angle), (float)Mathf.Sin(angle)));
        }
        /*reset the position of agent in simulation to the location of robotic wall, if it's too far from robotic wall*/
        Vector3 dis_vec = simulatedPos - transform.position;
        dis_vec.y = 0;
        //print($"distance = {dis_vec.magnitude}");
        if (dis_vec.magnitude > 0.3f)
        {
            flagTooFar = true;
        }
        else
            flagTooFar = false;
        if(dis_vec.magnitude > 1f)
        {
            Simulator.Instance.setAgentPosition(sid, new Vector2(transform.position.x, transform.position.z));
            simulatedPos = transform.position;
        }
    }

    private void LateUpdate()
    {
        if (sid > 0)
        {
            /*use the simulated position by rvo to guide this robotic wall*/
            Vector2 velocity = Simulator.Instance.getAgentVelocity(sid);
            Vector3 velocity_3 = new Vector3(velocity.x(), 0, velocity.y());
            //print($"{this.name} velocity = {velocity_3*10}");
            Debug.DrawRay(transform.position, velocity_3);
            Vector2 pos = Simulator.Instance.getAgentPosition(sid);
            if (!flagTooFar)
            {
                simulatedPos = new Vector3(pos.x(), flag.transform.position.y, pos.y());
                //flag.transform.position = new Vector3(pos.x(), flag.transform.position.y, pos.y());
            }
            else
            {
                //Simulator.Instance.setAgentPosition(sid, new Vector2(flag.transform.position.x, flag.transform.position.z));
                Simulator.Instance.setAgentPosition(sid, new Vector2(simulatedPos.x, simulatedPos.z));
                
            }

            
            Vector3 dis_toFinalTarget = _target.transform.position - this.transform.position;
            dis_toFinalTarget.y = 0;
            /* when the flag is too far from robotic wall, position the flag at in-place */
            /*if (flagTooFar || (velocity_3.magnitude < 0.8 * maxSpeed && dis_toFinalTarget.magnitude > 0.5f))
                flag.transform.position = simulatedPos;*/
            /*when the robotic wall is far from final target, to enable PID controller, position the flag a little ahead of simulated position */
            /*else if (dis_toFinalTarget.magnitude > 0.5f)
            {
                Vector3 vec_temp = (simulatedPos - this.transform.position).normalized;
                vec_temp.y = 0;
                flag.transform.position = this.transform.position + vec_temp;
                //flag.transform.position = simulatedPos;
            }
            else
                flag.transform.position = _target.transform.position;*/
            if (dis_toFinalTarget.magnitude > 0.7f)
            {
                if (!flagTooFar)
                    flag.transform.position = simulatedPos;
                else
                {
                    Vector3 vec_temp = (simulatedPos - this.transform.position).normalized;
                    vec_temp.y = 0;
                    flag.transform.position = this.transform.position + vec_temp;
                }
            }
            else
                flag.transform.position = _target.transform.position;
            /*set the position for direction of flag based on target */
            flag.transform.forward = _target.transform.forward;
            /*set the flag as the temporary target of this robotic wall, it will lead this robotic wall to it's final target */
            thisRWall.wallToTarget_controller.Robot_Move_Switch(true);
            thisRWall.wallToTarget_controller.Set_Target(flag);
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
