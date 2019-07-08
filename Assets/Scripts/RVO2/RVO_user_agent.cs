using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RVO;
using Random = System.Random;
using Vector2 = RVO.Vector2;

public class RVO_user_agent : RVO_agent
{
    public SteamVR_TrackedObject user;
    public enum controllType {real,simulation}
    public controllType ControlType;

    private Vector3 pos_lastFrame;
    // Start is called before the first frame update
    void Start()
    {
        sid = 0;
        pos_lastFrame = transform.position;
    }
    private void FixedUpdate()
    {
        Simulator.Instance.setAgentRadius(0, 0.3f);
        //update the position and velocity for user in simulation of RVO
        if (ControlType == controllType.real)
        {
            SteamVR_Controller.Device device = SteamVR_Controller.Input((int)user.index);
            Simulator.Instance.setAgentPosition(0, new Vector2(transform.position.x, transform.position.z));
            Simulator.Instance.setAgentVelocity(0, new Vector2(device.velocity.x, device.velocity.z));
        }
        else
        {
            Simulator.Instance.setAgentPosition(0, new Vector2(transform.position.x, transform.position.z));
            Vector3 velocity = (transform.position - pos_lastFrame) / Time.deltaTime;
            velocity.y = 0;
            pos_lastFrame = transform.position;
            //print($"user velocity = {velocity.magnitude}");
            Vector2 vel_2 = new Vector2(velocity.x, velocity.z);
            Simulator.Instance.setAgentVelocity(0, vel_2 * 0.0f);
        }
    }

}
