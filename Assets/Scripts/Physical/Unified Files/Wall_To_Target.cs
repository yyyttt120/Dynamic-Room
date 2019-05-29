using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_To_Target : MonoBehaviour {
    public RoombaFeedback_Test controll;
    public Obstacle_Avoid avoidance;
    public enum Wall_Mov_Dir { Left_Right, Forward_Back};

    public Wall_Mov_Dir robotic_wall_dir = Wall_Mov_Dir.Left_Right;

    public GameObject wall;
    public int wallnum;
    public int p1 = 10;//parameter for translation(angle)
    public int p2 = 600;//parameter for translation(distance)
    public int p = 12;//parameter for rotation
    public int d_rotate = 200;//parameter for rotation

    //**** turn it false in real demo ***************
    public bool switch_button = false;
    //**************
    private bool start = true;//enable the whole system, controlled by a public method
    private bool start_translate = true;//enable the translation
    private bool start_rotate = false;//enable the rotation
    private bool in_turn = false;//in the area for turning
    private bool record_in_turn;
    private float time;

    private Animator anim;
    private GameObject target;
    private Vector3 avoidTarget;//the temporary target when avoiding obstacle 
    private Vector3 last_target_pos;//position of the last target
    // Use this for initialization
    void Start () {
        last_target_pos = target.transform.position;
        record_in_turn = in_turn;
        //avoidance = this.GetComponent<Obstacle_Avoid>();
        anim = gameObject.GetComponent<Animator>();
        time = 0;
    }

    // Update is called once per frame
    void Update() {
        time += Time.deltaTime;
        anim = gameObject.GetComponent<Animator>();
        print(gameObject.name + "target =" + target.name + target.transform.parent.name);
        if (target == null)
        {
            print(this.name + "no target");
            anim.SetBool("NoTarget", true);
        }
        else
        {
            anim.SetBool("NoTarget", false);
            //print(this.name + "target =" + target.name + target.transform.parent.name);
            //print("avoidance = " + avoidance.gameObject.name);
            avoidance.SetDetectDirection(target);
            //print("start = " + start);
            bool state = !start_translate && !start_rotate;
            //print("state =" + state);
            //when the target position changed
            float distance = (last_target_pos - target.transform.position).magnitude;//distance between last target and current target
            last_target_pos = target.transform.position;
            //***************
            float err_dis = 0.001f;
            if (anim == null)
                print("no animator");
            else
            {

                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Standby"))
                    err_dis = 0.005f;
                else
                    err_dis = 0.001f;
            }
            //*****************
            if (distance > err_dis /*|| !in_turn*/)
            {
                //print("translate3:" + start_translate);
                start_translate = true;
                //print("translate4:" + start_translate);
            }

            //print("start =" + start);
            if (Input.GetKeyUp(KeyCode.K))
            {
                switch_button = !switch_button;
            }
            Color color = Color.blue;
            Debug.DrawRay(target.transform.position, avoidance.GetAviodanceVector(), color, 0.1f, true);
            if (time >= 0.02)
            {
                time = 0;
                if (switch_button)
                {
                    //print("PID On");
                    if (start)
                    {
                        //print(this.name + "target =" + target.name);
                        if (start_translate)
                        {
                            avoidTarget = target.transform.position + avoidance.GetAviodanceVector();
                            //Debug.DrawRay(avoidTarget, Vector3.forward * 1f, color,0.1f,true);
                            //Debug.DrawRay(target.transform.position, avoidance.GetAviodanceVector(), color, 0.1f, true);
                            if (robotic_wall_dir == Wall_Mov_Dir.Left_Right)
                                start_translate = !controll.Translation_LR(controll.FindRoomba(avoidTarget, target.transform.forward), wall, wallnum, true, p1, p2);
                            else
                                start_translate = !controll.Translation_FB(controll.FindRoomba(avoidTarget, target.transform.forward), wall, wallnum, true, p1, p2);
                            //print("translate2:" + start_translate);
                        }
                    }
                    //**********************************************
                    print($"start translate = {start_translate}");
                    if (!start_translate)
                    {
                        //print("rotating");
                        double angle = AngleSigned(wall.transform.forward, target.transform.forward, Vector3.up);
                        if (angle > -90 && angle < 90)
                            start_rotate = !controll.Rotation(target.transform.forward, wall, wallnum, p, d_rotate, true);
                        else
                            start_rotate = !controll.Rotation(-target.transform.forward, wall, wallnum, p, d_rotate, true);
                    }

                }
            }
        }
    }

    private void FixedUpdate()
    {
        /*if (switch_button)
        {
            //print("PID On");
            if (start)
            {
                //print(this.name + "target =" + target.name);
                if (start_translate)
                {
                    avoidTarget = target.transform.position + avoidance.GetAviodanceVector();
                    //Debug.DrawRay(avoidTarget, Vector3.forward * 1f, color,0.1f,true);
                    //Debug.DrawRay(target.transform.position, avoidance.GetAviodanceVector(), color, 0.1f, true);
                    if (robotic_wall_dir == Wall_Mov_Dir.Left_Right)
                        start_translate = !controll.Translation_LR(controll.FindRoomba(avoidTarget, target.transform.forward), wall, wallnum, true, p1, p2);
                    else
                        start_translate = !controll.Translation_FB(controll.FindRoomba(avoidTarget, target.transform.forward), wall, wallnum, true, p1, p2);
                    //print("translate2:" + start_translate);
                }
            }
            //**********************************************
            /*if (!start_translate)
            {
                print("rotating");
                double angle = AngleSigned(wall.transform.forward, target.transform.forward, Vector3.up);
                if (angle > -90 && angle < 90)
                    start_rotate = !controll.Rotation(target.transform.forward, wall, wallnum, p, d_rotate, true);
                else
                    start_rotate = !controll.Rotation(-target.transform.forward, wall, wallnum, p, d_rotate, true);
            }

        }*/
    }

    public double AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)//return the angle between two vectors
    {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }

    private bool EdgeTrigger(bool bo)
    {
        bool edge;
        if (record_in_turn == bo)
            edge = false;
        else
            edge = true;
        record_in_turn = bo;
        return edge;
    }

    public void Robot_Move_Switch(bool switcher)
    {
        start = switcher;
    }

    public void Set_Target(GameObject target_input)
    {
        target = target_input;
        //print("set target =" + target.name);
    }

    public void Robot_Translate_Switch(bool switcher)
    {
        start_translate = switcher;
    }

    //get the state of the robotic wall(have the movement finished?)
    //if finished return true;
    public bool Get_State()
    {
        bool state = !start_rotate && !start_translate;
        return state;
    }

    public void SetP1(int p1_)
    {
        p1 = p1_;
    }

    public GameObject GetTarget()
    {
        if (target != null)
            return target;
        else
            return null;
    }
}
