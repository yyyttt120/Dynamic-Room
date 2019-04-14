using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;

public class RoombaFeedback_Test : MonoBehaviour {

    private RoombaControllerScript c2 = null;
    //Sendblue send = null;
    public GameObject wall;
    //public GameObject target1;
    //public GameObject wall2;
    //public GameObject target2;

    public GameObject roomba_reference;//reference for roomba
    public Renderer rend;

    public string portName;
    public int portSpeed;
    private static SerialPort s_serial;

    private double[] y_k = new double[4];
    private int n = 0;//when it is 4, output data
    private bool recodeflag = false;//when it is true, output data
    private double[] xx_k = new double[4];
    private ushort[] data1 = new ushort[4];//save the inputdata for oscillscope
    private ushort[] pos_Data_Temp = new ushort[4];
    private byte[] pos_Data = new byte[10];//position data for oscillscope
    byte[] data_re = new byte[10];

    //private public double distance;
    public double err_distance;
    private double originaldistance;
    private double distance_input;
    private int velocity_R;
    private int velocity_L;
    private double angle;//the angle between wall and target
    private double angle_input;
    //public double p1;//controlling variable of angle
    //public double p2;//controlling variable of distance
    public double d1;//D parameter of PID for translation(angle)
    public double d2;//D parameter of PID for translation(distance)
    //public double d_rotate;//D parameter of PID for rotation
    public double i_translate1;//I parameter of PID for translation(angle)
    public double i_translate2;//I parameter of PID for translation(distance)
    public double roomba_distance;
    //paramenter of 2nd loop of PID
    public double p_2loop;
    public double d_2loop;

    private int outputMaxAmount = 1000;

    private double angle_lastframe;
    private double angle_rotationlastframe;
    private double distance_lastframe;
    private double speed_lastframe = 0;
    private double angleSpd_lastframe = 0;
    private double angle_sum;
    private double distance_sum;

    private Vector3 target;
    private Vector3 target_Roomba;
    private Vector3 roomba;


    private bool start1;
    private bool switch1;//switch for translation
    private bool switch2;//swich for rotation
    //variable for translation

    private Vector3 walltoTarget;//the vector from wall to the target
    private double angleDirection;// the angle between walltoTarget and the forward direction of the wall
    private double movingdirection;

    //vararieble for second loop of PID
    private double spd_ref;
    private double angleSpd_ref;

    // Use this for initialization
    void Start () {
        c2 = GameObject.Find("Roomba").GetComponent<RoombaControllerScript>();

        //distance = 0;
        start1 = false;
        //target = new Vector3(target1.transform.position.x, 0, target1.transform.position.z);
        //target_Roomba = FindRoomba(target, Vector3.forward,roomba_distance);
        rend = wall.GetComponent<Renderer>();
        s_serial = new SerialPort(portName, portSpeed);
        try
        { s_serial.Open(); }
        finally
        {

        }

    }
	
