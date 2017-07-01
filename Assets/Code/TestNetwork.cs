using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using cocosocket4unity;
using System.IO;
using System;
using Google.Protobuf;
using BestSects.Net;
using BestSects.protocol;

public class TestNetwork : MonoBehaviour
{
    public Button btnConnect;
    public Button btnSend;
    public Text ConnectResult;
    public Text ConnectState;
    string result = "";
    USocket usocket;
    Queue<Action> methods = new Queue<Action>();

    public string Result
    {
        get { return result; }
        set
        {
            if (result != value)
            {
                result = value;
                RunInMainThread(() => { ConnectResult.text = result; });

            }
        }
    }


    private void Awake()
    {
        btnConnect.onClick.AddListener(() =>
        {
            Result += "开始尝试连接\n";
            //usocket = new USocket(this, new LVProtocol());
            //usocket.Connect("192.168.0.102", 1520, false);
            //NetworkModule.Instance.Connect("192.168.0.102", 1520);
            NetworkModule.Instance.Connect("47.94.220.1", 1520);
            Messenger.AddListener<IMessage>(MessageID.ResUserLogin.ToString(), (data) =>
            {
                ResUserLoginMessage response = data as ResUserLoginMessage;
                Result += response.Msg + "\n";
            });
        });
        btnSend.onClick.AddListener(() =>
        {
            Result += "开始发送数据\n";
            //usocket = new USocket(this, new LVProtocol());
            //usocket.Connect("192.168.0.102", 1520, false);
            //NetworkModule.Instance.Connect("192.168.0.102", 1520);
            NetworkModule.Instance.Send(MessageID.ReqUserLogin, new ReqUserLoginMessage() { Username = "abc", Password = "123" });
            //Messenger.BroadcastAsync<IMessage>(MessageID.ResUserLogin.ToString(), new ResUserLoginMessage() { Code = 2, Data = "123" });
        });
        
    }
    // Use this for initialization
    void Start()
    {

    }

    void RunInMainThread(Action method)
    {
        lock (methods)
        {
            methods.Enqueue(method);
        }
    }


    // Update is called once per frame
    void Update()
    {
        lock (methods)
        {
            if (methods.Count > 0)
            {
                methods.Dequeue()();
            }
        }
        ConnectState.text = NetworkModule.Instance.State.ToString();
    }
    
}
