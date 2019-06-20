using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class DataCatcher : MonoBehaviour
{
    public Robotic_Wall[] rWalls;
    public VirtualWall[] virtualWalls;
    public GameObject user;
    public SteamVR_TrackedObject user_tracked;
    public SteamVR_TrackedObject left_;
    public ServerObject dataRequester;

    public AudioSource notice;
    private float timer1 = 0;
    // Start is called before the first frame update
    //FileStream fs;
    float time = 0;
    float timer = 0;
    string vwallInfo = "";
    int target = 0;

    StringBuilder data_input;//build the data set for python
    List<string> infoList;//store the information for every sample

    int count;//when it's 20, a input data set is ready
    void Start()
    {
        //GameObject[] temp;
        data_input = new StringBuilder("");
        //virtualWalls = new List<VirtualWall>();
        //temp = GameObject.FindGameObjectsWithTag("Wall");
        /*for (int i = 0; i < temp.Length; i++)
            virtualWalls.Add(temp[i].GetComponent<VirtualWall>());*/
        foreach (Robotic_Wall wall_r in rWalls)
            wall_r.Set_Robotic_Wall(wall_r.gameObject);
        infoList = new List<string>();
        //*****************************collect data for training**************************
       /* //第一步访问Txt文件
        string path = Application.dataPath + "/Data/1.txt";
        //文件流
        fs = File.OpenWrite(path);
        string title = "time user_position.x user_position.y user_position.z user_forward.x user_forward.y user_forward.z user_velocity.x user_velocity.y user_velocity.z user_angle_velocity.x user_angle_velocity.y user_angle_velocity.z wall1_pos.x wall1_pos.y wall1_pos.z wall1_forward.x wall1_forward.y wall1_forward.z wall1_ip1_pos.x wall1_ip1_pos.y wall1_ip1_pos.z wall1_ip2_pos.x wall1_ip2_pos.y wall1_ip2_pos.z wall2_pos.x wall2_pos.y wall2_pos.z wall2_forward.x wall2_forward.y wall2_forward.z wall2_ip1_pos.x wall2_ip1_pos.y wall2_ip1_pos.z wall2_ip2_pos.x wall2_ip2_pos.y wall2_ip2_pos.z wall3_pos.x wall3_pos.y wall3_pos.z wall3_forward.x wall3_forward.y wall3_forward.z wall3_ip1_pos.x wall3_ip1_pos.y wall3_ip1_pos.z wall3_ip2_pos.x wall3_ip2_pos.y wall3_ip2_pos.z wall4_pos.x wall4_pos.y wall4_pos.z wall4_forward.x wall4_forward.y wall4_forward.z wall4_ip1_pos.x wall4_ip1_pos.y wall4_ip1_pos.z wall4_ip2_pos.x wall4_ip2_pos.y wall4_ip2_pos.z target";
        WriteData(title);*/
    }

    private void FixedUpdate()
    {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)user_tracked.index);
        time += Time.deltaTime;
        timer += Time.deltaTime;
        vwallInfo = GetVWallInfo();
        //sort all the virtual walls according to the value(descending)
        /*virtualWalls.Sort(delegate (VirtualWall a, VirtualWall b)
        {
            if (a.value > b.value)
                return -1;
            else
                return 1;
        });
        CleanList();*/

        timer1 += Time.deltaTime;
        /*if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            fs.Close();
            fs.Dispose();
        }*/
        target = UpdateTarget();
        print("button =" + target);
        string output = time + " " + VectortoString(user.transform.position) + " " + VectortoString(user.transform.forward) + " " + VectortoString(device.velocity) + " "+VectortoString(device.angularVelocity) + " " + vwallInfo + target;
        string input = time + " " + VectortoString(user.transform.position) + " " + VectortoString(user.transform.forward) + " " + VectortoString(device.velocity) + " " + VectortoString(device.angularVelocity) + " " + vwallInfo;
        //string input = time + "*" + VectortoString(user.transform.position) + "*" + VectortoString(user.transform.forward) + "*" + VectortoString(device.velocity) + "*" + VectortoString(device.angularVelocity) + "*" + vwallInfo;
        if (timer > 0.05f)
        {
            //WriteData(output);
            timer = 0;
            if (infoList.Count < 20)
                infoList.Add(input);
            else
            {
                infoList.RemoveAt(0);
                infoList.Add(input);
            }
            //count++;
            //data_input.Append(input);
        }

        //*****************************collect data for training**************************
        /*if (timer1 > 3)
        {
            timer1 = 0;
            notice.Play();
            WriteData("");
        }*/
        string inputStr = null;
        if (infoList.Count == 20)
        {
            //count = 0;
            foreach (string sample in infoList)
                data_input.Append(input);
            inputStr = data_input.ToString();
            inputStr = inputStr.Remove(inputStr.Length - 1, 1);
            //string result = py.UsePython(inputStr);
            //print(result);
            //print(inputStr);
            data_input.Clear();
        }
        dataRequester._Response = inputStr;

    }


    // Update is called once per frame
    void Update()
    {
        
    }


    private string VectortoString(Vector3 vec)
    {
        string temp = string.Format("{0:N6}", vec.x) + " " + string.Format("{0:N6}", vec.y) + " " + string.Format("{0:N6}", vec.z);
        return temp;
    }
    
    private int UpdateTarget()
    {
        SteamVR_Controller.Device left = SteamVR_Controller.Input((int)left_.index);
        int a = 0;
        if (left.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 touchpad = (left.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));
            //up
            if (touchpad.y > 0.5f)
                a = 1;
            //down
            if (touchpad.y < -0.5f)
                a = 2;
            //left
            if (touchpad.x < -0.5f)
                a = 3;
            //right
            if (touchpad.x > 0.5f)
                a = 4;
        }
        /*if (left.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
            WriteData("");*/
        return a;
    }

    private string GetVWallInfo()
    {
        string info = "";
        foreach(VirtualWall vwall in virtualWalls)
        {
            info += vwall.GetInfo() + " ";
        }
        return info;
    }

    //from the 3rd valuable virtual wall, every wall release its' matching robotic wall
    /*private void CleanList()
    {
        if(virtualWalls.Count > 2)
        {
            for(int i = 2; i < virtualWalls.Count; i++)
            {
                virtualWalls[i].ReleaseRWall();
            }
        }
    }*/

    private void Allocate()
    {
        
    }
    

    private void WriteData(string output,FileStream fs)
    {
        
        //第二步填充内容
        StringBuilder sb = new StringBuilder();
        /*for (int i = 0; i < 35; i++)
        {
            for (int j = 0; j < 35; j++)
            {
                sb.Append(Random.Range(0, 3));
            }
            sb.AppendLine();
        }*/
        sb.Append(output);
        sb.AppendLine();
        //Using system.Text
        byte[] map = Encoding.UTF8.GetBytes(sb.ToString());
        fs.Write(map, 0, map.Length);
    
    }
}
