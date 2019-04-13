using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Function : MonoBehaviour {
    Animator ani;
	// Use this for initialization
	void Start () {
        ani = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShootBullet()
    {
        int bullet = ani.GetInteger("Bullet");
        ani.SetInteger("Bullet", bullet - 1);
    }

    public void DeleteBody()
    {
        this.gameObject.SetActive(false);
    }
}
