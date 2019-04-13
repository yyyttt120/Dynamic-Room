using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Secret_Paper : MonoBehaviour {

    private bool touched;
	// Use this for initialization
	void Start () {
        touched = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.CompareTo("Hand") == 0)
        {
            touched = true;
            StartCoroutine(Waite());
        }
    }

    IEnumerator Waite()
    {
        yield return new WaitForSeconds(1.5f);
        if(touched)
        {
            this.gameObject.SetActive(false);
        }
        
    }
}
