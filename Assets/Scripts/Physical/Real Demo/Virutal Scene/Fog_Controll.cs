using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attached onto the window door which could be open
public class Fog_Controll : MonoBehaviour {

    private Vector3 closePos;
    private bool windowOpened;
    private GameObject light;
	// Use this for initialization
	void Start () {
        closePos = transform.position;
        light = GameObject.Find("Light");
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.x - closePos.x > 0.3f)
            windowOpened = true;
        else
            windowOpened = false;
        // when light is opend, turn the fog into gray
        Color color = Color.gray;
        if(light.activeSelf)
            RenderSettings.fogColor = color;
        RenderSettings.fog = true;
        if (!windowOpened)
            RenderSettings.fogDensity = 0.9f;
        else
            FogFade(0.003f);
	}

    private void FogFade(float speed)
    {
        RenderSettings.fogDensity = RenderSettings.fogDensity - speed;
        if (RenderSettings.fogDensity < 0)
            RenderSettings.fogDensity = 0;
    }

}
