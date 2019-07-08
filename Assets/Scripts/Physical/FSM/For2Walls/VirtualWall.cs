using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualWall : MonoBehaviour
{
    private Animator matchRWall;//the matched robotic wall
    private Animator matchRWall_record;//record the match RWall
    private GameObject user;
    private SteamVR_TrackedObject user_tracker;
    private int layMask = (1 << 8);
    [HideInInspector] public bool matched;// true, when it's matched by a robotic wall
    public bool touching;//be true, when the virtual wall is correctly matched and is touching by user
    public Interactable_Points[] IPs;
    private Vector3 originalPos;

    //for evaluation data
    private GameObject evalu_Data_Writer;
    private bool timerStart =false;
    private float timer = 0;
    private float userSpd = 0;
    //the parameters using for evaluation
    private float dis_sur2user;//the distance between user and this virtual wall
    private float dis_IP2user;//the minimal distance from the interactable points to user 
    private float angle_f;//the angle between the forward direction of user and the direction from user to this virtual wall
    private int untouched_amount_IP;//the amount of untouched interactable points of the virutal wall
    // Start is called before the first frame update
    void Start()
    {
        //initiation
        matchRWall = null;
        matched = false;
        user = GameObject.Find("Camera (eye)");
        //user_tracker = user.transform.GetChild(4).GetComponent<SteamVR_TrackedObject>();
        dis_sur2user = 1000;
        dis_IP2user = 1000;
        angle_f = 180;
        untouched_amount_IP = IPs.Length;
        evalu_Data_Writer = GameObject.Find("Evalu_Data_Writer");
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        /*if(matchRWall != null)
            print($"{gameObject.name} match rwall = {matchRWall.name}");
        else
            print($"{gameObject.name} match rwall = null");*/
        if (!transform.GetChild(0).gameObject.activeSelf || transform.GetChild(0).gameObject == null)
            matchRWall = null;
        if (matchRWall == null)
        {
            touching = false;
            print($"{gameObject.name} release wall");
            if (matchRWall_record != null)
            {
                matchRWall_record.GetBehaviour<Wall_State>().SetReadyRelease(true);
                matched = matchRWall_record.GetBehaviour<Wall_State>().matching;
                if(!matched)
                    matchRWall_record = null;
            }
            transform.position = originalPos;
        }
        else
        {
            matched = true;
            print($"{gameObject.name} match {matchRWall.name}");
            matchRWall_record = matchRWall;
            matchRWall.SetInteger("NearWallCounter", 10);
            matchRWall.GetBehaviour<Wall_State>().SetTargetWall(gameObject);
        }
    }
    private void FixedUpdate()
    {
        GetDis_sur2user();
        GetDis_IP2userNIP_amount();
        GetAngle_f();
        //write evaluation data
        RWall_Matched();
        if (timerStart)
        {
            timer += Time.deltaTime;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.CompareTo("Hand") == 0)
        {
            print("timer start");
            timerStart = true;
            timer = 0;
            //userSpd = GetUserSpeed();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.CompareTo("Hand") == 0)
        {
            touching = false;
        }
    }

    /*private float GetUserSpeed()
    {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)user_tracker.index);
        Vector3 vel = device.velocity;
        vel.y = 0;
        return vel.magnitude;
    }*/

    /* if the asigned robotic walls correctly matched this virtual wall, stop the timer */
    private void RWall_Matched()
    {
        if (matchRWall != null)
        {
            Vector3 dis = matchRWall.transform.position - matchRWall.GetComponent<RoombaFeedback_Test>().FindRoomba(transform.GetChild(0).position,transform.forward);
            dis.y = 0;
            double angle = AngleSigned(matchRWall.transform.forward, this.transform.forward, Vector3.up);
            if (dis.magnitude < 0.15f && ((angle < 10 && angle > -10)|| (angle >170 || angle < -170)) )
            {
                //print("timer stop");
                if (timerStart)
                {
                    evalu_Data_Writer.GetComponent<Evalu_Data_Writer>().WriteData(gameObject.name + " " + timer.ToString());
                    touching = true;
                }
                timerStart = false;
                calibrateVWall();
            }
        }
    }

    private void calibrateVWall()
    {
        
        //if (matchRWall != null)
        {
            print("calibating******************");
            GameObject matchedSide;
            Wall_To_Target.Side side = matchRWall.GetComponent<Wall_To_Target>().matchedSide;
            if (side == Wall_To_Target.Side.front)
                matchedSide = matchRWall.transform.GetChild(1).gameObject;
            else
                matchedSide = matchRWall.transform.GetChild(2).gameObject;

            if (Mathf.Abs(transform.forward.x) > 0)
            {
                transform.GetChild(1).position = new Vector3(matchedSide.transform.position.x, transform.position.y, transform.position.z);
                print($"{this.name} calibrate in x axis");
            }
            else
            {
                transform.GetChild(1).position = new Vector3(transform.position.x, transform.position.y, matchedSide.transform.position.z);
                print($"{this.name} calibrate in z axis");
            }
        }
    }

    public void SetMatchRWall(Animator rWall)
    {
        this.matchRWall = rWall;
    }

    public Animator GetMatchRWall()
    {
        return matchRWall;
    }

    public string GetInfo()
    {
        string info;
        Vector3 nullvec = new Vector3(0,0,0);
        string nulv = VectortoString(nullvec);
        switch (IPs.Length)
        {
            case 0:
                info = VectortoString(transform.position) + " " + VectortoString(transform.forward) + " " + nulv + " " + nulv;
                break;
            case 1:
                info = VectortoString(transform.position) + " " + VectortoString(transform.forward) + " " + VectortoString(IPs[0].transform.position) + " " + nulv;
                break;
            default:
                info = VectortoString(transform.position) + " " + VectortoString(transform.forward) + " " + VectortoString(IPs[0].transform.position) + " " + VectortoString(IPs[1].transform.position);
                break;
        }
        return info;
    }


    private string VectortoString(Vector3 vec)
    {
        string temp = string.Format("{0:N6}", vec.x) + " " + string.Format("{0:N6}", vec.y) + " " + string.Format("{0:N6}", vec.z);
        return temp;
    }

    private void GetAngle_f()
    {
        Vector3 userforward = user.transform.forward;
        userforward.y = 0;
        Vector3 user2Wall = -transform.forward;
        user2Wall.y = 0;
        angle_f = Vector3.Angle(userforward, user2Wall);
    }

    private void GetDis_sur2user()
    {
        RaycastHit hit;
        if(Physics.Raycast(this.transform.position, -this.gameObject.transform.forward, out hit, 100f, layMask))
        {
            dis_sur2user = hit.distance; 
        }
        else
            dis_sur2user = 0;
    }

    private void GetDis_IP2userNIP_amount()
    {
        if (IPs.Length > 0)
        {
            float min = 1000;
            int amount = 0;
            try
            {
                foreach (Interactable_Points ip in IPs)
                {
                    //dis_IP2user
                    float dis_temp = GetDistance(ip.gameObject, user);
                    if (dis_temp < min)
                        min = dis_temp;
                    //untouched_amount_IP
                    if (!ip.GetTouched())
                        amount++;
                }
                dis_IP2user = min;
                untouched_amount_IP = amount;
            }
            catch(System.NullReferenceException e1)
            {
                print($"{this.name} have no interactable points");
            }
        }
        else
        {
            dis_IP2user = 0;
            untouched_amount_IP = 0;
        }
    }

    public double AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)//return the angle between two vectors
    {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }

    private float GetDistance(GameObject a, GameObject b)
    {
        Vector3 v = a.transform.position - b.transform.position;
        v.y = 0;
        return v.magnitude;
    }


    //release the matching robotic wall
    public void ReleaseRWall()
    {
        matchRWall = null;
    }
}
