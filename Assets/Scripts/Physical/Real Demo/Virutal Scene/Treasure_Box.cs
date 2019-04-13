using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure_Box : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerStay(Collider other)
    {
        if(other.tag.CompareTo("Hero") == 0)
        {
            print("treasure in");
            GetComponent<Animator>().SetBool("Open", true);
        }
    }
}
