using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchTest : MonoBehaviour {

    private List<GameObject> noneDestroyList;
	// Use this for initialization
	void Start () {
        noneDestroyList = new List<GameObject>();
        /*AddNoneDestroy(GameObject.Find("PTK_Elevator_2Floors"));
        AddNoneDestroy(GameObject.Find("Roomba"));
        AddNoneDestroy(GameObject.Find("[CameraRig]"));
        AddNoneDestroy(GameObject.Find("Stand_by"));
        AddNoneDestroy(GameObject.Find("StatesController"));
        AddNoneDestroy(GameObject.Find("User_Encounter_Area"));
        AddNoneDestroy(GameObject.Find("Virtual Scene Controller"));*/
        //AddNoneDestroy(GameObject.Find("Apartment Door - Wooden - No Script"));
    }
	
	// Update is called once per frame
	void Update () {
       foreach(GameObject obj in noneDestroyList)
        {
            DontDestroyOnLoad(obj);
        }

        if (Input.GetKeyUp(KeyCode.L))
            NextScene();
    }

    public void AddNoneDestroy(GameObject obj)
    {
        if (noneDestroyList.Contains(obj))
            Debug.Log(obj.name + "is already in noneDestroy list");
        else
            noneDestroyList.Add(obj);
    }

    public void SwitchScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

    public void NextScene()
    {
        int ID = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(ID + 1);
    }
}
