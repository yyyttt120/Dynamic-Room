using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO.Ports;


public class RoombaControllerScript : MonoBehaviour
{
    public GameObject mainCamera;
    public static RoombaControllerScript Instance;
    List<SerialPort> serialPorts;
    public List<string> _Ports;
    public int speed;
    public bool open = false;

    public float _velocity;
    public float _radius;
    public bool Double_digid;

    private int RoombaCounter;

    [SerializeField]
    //private int BOARDRATE = 19200;
    private int BOARDRATE = 57600;

    void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start()
    {
        serialPorts = new List<SerialPort>();
      
        foreach (var p in _Ports)
        {
            string s = "";
            if (Double_digid) 
            {
                s += "\\\\.\\";
            }
            s += p;
                serialPorts.Add(new SerialPort(s, BOARDRATE, Parity.None, 8, StopBits.One));
            ++RoombaCounter;
        }
    }
        
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Z))
        {
            if (RoombaCounter == 1)
            {
                Open();
                sendPacket(WakeUp(), 0);
                sendPacket(Safe(), 0);
                //sendPacket(Full(), 0);
                sendPacket(BoardRate(), 0);
                open = true;

                print("connecting");
            }

            if (RoombaCounter == 2)
            {
                Open();
                sendPacket(WakeUp(), 0);
                sendPacket(Safe(), 0);
                sendPacket(BoardRate(), 0);
                sendPacket(WakeUp(), 1);
                sendPacket(Safe(), 1);
                sendPacket(BoardRate(), 1);
                open = true;
            }
            if (RoombaCounter == 3)
            {
                Open();
                sendPacket(WakeUp(), 0);
                sendPacket(Safe(), 0);
                sendPacket(BoardRate(), 0);
                sendPacket(WakeUp(), 1);
                sendPacket(Safe(), 1);
                sendPacket(BoardRate(), 1);
                sendPacket(WakeUp(), 2);
                sendPacket(Safe(), 2);
                sendPacket(BoardRate(), 2);
                open = true;
            }
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            Move(speed,speed,0);
            print("forward");
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            sendPacket(Drive_Straight(-speed), 0);
            print("back");
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            sendPacket(Drive_Turn(true, speed), 0);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            sendPacket(Drive_Turn(false, speed), 0);
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            sendPacket(BoardRate(), 0);
            print("Done");
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            Stop();
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            sendPacket(Drive_Straight(speed), 1);
            print("forward");
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            sendPacket(Drive_Straight(-speed), 1);
            print("back");
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            sendPacket(Drive_Turn(true, speed), 1);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            sendPacket(Drive_Turn(false, speed), 1);
        }

        else if (Input.GetKeyUp(KeyCode.B))
        {
            sendPacket(Drive(0, 0), 1);
        }
        else if (Input.GetKeyUp(KeyCode.Keypad8))
        {
            sendPacket(Drive_Straight(speed), 2);
            print("forward");
        }
        else if (Input.GetKeyUp(KeyCode.Keypad5))
        {
            sendPacket(Drive_Straight(-speed), 2);
            print("back");
        }
        else if (Input.GetKeyUp(KeyCode.Keypad6))
        {
            sendPacket(Drive_Turn(true, speed), 2);
        }
        else if (Input.GetKeyUp(KeyCode.Keypad4))
        {
            sendPacket(Drive_Turn(false, speed), 2);
        }

        else if (Input.GetKeyUp(KeyCode.Keypad0))
        {
            sendPacket(Drive(0, 0), 2);
        }

    }

    public void Open()
    {
        int i = 0;
        foreach (var r in serialPorts)
        {
            r.DtrEnable = false;
            r.Handshake = Handshake.None;
            r.RtsEnable = false;
            r.Open();
            print("Connected");
            i++;
            //AudioClip clip = GetComponent<AudioSource>().clip;
            //GetComponent<AudioSource>().PlayOneShot(clip);
        }
    }

    public void ChangeBoardRate()
    {

    }

    public void MoveForward()
    {
        sendPacket(Drive_Straight(speed), 0);
    }

    public void MoveForward(int _speed) 
    {
        sendPacket(Drive_Straight(_speed), 0);       
    }

    public void MoveBack()
    {
        sendPacket(Drive_Straight(-speed), 0);
    }

    public void MoveBack(int _speed)
    {
        sendPacket(Drive_Straight(-_speed), 0);
    }

    public void Stop()
    {
        sendPacket(Drive(0, 0), 0);
    }

    void OnDestroy()
    {
        foreach (var r in serialPorts)
        {
            r.Close();
        }
    }

    public const int BAUD_19200 = 7;
    public const int BAUD_115200 = 11;

    private byte[] WakeUp()
    {
        byte[] packet = new byte[] { 128 };
        return packet;
    }

    private byte[] Baud(byte baud_code)
    {
        byte[] packet = new byte[] { 129, baud_code };
        return packet;
    }

    //--------------------------------------------------------------------------

    //Publicなルンバ制御関数
    public void Forward(int _speed, int roombaId)
    {
        sendPacket(Drive_Straight(_speed), roombaId);
    }

    public void Back(int _speed, int roombaId)
    {
        sendPacket(Drive_Straight(-_speed), roombaId);
    }

    public void Stop(int roombaId)
    {
        sendPacket(Drive(0, 0), roombaId);
    }

    public void TurnLeft(int _speed, int roombaId)
    {
        sendPacket(Drive_Turn(false, _speed/5), roombaId);
        print("TurnLeft");
    }

    public void TurnRight(int _speed, int roombaId)
    {
        sendPacket(Drive_Turn(true, _speed/5), roombaId);
        print("TrunRight");
    }

    public void Move(int speed_R,int speed_L,int roombaId)
    {
        sendPacket(Drive_Wheel(speed_R, speed_L),roombaId);
    }

    //--------------------------------------------------------------------------

    private byte[] Safe()
    {
        byte[] packet = new byte[] { 131 };
        print("ModeSafe");
        return packet;
    }

    private byte[] BoardRate()
    {
        byte[] packet = new byte[] { 129, 10 };
        print("BoadRate:57600");
        return packet;
    }

    private byte[] Full()
    {
        byte[] packet = new byte[] { 132 };
        return packet;
    }

    private byte[] Power()
    {
        byte[] packet = new byte[] { 133 };
        return packet;
    }

    //--------------------------------------------------------------------------

    private byte[] Clean()
    {
        byte[] packet = new byte[] { 135 };
        return packet;
    }

    //--------------------------------------------------------------------------

    private byte[] Drive(int velocity, int radius)
    {
        //convert HEX
        byte[] velocity_bytes = BitConverter.GetBytes(velocity);
        byte[] radius_bytes = BitConverter.GetBytes(radius);

        byte[] packet = new byte[] { 137, velocity_bytes[1], velocity_bytes[0], radius_bytes[1], radius_bytes[0] };
        return packet;
    }

    public byte[] Drive_Straight(int velocity)
    {
        //convert HEX
        byte[] velocity_bytes = BitConverter.GetBytes((UInt16)velocity);
        byte[] radius_bytes = BitConverter.GetBytes((UInt16)0x7FFF);

        byte[] packet = new byte[] { 137, velocity_bytes[1], velocity_bytes[0], radius_bytes[1], radius_bytes[0] };
        return packet;
    }


    //public IEnumerator Turn() 
    //{
    //    var sec = 7f / 360f * ar.deg;
    //    sendPacket(Drive_Turn(ar.tc, 189), 0);
    //    yield return new WaitForSeconds(sec);
    //    Stop();
    //}

    private byte[] Drive_Turn(bool clockwise, int velocity)
    {
        int direct;
        if (clockwise)
            direct = 1;
        else
            direct = -1;

        //convert HEX
        byte[] velocity_bytes = BitConverter.GetBytes((UInt16)velocity);
        byte[] radius_bytes = BitConverter.GetBytes((UInt16)direct);

        //byte[] velocity_bytes = BitConverter.GetBytes((UInt16)_velocity);
        //byte[] radius_bytes = BitConverter.GetBytes((UInt16)_radius);


        byte[] packet = new byte[] { 137, velocity_bytes[1], velocity_bytes[0], radius_bytes[1], radius_bytes[0] };
        return packet;
    }


    private byte[] Motors(bool mainb_direct, bool sideb_direct, bool mainbrush, bool vaccum, bool sidebrush)
    {
        int data = 0;
        data += (mainb_direct) ? 16 : 0;
        data += (sideb_direct) ? 8 : 0;
        data += (mainbrush) ? 4 : 0;
        data += (vaccum) ? 2 : 0;
        data += (sidebrush) ? 1 : 0;


        byte[] packet = new byte[] { 138, Convert.ToByte(data) };

        return packet;
    }

    private void sendPacket(byte[] packet, int portIndex)
    {
        var serialPort = serialPorts[portIndex];

        serialPort.Write(packet, 0, packet.Length);

    }

    private byte[] Drive_Wheel(int velocityR, int velocityL)//Drive direct mode
    {
      
        //convert HEX
        byte[] velocityR_bytes = BitConverter.GetBytes((UInt16)velocityR);
        byte[] velocityL_bytes = BitConverter.GetBytes((UInt16)velocityL);



        byte[] packet = new byte[] { 145, velocityR_bytes[1], velocityR_bytes[0], velocityL_bytes[1], velocityL_bytes[0] };
        return packet;
    }
}
