using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSwitch : MonoBehaviour
{
    public GameObject[] VWalls;
    public AudioSource gameComplete;
    public AudioSource buttonTouched;
    public AudioSource buttonFinished;
    public GameObject lights;
    public bool gameover = false;
    private int counter = 0;
    private int touchCount = 0;
    private int lastWallID = 0;
    // Start is called before the first frame update
    void Start()
    {
        lights.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(counter > 100)
        {
            counter = 0;
            if (touchCount < 5)
            {
                buttonTouched.Play();
                Blink();
            }
            else
            {
                buttonFinished.Play();
                gameComplete.Play();
                lights.SetActive(true);
                gameover = true;
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag.CompareTo("Hand") == 0)
        {
            counter++;
        }
    }

    private void Blink()
    {
        touchCount++;
        int wallID = Random.Range(0, 3);
        while (wallID == lastWallID)
        {
            wallID = Random.Range(0, 3);
        }
        lastWallID = wallID;
        float pos_x = Random.Range(-0.4f, 0.4f);
        float pos_y = Random.Range(-0.4f, 0.4f);
        GameObject wall = VWalls[wallID];
        pos_x = pos_x * wall.GetComponent<Renderer>().bounds.size.x + wall.transform.position.x;
        transform.position = new Vector3(pos_x, transform.position.y, wall.transform.position.z)- wall.transform.forward * 0.03f;
        transform.rotation = Quaternion.LookRotation(-wall.transform.up, wall.transform.forward);
    }
}
