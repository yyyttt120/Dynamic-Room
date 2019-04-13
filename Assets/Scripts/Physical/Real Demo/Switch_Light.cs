using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// attach onto the collider box of light switch
public class Switch_Light : MonoBehaviour {
    private int count;
    public bool switcher = false;
    public GameObject light;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(count > 50 && count < 200)
        {
            switcher = !switcher;
            count = 200;
        }
        LightOn(switcher);
	}

    private void LightOn(bool sw)
    {
        if (sw)
        {
            if (RenderSettings.fog)
            {
                if(SceneManager.GetActiveScene().buildIndex != 0)
                    RenderSettings.fog = false;
                light.SetActive(true);
            }
        }
        else
        {
            if (!RenderSettings.fog)
            {
                RenderSettings.fog = true;
                light.SetActive(false);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag.CompareTo("Hand") == 0)
        {
            count += 1;
        }
        //LightOn(switcher);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.CompareTo("Hand") == 0)
        {
            count = 0;
        }
        //LightOn(switcher);
    }
}
