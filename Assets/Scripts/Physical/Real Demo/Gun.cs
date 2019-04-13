using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attach onto gun gameobject which is under contrller
public class Gun : Shoot {
    SteamVR_TrackedObject controller;
    GameObject trigger;
	// Use this for initialization
	void Start () {
        controller = this.transform.parent.GetComponent<SteamVR_TrackedObject>();
        trigger = transform.GetChild(0).gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)controller.index);
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            //print("shoot");
            Quaternion shoot = Quaternion.LookRotation(gunPoint.transform.forward, gunPoint.transform.up);
            Instantiate(bullet, gunPoint.transform.position,shoot);
            PlayGunShut();
        }

        trigger.transform.localPosition = new Vector3(trigger.transform.localPosition.x, trigger.transform.localPosition.y, device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x);
	}

}
