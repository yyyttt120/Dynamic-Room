﻿using System.Diagnostics;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;

public class NetMqPublisher
{
    private readonly Thread _listenerWorker;

    private bool _listenerCancelled;

    public delegate string MessageDelegate(string message);

    private readonly MessageDelegate _messageDelegate;

    private readonly Stopwatch _contactWatch;

    private const long ContactThreshold = 1000;

    public bool Connected;
    public string result;

    private void ListenerWork()
    {
        AsyncIO.ForceDotNet.Force();
        using (var client = new RequestSocket())
        {
            client.Connect("tcp://localhost:7777");

            while (!_listenerCancelled)
            {
                //Connected = _contactWatch.ElapsedMilliseconds < ContactThreshold;
                string message = "0.01111111*-5.03*-0.13*2.77*0*0*-1*0*0*0*0*0*0*-4.896925*1.03613*4.51299*0*0*-1*-4.408195*1.082453*4.491018*0*0*0*-5.180458*0.9256428*1.865499*0*0*1*-4.964385*1.137763*1.843617*-3.226*1.037*1.900139*-7.01524*1.084502*3.189588*1*0*-1.192093E-07*-6.998515*1.003343*4.140667*0*0*0*-2.774269*1.084502*3.148801*-1*0*-1.192093E-07*0*0*0*0*0*0*0.02222222*-5.03*-0.13*2.77*0*0*-1*0*0*0*0*0*0*-4.896925*1.03613*4.51299*0*0*-1*-4.408195*1.082453*4.491018*0*0*0*-5.180458*0.9256428*1.865499*0*0*1*-4.964385*1.137763*1.843617*-3.226*1.037*1.900139*-7.01524*1.084502*3.189588*1*0*-1.192093E-07*-6.998515*1.003343*4.140667*0*0*0*-2.774269*1.084502*3.148801*-1*0*-1.192093E-07*0*0*0*0*0*0*0.03333334*-5.03*-0.13*2.77*0*0*-1*0*0*0*0*0*0*-4.896925*1.03613*4.51299*0*0*-1*-4.408195*1.082453*4.491018*0*0*0*-5.180458*0.9256428*1.865499*0*0*1*-4.964385*1.137763*1.843617*-3.226*1.037*1.900139*-7.01524*1.084502*3.189588*1*0*-1.192093E-07*-6.998515*1.003343*4.140667*0*0*0*-2.774269*1.084502*3.148801*-1*0*-1.192093E-07*0*0*0*0*0*0*0.04444445*-5.03*-0.13*2.77*0*0*-1*0*0*0*0*0*0*-4.896925*1.03613*4.51299*0*0*-1*-4.408195*1.082453*4.491018*0*0*0*-5.180458*0.9256428*1.865499*0*0*1*-4.964385*1.137763*1.843617*-3.226*1.037*1.900139*-7.01524*1.084502*3.189588*1*0*-1.192093E-07*-6.998515*1.003343*4.140667*0*0*0*-2.774269*1.084502*3.148801*-1*0*-1.192093E-07*0*0*0*0*0*0*0.05555556*-5.03*-0.13*2.77*0*0*-1*0*0*0*0*0*0*-4.896925*1.03613*4.51299*0*0*-1*-4.408195*1.082453*4.491018*0*0*0*-5.180458*0.9256428*1.865499*0*0*1*-4.964385*1.137763*1.843617*-3.226*1.037*1.900139*-7.01524*1.084502*3.189588*1*0*-1.192093E-07*-6.998515*1.003343*4.140667*0*0*0*-2.774269*1.084502*3.148801*-1*0*-1.192093E-07*0*0*0*0*0*0*0.06666667*-5.03*-0.13*2.77*0*0*-1*0*0*0*0*0*0*-4.896925*1.03613*4.51299*0*0*-1*-4.408195*1.082453*4.491018*0*0*0*-5.180458*0.9256428*1.865499*0*0*1*-4.964385*1.137763*1.843617*-3.226*1.037*1.900139*-7.01524*1.084502*3.189588*1*0*-1.192093E-07*-6.998515*1.003343*4.140667*0*0*0*-2.774269*1.084502*3.148801*-1*0*-1.192093E-07*0*0*0*0*0*0*0.07777778*-5.03*-0.13*2.77*0*0*-1*0*0*0*0*0*0*-4.896925*1.03613*4.51299*0*0*-1*-4.408195*1.082453*4.491018*0*0*0*-5.180458*0.9256428*1.865499*0*0*1*-4.964385*1.137763*1.843617*-3.226*1.037*1.900139*-7.01524*1.084502*3.189588*1*0*-1.192093E-07*-6.998515*1.003343*4.140667*0*0*0*-2.774269*1.084502*3.148801*-1*0*-1.192093E-07*0*0*0*0*0*0*0.08888889*-5.03*-0.13*2.77*0*0*-1*0*0*0*0*0*0*-4.896925*1.03613*4.51299*0*0*-1*-4.408195*1.082453*4.491018*0*0*0*-5.180458*0.9256428*1.865499*0*0*1*-4.964385*1.137763*1.843617*-3.226*1.037*1.900139*-7.01524*1.084502*3.189588*1*0*-1.192093E-07*-6.998515*1.003343*4.140667*0*0*0*-2.774269*1.084502*3.148801*-1*0*-1.192093E-07*0*0*0*0*0*0*0.1*-5.03*-0.13*2.77*0*0*-1*0*0*0*0*0*0*-4.896925*1.03613*4.51299*0*0*-1*-4.408195*1.082453*4.491018*0*0*0*-5.180458*0.9256428*1.865499*0*0*1*-4.964385*1.137763*1.843617*-3.226*1.037*1.900139*-7.01524*1.084502*3.189588*1*0*-1.192093E-07*-6.998515*1.003343*4.140667*0*0*0*-2.774269*1.084502*3.148801*-1*0*-1.192093E-07*0*0*0*0*0*0*0.1111111*-5.03*-0.13*2.77*0*0*-1*0*0*0*0*0*0*-4.896925*1.03613*4.51299*0*0*-1*-4.408195*1.082453*4.491018*0*0*0*-5.180458*0.9256428*1.865499*0*0*1*-4.964385*1.137763*1.843617*-3.226*1.037*1.900139*-7.01524*1.084502*3.189588*1*0*-1.192093E-07*-6.998515*1.003343*4.140667*0*0*0*-2.774269*1.084502*3.148801*-1*0*-1.192093E-07*0*0*0*0*0*0*0.1222222*-5.03*-0.13*2.77*0*0*-1*0*0*0*0*0*0*-4.896925*1.03613*4.51299*0*0*-1*-4.408195*1.082453*4.491018*0*0*0*-5.180458*0.9256428*1.865499*0*0*1*-4.964385*1.137763*1.843617*-3.226*1.037*1.900139*-7.01524*1.084502*3.189588*1*0*-1.192093E-07*-6.998515*1.003343*4.140667*0*0*0*-2.774269*1.084502*3.148801*-1*0*-1.192093E-07*0*0*0*0*0*0*0.1333333*-5.03*-0.13*2.77*0*0*-1*0*0*0*0*0*0*-4.896925*1.03613*4.51299*0*0*-1*-4.408195*1.082453*4.491018*0*0*0*-5.180458*0.9256428*1.865499*0*0*1*-4.964385*1.137763*1.843617*-3.226*1.037*1.900139*-7.01524*1.084502*3.189588*1*0*-1.192093E-07*-6.998515*1.003343*4.140667*0*0*0*-2.774269*1.084502*3.148801*-1*0*-1.192093E-07*0*0*0*0*0*0*0.1444445*-5.03*-0.13*2.77*0*0*-1*0*0*0*0*0*0*-4.896925*1.03613*4.51299*0*0*-1*-4.408195*1.082453*4.491018*0*0*0*-5.180458*0.9256428*1.865499*0*0*1*-4.964385*1.137763*1.843617*-3.226*1.037*1.900139*-7.01524*1.084502*3.189588*1*0*-1.192093E-07*-6.998515*1.003343*4.140667*0*0*0*-2.774269*1.084502*3.148801*-1*0*-1.192093E-07*0*0*0*0*0*0*0.1555556*-5.03*-0.13*2.77*0*0*-1*0*0*0*0*0*0*-4.896925*1.03613*4.51299*0*0*-1*-4.408195*1.082453*4.491018*0*0*0*-5.180458*0.9256428*1.865499*0*0*1*-4.964385*1.137763*1.843617*-3.226*1.037*1.900139*-7.01524*1.084502*3.189588*1*0*-1.192093E-07*-6.998515*1.003343*4.140667*0*0*0*-2.774269*1.084502*3.148801*-1*0*-1.192093E-07*0*0*0*0*0*0*0.1666667*-5.03*-0.13*2.77*0*0*-1*0*0*0*0*0*0*-4.896925*1.03613*4.51299*0*0*-1*-4.408195*1.082453*4.491018*0*0*0*-5.180458*0.9256428*1.865499*0*0*1*-4.964385*1.137763*1.843617*-3.226*1.037*1.900139*-7.01524*1.084502*3.189588*1*0*-1.192093E-07*-6.998515*1.003343*4.140667*0*0*0*-2.774269*1.084502*3.148801*-1*0*-1.192093E-07*0*0*0*0*0*0*0.1777778*-5.03*-0.13*2.77*0*0*-1*0*0*0*0*0*0*-4.896925*1.03613*4.51299*0*0*-1*-4.408195*1.082453*4.491018*0*0*0*-5.180458*0.9256428*1.865499*0*0*1*-4.964385*1.137763*1.843617*-3.226*1.037*1.900139*-7.01524*1.084502*3.189588*1*0*-1.192093E-07*-6.998515*1.003343*4.140667*0*0*0*-2.774269*1.084502*3.148801*-1*0*-1.192093E-07*0*0*0*0*0*0*0.1888889*-5.03*-0.13*2.77*0*0*-1*0*0*0*0*0*0*-4.896925*1.03613*4.51299*0*0*-1*-4.408195*1.082453*4.491018*0*0*0*-5.180458*0.9256428*1.865499*0*0*1*-4.964385*1.137763*1.843617*-3.226*1.037*1.900139*-7.01524*1.084502*3.189588*1*0*-1.192093E-07*-6.998515*1.003343*4.140667*0*0*0*-2.774269*1.084502*3.148801*-1*0*-1.192093E-07*0*0*0*0*0*0*0.2*-5.03*-0.13*2.77*0*0*-1*0*0*0*0*0*0*-4.896925*1.03613*4.51299*0*0*-1*-4.408195*1.082453*4.491018*0*0*0*-5.180458*0.9256428*1.865499*0*0*1*-4.964385*1.137763*1.843617*-3.226*1.037*1.900139*-7.01524*1.084502*3.189588*1*0*-1.192093E-07*-6.998515*1.003343*4.140667*0*0*0*-2.774269*1.084502*3.148801*-1*0*-1.192093E-07*0*0*0*0*0*0*0.2111111*-5.03*-0.13*2.77*0*0*-1*0*0*0*0*0*0*-4.896925*1.03613*4.51299*0*0*-1*-4.408195*1.082453*4.491018*0*0*0*-5.180458*0.9256428*1.865499*0*0*1*-4.964385*1.137763*1.843617*-3.226*1.037*1.900139*-7.01524*1.084502*3.189588*1*0*-1.192093E-07*-6.998515*1.003343*4.140667*0*0*0*-2.774269*1.084502*3.148801*-1*0*-1.192093E-07*0*0*0*0*0*0*0.2222222*-5.03*-0.13*2.77*0*0*-1*0*0*0*0*0*0*-4.896925*1.03613*4.51299*0*0*-1*-4.408195*1.082453*4.491018*0*0*0*-5.180458*0.9256428*1.865499*0*0*1*-4.964385*1.137763*1.843617*-3.226*1.037*1.900139*-7.01524*1.084502*3.189588*1*0*-1.192093E-07*-6.998515*1.003343*4.140667*0*0*0*-2.774269*1.084502*3.148801*-1*0*-1.192093E-07*0*0*0*0*0*0";
                //if (!client.TryReceiveFrameString(out message)) continue;
                _contactWatch.Restart();
                var response = _messageDelegate(message);
                if (response != null)
                {
                    client.SendFrame(response);
                    string m2 = client.ReceiveFrameString();
                    //UnityEngine.Debug.Log(m2);
                    result = m2;
                }

                //var response = _messageDelegate(message);
                //server.SendFrame(response);
            }
        }
        NetMQConfig.Cleanup();
    }

    public NetMqPublisher(MessageDelegate messageDelegate)
    {
        _messageDelegate = messageDelegate;
        _contactWatch = new Stopwatch();
        _contactWatch.Start();
        _listenerWorker = new Thread(ListenerWork);
    }

    public void Start()
    {
        _listenerCancelled = false;
        _listenerWorker.Start();
    }

    public void Stop()
    {
        _listenerCancelled = true;
        _listenerWorker.Join();
    }
}

public class ServerObject : MonoBehaviour
{
    public bool Connected;
    private NetMqPublisher _netMqPublisher;
    public string result { get; set; }

    public string _Response { get; set; }
    private void Start()
    {
        _netMqPublisher = new NetMqPublisher(HandleMessage);
        _netMqPublisher.Start();
    }

    private void Update()
    {
        var position = transform.position;
        //_response = $"{position.x} {position.y} {position.z}";
        Connected = _netMqPublisher.Connected;
        result = _netMqPublisher.result;
        if(Input.GetKeyDown(KeyCode.LeftControl))
            _netMqPublisher.Stop();
    }

    private string HandleMessage(string message)
    {
        // Not on main thread
        return _Response;
    }

    private void OnDestroy()
    {
        _netMqPublisher.Stop();
    }
}