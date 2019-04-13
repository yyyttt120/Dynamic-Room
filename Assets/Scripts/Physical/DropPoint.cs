using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPoint : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;
    public GameObject droppedPointPrefab; //This is our dot prefab
    public int delay = 20; //Using a simple count up mechanism instead of a coroutine or invokerepeating because I'm lazy and the point is to get this done quickly
    private int delayCounter = 0;
    private Transform dropper;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Update()
    {
        if (Controller.GetHairTrigger())
        { //If we've pressed the trigger
            if (delayCounter <= 0)
            { //If our timer is up
                dropper = transform.GetChild(0);
                Instantiate(droppedPointPrefab, dropper.position, dropper.rotation); //Drop a ball in place
                delayCounter = delay; //Reset our delay counter
            }
            else
            {
                delayCounter--;
                Debug.Log(delayCounter);
            }
        }
        else
        {
            delayCounter = 0;
        }
    }
}
