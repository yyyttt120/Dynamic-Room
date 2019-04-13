using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = transform.position + transform.up * speed;
	}

    private void OnTriggerEnter(Collider other)
    {
        print("destroy by" + other.gameObject.name);
        Destroy(this.gameObject);
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        print("destroy by" + collision.gameObject.name);
        Destroy(this.gameObject);
    }*/

    private void OnTriggerStay(Collider other)
    {
        print("destroy by" + other.gameObject.name);
        Destroy(this.gameObject);
    }
}

