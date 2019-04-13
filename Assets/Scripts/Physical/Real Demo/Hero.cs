using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {
    public AudioSource hurtVoice;
    public GameObject bloodImage;
    public int getHitCount = 0;
    private bool startBlood;
    private int bloodCount;
	// Use this for initialization
	void Start () {
        bloodCount = 100;
	}
	
	// Update is called once per frame
	void Update () {
        if (startBlood)
        {
            bloodCount -= 1;
        }

        if(bloodCount < 0)
        {
            bloodImage.SetActive(false);
            bloodCount = 100;
            startBlood = false;
        }

        //print("Get hit " + getHitCount);
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.CompareTo("Bullet") == 0)
        {
            startBlood = true;
            print("hit hero");
            hurtVoice.Play();
            bloodImage.SetActive(true);
            getHitCount++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.CompareTo("Bullet") == 0)
        {
            startBlood = true;
            print("hit hero");
            hurtVoice.Play();
            bloodImage.SetActive(true);
            getHitCount++;
        }
    }
}