	// Update is called once per frame
	void Update () {
        //send the location data to oscillscope
        double pos_x = wall.transform.position.x * 100;
        double pos_y = wall.transform.position.y * 100;
        double pos_z = wall.transform.position.z * 100;
        if (pos_x < 0)
        {
            pos_Data_Temp[0] = Convert.ToUInt16((Convert.ToUInt16(Math.Abs(pos_x)) ^ 0xffff) + 1);
        }
        else { pos_Data_Temp[0] = Convert.ToUInt16(pos_x); }

        if (pos_y < 0)
        {
            pos_Data_Temp[1] = Convert.ToUInt16((Convert.ToUInt16(Math.Abs(pos_y)) ^ 0xffff) + 1);
        }
        else { pos_Data_Temp[1] = Convert.ToUInt16(pos_y); }

        if (pos_z < 0)
        {
            pos_Data_Temp[2] = Convert.ToUInt16((Convert.ToUInt16(Math.Abs(pos_z)) ^ 0xffff) + 1);
        }
        else { pos_Data_Temp[2] = Convert.ToUInt16(pos_z); }

        if (pos_y < 0)
        {
            pos_Data_Temp[3] = Convert.ToUInt16((Convert.ToUInt16(Math.Abs(pos_y)) ^ 0xffff) + 1);
        }
        else { pos_Data_Temp[3] = Convert.ToUInt16(pos_y); }

        pos_Data = OutPut_Data(pos_Data_Temp, data_re);
        serial_write(pos_Data);



        /* if (Input.GetKeyUp(KeyCode.P))
         {
             start1 = !start1;
             switch1 = true;
             Vector3 wallposition = new Vector3(wall1.transform.position.x, 0, wall1.transform.position.z);
             roomba = FindRoomba(wallposition, wall1.transform.forward,roomba_distance);
             originaldistance = Vector3.Distance(roomba, target_Roomba);
             print("targetposition = " + target_Roomba);

         }

         if (start1)
         {
             //Initiation(switch1, start, wall1, target,Vector3.forward ,0);

             if (switch1)
                 switch1 = Translation(target_Roomba, wall1,0,switch1,p1,p2);
             switch2 = !switch1;
             if (switch2)
                 switch2 = Rotation(Vector3.forward, wall1,0,5f,d2,switch2 );

             if (!switch1 && !switch2)
             {
                 start1 = false;
                 rend.enabled = false;//make the wall reference disappeared after the initiation
             }

         }*/

    }

    /*public bool Initiation(Vector3 target, GameObject wall, int wallnum)//use to move the wall to the target position with the target direction
    {

        if (!move1over)
        {
            move1over = controller.Translation(targetVe3, wall, wallnum, true, p1, p2);
            move2over = !move1over;
        }
        if (!move2over)
        {
            move2over = controller.Rotation(Vector3.forward, wall, wallnum, p3, 120, true);
        }

        return move1over && move2over;
    }*/
    private int ampLimit(int vaule)
    {
        if (vaule > outputMaxAmount)
        {
            vaule = outputMaxAmount;
        }
        if (vaule < -outputMaxAmount)
            vaule = -outputMaxAmount;
        return vaule;
    }


