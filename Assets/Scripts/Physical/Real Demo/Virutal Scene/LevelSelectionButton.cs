using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionButton : MonoBehaviour {
    public int level;
    public Material on;
    public Material off;
    public AudioSource ele_arrive;
    private bool buttonOn = false;//if the button is lit up
	// Use this for initialization
	void Start () {
        off = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        //if the elevator arrive the target level, button off
        if (SceneManager.GetActiveScene().buildIndex == level - 1)
        {
            if (buttonOn)
                ele_arrive.Play();
            buttonOn = false;
            //level++;
        }
        //implementate the button
        if (buttonOn)
        {
            //GetComponent<Renderer>().material = on;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            //GetComponent<Renderer>().material = off;
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.CompareTo("Hand") == 0)
        {
            buttonOn = true;
        }
    }
}
