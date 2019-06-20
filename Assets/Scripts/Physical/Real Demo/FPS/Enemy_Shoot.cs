using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shoot : Shoot {
    public AudioSource cocked;
    public AudioSource hurt;
    public AudioSource die;
    public GameObject mylight;
    private LayerMask mask = ~(1 << 9 | 1 << 10| 1<<11);
    Animator ani;
    bool detected;// if detecte hero, it's true; else, false
	// Use this for initialization
	void Start () {
        ani = GetComponent<Animator>();
        //target = GameObject.Find("Dummy (1)").transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        detected = DetectHero();
        //if ( !mylight.activeSelf)
            //detected = false;
        ani.SetBool("SawHero", detected);
	}

    // when enemy can see hero, return true;
    private bool DetectHero()
    {
        RaycastHit hit;
        Vector3 detectDirection = target.transform.position - hurt.gameObject.transform.position;
        Physics.Raycast(hurt.gameObject.transform.position, detectDirection, out hit, 100f,mask);
        detectDirection.y = 0;
        //this.gameObject.transform.forward = detectDirection;
        Color color = Color.red;
        //Debug.DrawRay(hurt.gameObject.transform.position, hit.point - hurt.transform.position, color, 0.1f);
        //print($"hit {hit.collider.name}");
        if (hit.collider.tag.CompareTo("Hero") == 0)
        {
            this.gameObject.transform.forward = detectDirection;
            return true;
        }
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

    public void ShootBullet()
    {
        int bullet = ani.GetInteger("Bullet");
        ani.SetInteger("Bullet", bullet - 1);
    }

    public void DeleteBody()
    {
        Destroy(this.gameObject);
    }
}
