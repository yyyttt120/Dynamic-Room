using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attached onto the user encounter reference
//detect the wall condition, when a wall is needed, post a request to FSMSystem
public class Wall_Requester : MonoBehaviour {
    public FSMSystem statesController;
    public GameObject user;
    private List<GameObject> requestWallList;
    private List<GameObject> solvedWallList;
    private GameObject releasedWall;
    private Vector3 colliderSize;
    private Vector3 colliderCenter;
    public SteamVR_TrackedObject user_tracker = null;

    // Use this for initialization
    void Start() {
        //requestWallList = new List<GameObject>();
        solvedWallList = new List<GameObject>();
        colliderSize = gameObject.GetComponent<BoxCollider>().size;
        colliderCenter = gameObject.GetComponent<BoxCollider>().center;
    }

    // Update is called once per frame
    void Update() {
        /*foreach(GameObject wall in requestWallList)
        {
            if (statesController.Allocate_wall(wall))
            {
                requestWallList.Remove(wall);
                solvedWallList.Add(wall);
            }
        }*/
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)user_tracker.index);

        this.gameObject.transform.position = user.transform.position;
        this.gameObject.transform.eulerAngles = new Vector3(0, user.transform.eulerAngles.y, 0);

        //adjust the size of collider to based on the user's velocity
        float angle = AngleSigned(transform.forward, device.velocity, Vector3.up);
        if (angle < 90 || angle > -90)
        {
            gameObject.GetComponent<BoxCollider>().size = colliderSize + device.velocity.magnitude * new Vector3(0, 0, 20f);
            gameObject.GetComponent<BoxCollider>().center = colliderCenter + device.velocity.magnitude * new Vector3(0, 0, 20f)/2;
        }

        //print("requested wall =" + requestWallList[0]);
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.CompareTo("Wall") == 0)
        {
            //print("wall in:" + other.name);
            //make sure the encountered wall woulden't be released
            if (releasedWall == other.gameObject)
                releasedWall = null;
            //check if this wall isn't covered by other walls
            Slider_Controller slider_Con = other.gameObject.GetComponent<Slider_Controller>();
            if (other.gameObject.transform.GetChild(0).gameObject.activeSelf)
            {
                if (!solvedWallList.Contains(other.gameObject))
                {
                    Animator states = statesController.Allocate_wall(other.gameObject);
                    //solvedWallList.Add(other.gameObject);
                    int counter = states.GetInteger("NearWallCounter") + 1;
                    if (counter > 100)
                        counter = 100;
                    states.SetInteger("NearWallCounter", counter);
                }
            }

            /*if(states != null)
            {
                int counter = states.GetInteger("NearWallCounter") + 2;
                if (counter > 100)
                    counter = 100;
                states.SetInteger("NearWallCounter", counter);
            }*/

           
        }
    }
    //add the wall into sloved list
    public void SetWallSolved(GameObject wall)
    {
        if (!solvedWallList.Contains(wall))
        {
            solvedWallList.Add(wall);
        }
        else
            Debug.Log("err: target wall " + wall.name + " is already in solved list");
    }

    public void ReleaseWall(GameObject wall)
    {
        if (solvedWallList.Contains(wall))
        {
            solvedWallList.Remove(wall);
        }
        else
            Debug.Log("err: target wall is not in solved list");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.CompareTo("Wall") == 0)
        {
            //print("wall out:" + other.name);
            releasedWall = other.gameObject;
            
        }

    }

    public GameObject GetReleasedWall()
    {
        return releasedWall;
    }

    public List<GameObject> GetSolvedList()
    {
        return solvedWallList;
    }

    public float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)//return the angle between two vectors
    {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }
}
