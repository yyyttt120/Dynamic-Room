using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shoot : Shoot {
    public AudioSource cocked;
    public AudioSource hurt;
    public AudioSource die;
    public GameObject light;
    Animator ani;
    bool detected;// if detecte hero, it's true; else, false
	// Use this for initialization
	void Start () {
        ani = GetComponent<Animator>();
        target = GameObject.Find("Dummy (1)").transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        detected = DetectHero();
        if ( !light.activeSelf)
            detected = false;
        ani.SetBool("SawHero", light.activeSelf);
	}

    // when enemy can see hero, return true;
    private bool DetectHero()
    {
        RaycastHit hit;
        Vector3 detectDirection = target.transform.position - this.gameObject.transform.GetChild(0).position;
        Physics.Raycast(this.gameObject.transform.GetChild(0).position, detectDirection, out hit, 100f);
        detectDirection.y = 0;
        this.gameObject.transform.forward = detectDirection;
        Color color = Color.red;
        Debug.DrawRay(this.gameObject.transform.position, detectDirection * 10, color, 0.1f);
        if (hit.collider.tag.CompareTo("Hero") == 0)
            return true;
        else
            return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.CompareTo("Bullet") == 0)
        {
            ani.SetBool("Hit", true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag.CompareTo("Bullet") == 0)
        {
            ani.SetBool("Hit", true);
        }
    }

    public void PlayCockVoice()
    {
        cocked.Play();
    }

    public void PlayHurtVoice()
    {
        hurt.Play();
    }

    public void PlayDieVoice()
    {
        die.Play();
    }
}
