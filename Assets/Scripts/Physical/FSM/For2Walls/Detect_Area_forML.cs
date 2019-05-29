using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detect_Area_forML : MonoBehaviour
{
    List<GameObject> wallsInAreaList;
    // Start is called before the first frame update
    void Start()
    {
        wallsInAreaList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag.CompareTo("Wall") == 0)
        {
            print($"*************{other.name} detected");
            AddWall(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.CompareTo("Wall") == 0)
        {
            if (wallsInAreaList.Contains(other.gameObject))
                wallsInAreaList.Remove(other.gameObject);
        }
    }

    private void AddWall(GameObject wall)
    {
        if (!wallsInAreaList.Contains(wall))
            wallsInAreaList.Add(wall);
    }

    public List<GameObject> GetWallInAreaList()
    {
        return wallsInAreaList;
    }
}
