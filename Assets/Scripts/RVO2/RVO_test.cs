using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVO_test : MonoBehaviour
{
    private RVO_agent agent;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<RVO_agent>();
        agent.target = target;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
