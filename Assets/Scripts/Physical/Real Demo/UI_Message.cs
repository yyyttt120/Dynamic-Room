using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//attach onto the canvas
public class UI_Message : MonoBehaviour {
    public int lastSceneId;
    public GameObject canvas;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (SceneManager.GetActiveScene().buildIndex == lastSceneId)
            canvas.SetActive(true);
        else
            canvas.SetActive(false);
	}


}