    public double AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)//return the angle between two vectors
    {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }

    public static Vector2 IgnoreYAxisa(Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.z);
    }

    public double DistanceOfPointToVector(Vector3 direction, Vector3 startpoint, Vector3 point)//ditance from a point to line
    {
        Vector2 directionVe2 = IgnoreYAxisa(direction);
        Vector3 startpiontVe2 = IgnoreYAxisa(startpoint);
        double A = directionVe2.y;
        double B = -directionVe2.x;
        double C = directionVe2.x * startpiontVe2.y - directionVe2.y * startpiontVe2.x;
        double denominator = Math.Sqrt(A * A + B * B);
        Vector2 pointVe2 = IgnoreYAxisa(point);
        return Math.Abs((A * pointVe2.x + B * pointVe2.y + C) / denominator); ;
    }

    public bool Rotation(Vector3 targetdirection,GameObject wall,int wallnum ,double p1 ,double d, bool on)//angle recorrecting (p1 is the controlling variable of angle)
    {
        bool finished = false;
        int velocity_R;
        int velocity_L;
        double angle;//the angle between wall and target
        double angle_input;
        Vector3 target = new Vector3(targetdirection.x, 0, targetdirection.z);
        Vector3 wallposition = new Vector3(wall.transform.position.x, 0, wall.transform.position.z);
        angle = AngleSigned(wall.transform.forward, target, Vector3.up);
        //print("angle =" + angle);
        /*if (angle <= 20f)
        {
            angle_input = p1 * angle/2;
        }
        else
        {*/
        double angle_error = angle - angle_rotationlastframe;
        angle_rotationlastframe = angle;

        angle_input = p1 * angle + d * angle_error;
        angle_input = filter(angle_input);
        //print("d = " + angle_error );
        //}
        velocity_R = (int)(0 - angle_input);
        velocity_L = (int)(0 + angle_input);

        //amplitude limiting
        velocity_R = ampLimit(velocity_R);
        velocity_L = ampLimit(velocity_L);
        if (angle >= -0.5 && angle <= 0.5)
        {
            finished = true;
            c2.Stop(wallnum);
        }
         else
        {
            if (recodeflag)
            {
                c2.Move(velocity_R, velocity_L, wallnum);
                finished = false;
            }
        }

        return finished;
    }

    ////moving to target position with feedback controlling
    //input: 1.target position; 2.reference of the wall; 3.ID of the wall; 4.; 5. P parameter for angle; 6.P parameter for distance
    //output: return true when arrive the target
    public bool Translation_LR(Vector3 targetposition,GameObject wall,int wallnum,bool on,double p1,double p2)//moving to target position with feedback controlling
    {

        recodeflag = false;
        byte[] data2 = new byte[10] ;
        byte[] data3 = new byte[10] ;

        bool finished = true;
        double distance;
        double angle;
        int velocity_R;
        int velocity_L;
        double distance_input;
        double angle_input;
        double p_ang ;//controlling variable of angle
        double p_dis = p2;//controlling variable of distance
        Vector3 walltoTarget;//the vector from wall to the target
        double angleDirection;// the angle between walltoTarget and the forward direction of the wall
        int movingdirection;

        Vector3 target = new Vector3(targetposition.x, 0, targetposition.z);
        Vector3 wallposition = new Vector3(wall.transform.position.x, 0, wall.transform.position.z);
        Vector3 roombaposition = FindRoomba(wallposition, wall.transform.forward);
        walltoTarget = target - roombaposition;
        angleDirection = AngleSigned(walltoTarget, wall.transform.right, Vector3.up);
        if (angleDirection > -90 && angleDirection < 90)
            angle = AngleSigned(wall.transform.right, walltoTarget, Vector3.up);
        else
            angle = AngleSigned(-wall.transform.right, walltoTarget, Vector3.up);
        //print("angle =" + angle);
        distance = Vector3.Distance(roombaposition, target);
        //print("roombaposition1 = " + roombaposition + " targetroomba2 = " + target);
        //print("roombaposition = " + roombaposition);
        //print("distance ="+ distance);
        p_ang = p1;//* distance / originaldistance ;//make the angle paramenter small when the wall is close to the target
        //filt the angle signal
        double originAngle = angle;
        angle = filter(angle);
        //use the error of angle to achieve the D control
        double angle_error = angle - angle_lastframe;
        angle_lastframe = angle;
        //I control for angle
        angle_sum = angle_sum + angle;
        //print("angle_sum" + angle_sum);
        //print("angle = " + angle);
        angle_input = p_ang * angle + d1 * angle_error + angle_sum * i_translate1;
        //use the error of distance to achieve the D control
        double distance_error = distance - distance_lastframe;
        distance_lastframe = distance;
        if (angleDirection > -90 && angleDirection < 90)//when the moving direction is away from the target make the speed be minus
            movingdirection = 1;
        else
            movingdirection = -1;
        //I control for distance
        distance_sum = distance_sum + distance * movingdirection;
        //bang bang controll
        //if (distance <= 0.2f)
        //{
            distance_input = p_dis * distance * movingdirection + d2 * distance_error + i_translate2 * distance_sum;
            angle_input = p_ang * angle;
        //}
        /*else
        {
            distance_input = 1000 * movingdirection;
        }*/

        //*******************prepare for second loop of PID********************
        spd_ref = distance_input;
        print("spd_ref =" + spd_ref);
        angleSpd_ref = angle_input;
        distance_input = Translate_vel_dis(p_2loop, d_2loop);
        //*******************************************

        distance_input = ampLimit((int)distance_input);
        angle_input = ampLimit((int)angle_input);
        velocity_R = (int)(distance_input - angle_input);
        velocity_L = (int)(distance_input + angle_input);

        velocity_R = ampLimit(velocity_R);
        velocity_L = ampLimit(velocity_L);
       
        if (distance <= err_distance)
        {
            c2.Stop(wallnum);
            finished = true;
            
        }
        else
        {
            if (recodeflag)
            {
                //print("velocity R =" + velocity_R);
                //print("velocity L =" + velocity_L);
                c2.Move(velocity_R, velocity_L, wallnum);
            }
                finished = false;
            
        }

        // put the data need to be shown in oscillscope into data1[]
        if (/*distance * movingdirection*1000*/originAngle*10 < 0)
        {
            data1[0] = Convert.ToUInt16((Convert.ToUInt16(Math.Abs(originAngle*10/*distance * movingdirection*1000*/)) ^ 0xffff) + 1);
        }
        else
        { data1[0] = Convert.ToUInt16(/*distance * movingdirection*1000*/originAngle*10); }
        //print(distance);
        if (angle*10 < 0)
        {
            data1[1] = Convert.ToUInt16((Convert.ToUInt16(Math.Abs(angle * 10)) ^ 0xffff) + 1);
            //print(data1[1]);
        }
        else { data1[1] = Convert.ToUInt16(angle*10); }

        if (velocity_R < 0)
        {
            data1[2] = Convert.ToUInt16((Convert.ToUInt16(Math.Abs(velocity_R)) ^ 0xffff) + 1);
        }
        else { data1[2] = Convert.ToUInt16(velocity_R); }


        if (velocity_L < 0)
        {
            data1[3] = Convert.ToUInt16((Convert.ToUInt16(Math.Abs(velocity_L)) ^ 0xffff) + 1);
        }
        else { data1[3] = Convert.ToUInt16(velocity_L); }
        //Debug.Log(velocity_R);

        data3 = OutPut_Data(data1, data2);
        /*if(recodeflag)
            serial_write(data2);*/
        print(gameObject.name + " translating");
        return finished;
    }

    public bool Translation_FB(Vector3 targetposition, GameObject wall, int wallnum, bool on, double p1, double p2)//moving to target position with feedback controlling
    {
        recodeflag = false;
        byte[] data2 = new byte[10];
        byte[] data3 = new byte[10];

        bool finished = true;
        double distance;
        double angle;
        int velocity_R;
        int velocity_L;
        double distance_input;
        double angle_input;
        double p_ang;//controlling variable of angle
        double p_dis = p2;//controlling variable of distance
        Vector3 walltoTarget;//the vector from wall to the target
        double angleDirection;// the angle between walltoTarget and the forward direction of the wall
        int movingdirection;

        Vector3 target = new Vector3(targetposition.x, 0, targetposition.z);
        Vector3 wallposition = new Vector3(wall.transform.position.x, 0, wall.transform.position.z);
        Vector3 roombaposition = FindRoomba(wallposition, wall.transform.forward);
        walltoTarget = target - roombaposition;
        angleDirection = AngleSigned(walltoTarget, wall.transform.forward, Vector3.up);
        if (angleDirection > -90 && angleDirection < 90)
            angle = AngleSigned(wall.transform.forward, walltoTarget, Vector3.up);
        else
            angle = AngleSigned(-wall.transform.forward, walltoTarget, Vector3.up);
        //print("angle =" + angle);
        distance = Vector3.Distance(roombaposition, target);
        //print("roombaposition1 = " + roombaposition + " targetroomba2 = " + target);
        //print("roombaposition = " + roombaposition);
        //print("distance ="+ distance);
        p_ang = p1;//* distance / originaldistance ;//make the angle paramenter small when the wall is close to the target
        //filt the angle signal
        double originAngle = angle;
        angle = filter(angle);
        //use the error of angle to achieve the D control
        double angle_error = angle - angle_lastframe;
        angle_lastframe = angle;
        //I control for angle
        angle_sum = angle_sum + angle;
        //print("angle_sum" + angle_sum);
        //print("angle = " + angle);
        angle_input = p_ang * angle + d1 * angle_error + angle_sum * i_translate1;
        //use the error of distance to achieve the D control
        double distance_error = distance - distance_lastframe;
        distance_lastframe = distance;
        if (angleDirection > -90 && angleDirection < 90)//when the moving direction is away from the target make the speed be minus
            movingdirection = 1;
        else
            movingdirection = -1;
        //I control for distance
        distance_sum = distance_sum + distance * movingdirection;
        if (distance <= 0.2f)
        {
            distance_input = p_dis * distance * movingdirection + d2 * distance_error + i_translate2 * distance_sum;
            //angle_input = p_ang * angle;
        }
        else
        {
            distance_input = 1000 * movingdirection;
        }
        if (distance_input > 1000)//amplitude limiting
        {
            distance_input = 1000;
        }
        if (distance_input < -1000)
            distance_input = -1000;
        if (angle_input > 1000)
        {
            angle_input = 1000;
        }
        if (angle_input < -1000)
            angle_input = -1000;
        velocity_R = (int)(distance_input - angle_input);
        velocity_L = (int)(distance_input + angle_input);

        if (velocity_R > 1000)//amplitude limiting
        {
            velocity_R = 1000;
        }
        if (velocity_R < -1000)
            velocity_R = -1000;
        if (velocity_L > 1000)
        {
            velocity_L = 1000;
        }
        if (velocity_L < -1000)
            velocity_L = -1000;
        if (distance <= err_distance)
        {
            c2.Stop(wallnum);
            finished = true;

        }
        else
        {
            if (recodeflag)
            {
                //print("velocity R =" + velocity_R);
                //print("velocity L =" + velocity_L);
                c2.Move(velocity_R, velocity_L, wallnum);
            }
            finished = false;

        }
        // put the data need to be shown in oscillscope into data1[]
        if (/*distance * movingdirection*1000*/originAngle * 10 < 0)
        {
            data1[0] = Convert.ToUInt16((Convert.ToUInt16(Math.Abs(originAngle * 10/*distance * movingdirection*1000*/)) ^ 0xffff) + 1);
        }
        else
        { data1[0] = Convert.ToUInt16(/*distance * movingdirection*1000*/originAngle * 10); }
        //print(distance);
        if (angle * 10 < 0)
        {
            data1[1] = Convert.ToUInt16((Convert.ToUInt16(Math.Abs(angle * 10)) ^ 0xffff) + 1);
            //print(data1[1]);
        }
        else { data1[1] = Convert.ToUInt16(angle * 10); }

        if (velocity_R < 0)
        {
            data1[2] = Convert.ToUInt16((Convert.ToUInt16(Math.Abs(velocity_R)) ^ 0xffff) + 1);
        }
        else { data1[2] = Convert.ToUInt16(velocity_R); }


        if (velocity_L < 0)
        {
            data1[3] = Convert.ToUInt16((Convert.ToUInt16(Math.Abs(velocity_L)) ^ 0xffff) + 1);
        }
        else { data1[3] = Convert.ToUInt16(velocity_L); }
        //Debug.Log(velocity_R);

        data3 = OutPut_Data(data1, data2);
        /*if(recodeflag)
            serial_write(data2);*/
        return finished;
    }

    // 2nd LOOP of PID output of distance
    public double Translate_vel_dis(double p_dis, double d_dis)
    {
        SteamVR_TrackedObject tracker = gameObject.transform.parent.GetComponent<SteamVR_TrackedObject>();
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)tracker.index);
        double spd_err = spd_ref - device.velocity.magnitude*1000;
        double spd_d = device.velocity.magnitude*1000 - speed_lastframe;
        speed_lastframe = device.velocity.magnitude*1000;
        //print("speed_d =" + spd_d);
        double output = spd_err * p_dis + spd_d * d_dis;
        return output;

    }
    //2nd LOOP of PID output of angle
    public double Translate_vel_angle(double p_dis, double d_dis)
    {
        SteamVR_TrackedObject tracker = gameObject.transform.parent.GetComponent<SteamVR_TrackedObject>();
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)tracker.index);
        double angle_err = angleSpd_ref - device.angularVelocity.magnitude;
        /*double spd_d = device.velocity.magnitude - speed_lastframe;
        speed_lastframe = device.velocity.magnitude;
        //print("speed_d =" + spd_d);
        double output = spd_err * p_dis + spd_d * d_dis;*/
        return 0;
    }

    // inpput:1.the position of the wall 2.the direction the wall is facing 3.the distance from roomba to wall
    // output:the position of the roomba which controll the wall

    public Vector3 FindRoomba(Vector3 wallposition, Vector3 walldirection)
    {
        Vector2 directionVe2 = IgnoreYAxisa(-walldirection);
        Vector2 wallVe2 = IgnoreYAxisa(wallposition);
        double a = directionVe2.x;
        double b = directionVe2.y;
        double x1 = a * roomba_distance / Math.Sqrt(a * a + b * b) + wallVe2.x;
        double x2 = b * roomba_distance / Math.Sqrt(a * a + b * b) + wallVe2.y;
        //GameObject roomba =  Instantiate(roomba_reference,new Vector3(x1, 0, x2),transform.rotation);//show the position of the roomba
        //roomba.transform.SetParent(wall1.transform, true);
        return new Vector3((float)x1, 0,(float) x2);

    }

    public void SetErrDistance(double err)
    {
        err_distance = err;
    }

    // convert to oscillscope data
    public byte[] OutPut_Data(ushort[] OutData, byte[] datarecord)
    {

        int CRC_Temp;
        int sb = 255;
        int aa = 0;
        byte[] databuf = new byte[10];
        byte i, j;
        ushort CRC16 = 0;

        for (i = 0; i < 3; i++)
        {

            databuf[i * 2] = Convert.ToByte(OutData[i] & sb);
            databuf[i * 2 + 1] = Convert.ToByte(OutData[i] >> 8);
        }

        ///////add
        CRC_Temp = 0xffff;
        int dsb = 1;
        for (i = 0; i < 8; i++)
        {
            CRC_Temp ^= databuf[i];
            for (j = 0; j < 8; j++)
            {
                if (Convert.ToBoolean(CRC_Temp & dsb))
                    CRC_Temp = (CRC_Temp >> 1) ^ 0xa001;
                else
                    CRC_Temp = CRC_Temp >> 1;
            }
        }
        //////
        CRC16 = (ushort)CRC_Temp;
        //print(CRC16);
        databuf[8] = Convert.ToByte(CRC16 & 0xff);
        databuf[9] = Convert.ToByte(CRC16 >> 8);
        for (i = 0; i < 10; i++)
        {
            datarecord[i] = databuf[i];

        }
        return datarecord;
    }
    // output form port_name with port_speed
    public void serial_write(byte[] data)
    {
        //s_serial.Write(data, 0, data.Length);
    }

    public double kalman(double[] z_k)
    {

        double Q = 0.027;
        double R = 0.1;
        y_k[0] = 0;  //给定x(k-1|k-1)初值，随便给的。
        double p_k_k_1;
        double p_k_k = 5;//给定p(k-1|k-1)初值，随便给的，但不能给0。
        double x_k_k = 2;  //系统最优估计值.给定x(k-1|k-1)初值，随便给的。
        double x_k_k_1;
        double Kg_k;  //卡尔曼增益
        for (int i = 0; i < 3; i++) //从第2个值开始预测
        {
            y_k[i] = x_k_k;  //公式1。这个也是我们要的结果。

            x_k_k_1 = x_k_k; //公式1。这里开始迭代了
            p_k_k_1 = p_k_k + Q;  //公式2。

            Kg_k = p_k_k_1 / (p_k_k_1 + R);  //公式4
            x_k_k = x_k_k_1 + Kg_k * (z_k[i + 1] - x_k_k_1);  //公式3

            p_k_k = (1 - Kg_k) * p_k_k_1;  //公式5
        }
        return y_k[2];
    }

    public double filter(double x)
    {
        xx_k[3 - n] = x;
        n++;
        //print("x[0] =" + xx_k[0]);
        if (n == 4)
        {
            n = 0;
            
           x = double.Parse(kalman(xx_k).ToString());
           //print("kalman output = " + kalman(xx_k));
            recodeflag = true;
        }
        
        return x;
    }
}
