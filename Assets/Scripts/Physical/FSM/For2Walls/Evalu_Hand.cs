using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attached onto the hand of the user avatar
public class Evalu_Hand : MonoBehaviour
{
    public SteamVR_TrackedObject controller;
    private Evalu_Data_Writer evaluator;
    private bool rWall_Touched = false;
    private bool vWall_Touched = false;
    // Start is called before the first frame update
    void Start()
    {
        evaluator = GameObject.Find("Evalu_Data_Writer").GetComponent<Evalu_Data_Writer>();
    }

    // Update is called once per frame
    void Update()
    {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)controller.index);
        if (device.GetHairTriggerDown())
        {
            if(vWall_Touched)
                evaluator.AddTouch();
            if(vWall_Touched && rWall_Touched)
                evaluator.AddEncounterdTouch();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.CompareTo("Robotic_Wall") == 0)
        {
            rWall_Touched = true;
            //evaluator.AddEncounterdTouch();
        }
        if (other.tag.CompareTo("Wall") == 0)
        {
            //evaluator.AddTouch();
            vWall_Touched = true;
            /*if (rWall_Touched)
                evaluator.AddEncounterdTouch();*/
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag.CompareTo("Robotic_Wall") == 0)
        {
            rWall_Touched = true;
        }

        if (other.tag.CompareTo("Wall") == 0)
        {
            vWall_Touched = true;

            /*if (rWall_Touched)
                evaluator.AddEncounterdTouch();*/
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.CompareTo("Robotic_Wall") == 0)
        {
            rWall_Touched = false;
        }

        if (other.tag.CompareTo("Wall") == 0)
        {
            //evaluator.AddTouch();
            vWall_Touched = false;
            /*if (rWall_Touched)
                evaluator.AddEncounterdTouch();*/
        }
    }
}
