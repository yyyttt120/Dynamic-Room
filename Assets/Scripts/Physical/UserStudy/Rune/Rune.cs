using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour
{
    public Material mat;
    private TouchPoint_Rune[] touchPoints;
    private float timer = 0;
    private AudioSource vanish;
    // Start is called before the first frame update
    void Start()
    {
        touchPoints = GetComponentsInChildren<TouchPoint_Rune>();
        vanish = transform.GetChild(0).GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        /*foreach (TouchPoint_Rune t in touchPoints)
            print($"{t.name} in list");*/
        bool allTouched = true;
        foreach(TouchPoint_Rune t in touchPoints)
        {
            allTouched = allTouched && t.touched;
        }
        if (allTouched)
        {
            timer += Time.deltaTime;
            mat.EnableKeyword("_EMISSION");
            vanish.Play();
        }
        if(timer > 3f)
        {
            mat.DisableKeyword("_EMISSION");
            GameObject.Destroy(this.gameObject);
        }
    }


}
