using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {
    public GameObject gunPoint;
    public GameObject bullet;
    public float range_x;
    public float range_y;
    public GameObject target;
    public AudioSource gunShut;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    protected void CreateBullet()
    {
        gunPoint.transform.up = GetShootDirection();
        //Quaternion shoot = Quaternion.LookRotation(Vector3.up, GetShootDirection());
        Instantiate(bullet, gunPoint.transform.position, gunPoint.transform.rotation);   
    }

    protected Vector3 GetShootDirection()
    {
        float x_offset = Random.Range(-range_x, range_x);
        float y_offset = Random.Range(-range_y, range_y);
        Vector3 shootPiont = target.transform.position + new Vector3(x_offset, y_offset, 0);
        return shootPiont - gunPoint.transform.position;
    }

    public void PlayGunShut()
    {
        if(gunShut != null)
        {
            gunShut.Play();
        }
    }
}
