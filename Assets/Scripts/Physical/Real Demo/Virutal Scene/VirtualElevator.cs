using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VirtualElevator : MonoBehaviour {
    GameObject elevatorSwitch;// the switch of elevator
    GameObject button;
    GameObject button_emergency;
    GameObject door;
    GameObject ele_Indicator;

    bool button_ele;
    bool button_emer;
    bool switch_ele;
    bool userEntered;
    bool newScene = false; //if entered a new scene;
    bool doorOpening;
    bool doorClosing;

    Vector3 closePosition;

    int sceneID;
    int lastSceneID;

    int count = 0;
	// Use this for initialization
	void Start () {
        door = transform.GetChild(0).gameObject;
        closePosition = door.transform.position;
        button = GameObject.Find("Button");
        button_emergency = GameObject.Find("Button_Emergency");
        ele_Indicator = GameObject.Find("Elevator_Enter_Indicator");
        lastSceneID = SceneManager.GetActiveScene().buildIndex;
        door.transform.position = closePosition + new Vector3(0.016f, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        bool emerge = button_emergency.GetComponent<ElevatorButton>().GetButton();
        //print("emergency button ="+ emerge);
        if (emerge)
        {
            button_emer = true;
        }
        if (button.GetComponent<ElevatorButton>().GetButton())
        {
            button_ele = true;
        }
        elevatorSwitch = GameObject.Find("Elevator_Switch_Real").transform.GetChild(0).gameObject;
        //switch_ele = elevatorSwitch.GetComponent<Swithc_Elevator>().GetSwitcher();
        userEntered = ele_Indicator.GetComponent<Elevator_Enter_Indicator>().GetUserEntered();
        //print("button_ele =" + button_ele);
        //print("switch_ele =" + switch_ele);
        sceneID = SceneManager.GetActiveScene().buildIndex;
        if (sceneID != lastSceneID)
        {
            newScene = true;
        }
        lastSceneID = sceneID;

        //print("button_ele =" + button_ele);
        if (button_ele /*&& switch_ele*/ || Input.GetKey(KeyCode.P))
            OpenDoor();
        if(!doorOpening && door.transform.position.x - closePosition.x < 0.010 && !GetComponent<ElevatorEmergency>().GetEmergency())    
            CloseDoor();

        /*if(Input.GetKeyUp(KeyCode.L))
            SceneManager.LoadScene(sceneID + 1);*/
        /*if (userEntered && !newScene)
            CloseDoor();*/
        //print("new scene =" + newScene);
        //print("door close =" + doorClosing);
        //print("user enter =" + userEntered);
        if (newScene && !doorClosing && userEntered && !GetComponent<ElevatorEmergency>().GetEmergency())
        {
            OpenDoor();
            /*if (!doorOpening)
                newScene = false;*/
        }

        //print("newscene =" + newScene);

        if (button_emer)
            GameObject.Find("Door").GetComponent<Door_slide_Right>().enabled = true;
    }

    void OpenDoor()
    {
        print("door opening");
        if (door.transform.position.x - closePosition.x > -0.8f)
        {
            door.transform.position = door.transform.position + new Vector3(-0.008f, 0, 0);
            doorOpening = true;
        }
        else
        {
            doorOpening = false;
            button_ele = false;
        }
    }

    void CloseDoor()
    {
        count += 1;
        //print("count =" + count);
        if (count > 500)
        {
            if (door.transform.position.x - closePosition.x < 0)
            {
                door.transform.position = door.transform.position + new Vector3(0.008f, 0, 0);
                doorClosing = true;
            }
            else
            {
                print("closed");
                count = 0;
                door.transform.position = closePosition + new Vector3(0.016f, 0, 0);
                if (!newScene)
                    //SceneManager.LoadScene(sceneID + 1);
                    print("switch");
                else
                    newScene = false;
                    //print("dd");         
                doorClosing = false;
            }
        }
        else
            doorClosing = false;
    }


}
