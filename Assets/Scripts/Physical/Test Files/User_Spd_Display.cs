using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class User_Spd_Display : MonoBehaviour
{
    private Text txt;
    public SteamVR_TrackedObject user_tracker;
    private int count_frame = 0;
    private float user_spd_average = 0;
    private float user_spd_ave_3s = 0;
    private float spd_sum = 0;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        txt = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        /*SteamVR_Controller.Device device = SteamVR_Controller.Input((int)user_tracker.index);
        Vector3 vec = device.velocity;
        vec.y = 0;
        float spd = vec.magnitude;
        if(timer >= 3f)
        {
            user_spd_average = spd_sum / count_frame;
            txt.text = user_spd_average.ToString();
            timer = 0;
            if(count_frame != 0)
                user_spd_ave_3s = user_spd_average;
            user_spd_average = 0;
            count_frame = 0;
            spd_sum = 0;
        }*/

        /*if (spd > 0.2f)
        {
            count_frame++;
            spd_sum += spd;
            
        }*/
        //txt.text = user_spd_average.ToString();
        
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
            txt.text = user_spd_average.ToString();
            //timer = 0;
            if (count_frame != 0)
                user_spd_ave_3s = user_spd_average;
            user_spd_average = 0;
            count_frame = 0;
            spd_sum = 0;
        }
        //timer += Time.deltaTime;

        if (spd > 0.2f)
        {
            count_frame++;
            spd_sum += spd;            
        }
    }

    public float GetUserSpd()
    {
        return user_spd_ave_3s;
    }

}
