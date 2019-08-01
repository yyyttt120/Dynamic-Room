using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class Evalu_Data_Writer : MonoBehaviour
{
    private FileStream fs_wait_time;
    private FileStream fs_accur;
    public SpeedSupervisor speedsupervisor;

    int count_touch = 0;
    int count_encounterd_touch = 0;//count how many time the user touch virtual wall and encounterd by the robotic wall
    // Start is called before the first frame update
    void Start()
    {
        string path = Application.dataPath + "/Data/timer.txt";
        fs_wait_time = File.OpenWrite(path);
        path = Application.dataPath + "/Data/accuracy.txt";
        fs_accur = File.OpenWrite(path);
    }

    // Update is called once per frame
    void Update()
    {
        //print($"touch count = {count_touch}; encounterd touch count = {count_encounterd_touch}");
    }
    private void OnDestroy()
    {
        fs_wait_time.Close();
        fs_wait_time.Dispose();
        string accuracy_str = $"touch count = {count_touch}; encounterd touch count = {count_encounterd_touch}";
        WriteData(accuracy_str, fs_accur);
        fs_accur.Close();
        fs_accur.Dispose();
    }

    public void AddTouch()
    {
        count_touch++;
    }
    public void AddEncounterdTouch()
    {
        count_encounterd_touch++;
    }
    public void WriteData(string output)
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
        float user_spd = speedsupervisor.user_spd_average;
        sb.Append(output + $" {user_spd}");
        sb.AppendLine();
        //Using system.Text
        byte[] map = Encoding.UTF8.GetBytes(sb.ToString());
        fs_wait_time.Write(map, 0, map.Length);

    }
    public void WriteData(string output,FileStream fs)
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
