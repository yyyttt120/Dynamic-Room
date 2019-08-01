using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSupervisor : MonoBehaviour
{
    public SteamVR_TrackedObject user_tracker;
    public AudioSource[] threatens;
    private GameObject nautyButton;
    private int count_frame = 0;
    private int count_silent_frame = 0;//count the frame which user keep statinary
    public float user_spd_average = 0;
    private float user_spd_ave_3s = 0;
    private float spd_sum = 0;
    private bool flag = false;//the flag to record have we played the audio clips
    private bool silent = false;// user is keeping silent
    private bool gameover = false;

    // Start is called before the first frame update
    void Start()
    {
        threatens = GetComponentsInChildren<AudioSource>();
        nautyButton = GameObject.Find("NautyButton");
    }

    private void FixedUpdate()
    {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)user_tracker.index);
        Vector3 vec = device.velocity;
        vec.y = 0;
        float spd = vec.magnitude;
        if (/*timer >= 3f*/count_frame >= 100)
        {
            
            user_spd_average = spd_sum / count_frame;
            
            //txt.text = user_spd_average.ToString();
            //timer = 0;
            /*if (count_frame != 0)
                user_spd_ave_3s = user_spd_average;*/
            if (user_spd_average > 0.4f)
            {
                if (!flag)
                {
                    if (!gameover)
                    {
                        threatenStart();
                        flag = true;
                    }
                }
            }
            else
                flag = false;
                
            //user_spd_average = 0;
            count_frame = 0;
            count_silent_frame = 0;
            spd_sum = 0;
        }
        if (count_silent_frame > 100)
        {
            silent = true;
            count_silent_frame = 0;
            flag = false;
        }
        else
            silent = false;
        //timer += Time.deltaTime;

        if (spd > 0.2f)
        {
            count_frame++;
            spd_sum += spd;
        }
        else
            count_silent_frame++;

        gameover = nautyButton.GetComponent<RandomSwitch>().gameover;
    }

    private void threatenStart()
    {
        int i = Random.Range(1, threatens.Length);
        //if(!silent)
            threatens[i].Play();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
