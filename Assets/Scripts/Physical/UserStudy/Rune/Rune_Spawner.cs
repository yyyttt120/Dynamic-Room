using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune_Spawner : MonoBehaviour
{
    public GameObject[] VWalls;
    public GameObject[] Runes;
    private GameObject currentRune;
    public AudioSource gameComplete;
    public GameObject lights;
    private int runeCount;
    private int lastWallID = 0;
    private int[] IDs;
    private int ID_index = 0;
    // Start is called before the first frame update
    void Start()
    {
        lights.SetActive(false);
        IDs = new int[5] { 3, 0, 1, 0, 1 };
    }

    // Update is called once per frame
    void Update()
    {
        if(currentRune == null)
        {
            runeCount++;
            if (runeCount > 5 && runeCount < 7)
                GameComplete();
            else
                SpawnRune();
        }
    }

    private void GameComplete()
    {
        gameComplete.Play();
        lights.SetActive(true);
    }

    private void SpawnRune()
    {
        /*int wallID = Random.Range(0, 3);
        while (wallID == lastWallID)
        {
            wallID = Random.Range(0, 3);
        }
        lastWallID = wallID;*/

        int wallID = IDs[ID_index];
        ID_index++;
        float pos_x = Random.Range(-0.3f, 0.3f);
        float pos_y = Random.Range(-0.3f, 0.3f);
        GameObject wall = VWalls[wallID];
        pos_x = pos_x * wall.GetComponent<Renderer>().bounds.size.x + wall.transform.position.x;
        Vector3 pos = new Vector3(pos_x, 0.5f, wall.transform.position.z) + wall.transform.forward * 0.02f;
        Quaternion rot = wall.transform.rotation; /*Quaternion.LookRotation(-wall.transform.up, wall.transform.forward)*/;
        currentRune = Instantiate(Runes[runeCount - 1], pos, rot);
        try
        {
            currentRune.transform.parent = VWalls[wallID].transform.GetChild(1);
            currentRune.transform.localPosition = new Vector3(currentRune.transform.localPosition.x, currentRune.transform.localPosition.y, 0.6f);
        }
        catch (System.Exception e)
        {
            print(VWalls[wallID].name + " " + e.Message);
        }
    }

}
