using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using System.Text;

public class FindRightTracker : MonoBehaviour
{
    private SteamVR_TrackedObject[] trackers;
    public string serialID;
    private int index;
    private int timer = 0;
    private bool startChangeIndex = false;
    public bool resetAxis = false;
    private int tries;
    // Start is called before the first frame update
    void Start()
    {
        trackers = new SteamVR_TrackedObject[6];
        GameObject cameraRig = GameObject.Find("[CameraRig]");
        //put all the trackers into a array
        for(int i = 0; i < 6; i++)
        {
            trackers[i] = cameraRig.transform.GetChild(i).GetComponent<SteamVR_TrackedObject>();
            print($"tracker = {trackers[i].name}");
        }
        //index = (int)transform.parent.GetComponent<SteamVR_TrackedObject>().index;
    }

    // Update is called once per frame
    void Update()
    {
        //reset the axis of this object
        if (resetAxis)
        {
            transform.rotation = Quaternion.LookRotation(transform.up,transform.forward);
            resetAxis = false;
        }

        if(timer < 120)
            timer++;
        /*if(timer == 300)
        {
            index = (int)transform.parent.GetComponent<SteamVR_TrackedObject>().index;
            startChangeIndex = true;
            print($"record index [{gameObject.transform.parent.name}] = {index}");
        }*/
        if (/*startChangeIndex */ timer > 100 && tries <12)
        {
            if (GetSerialID(transform.parent.GetComponent<SteamVR_TrackedObject>()) != serialID)
            {
                //int index = (int)transform.parent.GetComponent<SteamVR_TrackedObject>().index;
                transform.parent.GetComponent<SteamVR_TrackedObject>().SetDeviceIndex(tries);
                tries++;
                /*foreach (SteamVR_TrackedObject tracker in trackers)
                {
                    if (GetSerialID(tracker.GetComponent<SteamVR_TrackedObject>()) == serialID)
                    {
                        print($"{transform.parent.name} find trakcer");
                        tracker.SetDeviceIndex(index);
                        break;
                    }
                }*/
            }
        }
    }
    private string GetSerialID(SteamVR_TrackedObject tracker)
    {
        uint index = (uint)tracker.index;
        ETrackedPropertyError error = new ETrackedPropertyError();
        StringBuilder sb = new StringBuilder();
        OpenVR.System.GetStringTrackedDeviceProperty(index, ETrackedDeviceProperty.Prop_SerialNumber_String, sb, OpenVR.k_unMaxPropertyStringSize, ref error);
        var probablyUniqueDeviceSerial = sb.ToString();
        print($"serial number = {probablyUniqueDeviceSerial}");
        return probablyUniqueDeviceSerial;
    }
}
