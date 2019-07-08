using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedArrow : MonoBehaviour
{
    public SteamVR_TrackedObject user_tracker;
    private Glint glint;

    private int count_frame = 0;
    private int count_silent_frame = 0;//count the frame which user keep statinary
    private float user_spd_average = 0;
    private float user_spd_ave_3s = 0;
    private float spd_sum = 0;
    // Start is called before the first frame update
    void Start()
    {
        glint = gameObject.GetComponent<Glint>();
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    private void FixedUpdate()
    {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)user_tracker.index);
        Vector3 vec = device.velocity;
        vec.y = 0;
        float spd = vec.magnitude;
        if (/*timer >= 3f*/count_frame >= 200)
        {
            user_spd_average = spd_sum / count_frame;
            
            //txt.text = user_spd_average.ToString();
            //timer = 0;
            /*if (count_frame != 0)
                user_spd_ave_3s = user_spd_average;*/
            if (user_spd_average > 0.4f)
            {
                glint.StartGlinting();
            }
            else
                glint.StopGlinting();
            user_spd_average = 0;
            count_frame = 0;
            count_silent_frame = 0;
            spd_sum = 0;
        }
        if(count_silent_frame > 100)
        {
            glint.StopGlinting();
            count_silent_frame = 0;
        }
        //timer += Time.deltaTime;

        if (spd > 0.2f)
        {
            count_frame++;
            spd_sum += spd;
        }
        else
            count_silent_frame++;
        
    }
}
