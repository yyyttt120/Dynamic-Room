using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            transform.Translate(transform.forward * speed * Time.deltaTime,Space.World);
        if (Input.GetKey(KeyCode.DownArrow))
            transform.Translate(transform.forward * -speed * Time.deltaTime,Space.World);
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Rotate(new Vector3(0, -speed * Time.deltaTime * 120, 0));
        if (Input.GetKey(KeyCode.RightArrow))
            transform.Rotate(new Vector3(0, speed * Time.deltaTime * 120, 0));
    }
}
