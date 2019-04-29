using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public AudioSource notice;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        if(timer > 3)
        {
            timer = 0;
            notice.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
