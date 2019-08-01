using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* the block which is a unit room with four walls for our four-class ML classifier */
public class ML_Block : MonoBehaviour
{
    public GameObject[] walls;// the 4 walls of the unit room, the order should be up, down, left, right
    public bool userEntered { get; set; }//turn true, when the user enter this block
    /*     boundary of the block        */
    public float max_x;
    public float max_z;
    public float min_x;
    public float min_z;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        userEntered = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.CompareTo("Hero") == 0)
        {
            userEntered = true;
        }
    }

}
