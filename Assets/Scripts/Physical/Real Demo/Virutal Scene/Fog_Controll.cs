using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attached onto the window door which could be open
public class Fog_Controll : MonoBehaviour {

    private Vector3 closePos;
    private bool windowOpened;
    public GameObject mylight;
	// Use this for initialization
	void Start () {
        closePos = transform.localPosition;
        //mylight = GameObject.Find("Light");
	}
	
	// Update is called once per frame
	void Update () {
        print($"window open = {Mathf.Abs(transform.localPosition.x - closePos.x)}");
        if (Mathf.Abs(transform.localPosition.x - closePos.x) > 0.5f)
            windowOpened = true;
        else
            windowOpened = false;
        // when light is opend, turn the fog into gray
        Color color = Color.gray;
        /*if(mylight.activeSelf)
            RenderSettings.fogColor = color;
        RenderSettings.fog = true;*/
        /*if (!windowOpened)
            RenderSettings.fogDensity = 0.9f;
        else
            FogFade(0.003f);*/
        if (windowOpened)
            FogFade(0.006f);

    }

    private void FogFade(float speed)
    {
        print("fog fading *************");
        RenderSettings.fogDensity = RenderSettings.fogDensity - speed;
        if (RenderSettings.fogDensity < 0.1)
            RenderSettings.fogDensity = 0.1f;
        /*for(int i = 0; i < 8; i++)
        {
            mylight.transform.GetChild(i).GetChild(0).GetComponent<Light>().range = 0.7f;
        }*/
    }

}
