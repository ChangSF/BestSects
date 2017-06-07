using cocosocket4unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KEngine;
using System;
using Google.Protobuf;
using System.Net;
using BestSects.protocol;
using CodedOutputStream = Google.Protobuf.CodedOutputStream;

namespace BestSects.Net
{
    /// <summary>
    /// 负责网络模块的顶层操作，如连接、断开、重连及在UI线程内分发回调
    /// </summary>
    public class NetworkModule
    {
        private USocket socket;
        private Protocol protocol;
        private USocketSender sender;
        private USocketListener listener;
        private static NetworkModule instance = null;
        private NetworkHelper networkHelper;
        private NetState netState = NetState.None;
        public static NetworkModule Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NetworkModule();
                    instance.Init();
                }
                return instance;
            }
        }
        public NetState State { get { return netState; } }
        void Init()
        {
            networkHelper = (new GameObject("NetworkHelper")).AddComponent<NetworkHelper>();
            protocol = new LVProtocol();
            listener = new USocketListener(OnOpen, OnClose);
            socket = new USocket(listener, protocol);
            sender = new USocketSender(socket);
            netState = NetState.Inited;
        }
        public void Connect(string ip, int port)
        {
            IPAddress address;
            if (!IPAddress.TryParse(ip, out address))
            {
                Log.Error("ip address is invalid!");
                return;
            }
            socket.Connect(ip, port, address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6);
            netState = NetState.Connecting;
        }
        void OnOpen()
        {
            netState = NetState.Connected;
        }
        void OnClose()
        {
            netState = NetState.Closed;
        }

        public void Reset()
        {
            socket = new USocket(listener, protocol);
            sender = new USocketSender(socket);
            netState = NetState.Inited;
        }
        public enum NetState
        {
            None = 0,
            Inited,
            Connecting,
            Connected,
            Closed,
        }
    }
    /// <summary>
    /// 负责发送消息
    /// </summary>
    public class USocketSender
    {
        private USocket socket;
        public USocketSender(USocket socket)
        {
            this.socket = socket;
        }
        public void Send(MessageID msgId, IMessage msg)
        {
            if (MessageMap.GetMsgType(msgId) != msg.GetType())
            {
                Log.Error("消息号和数据不匹配！");
                return;
            }
            byte[] data = msg.ToByteArray();
            Frame frame = new Frame(data.Length + 4);
            frame.PutShort((short)(data.Length + 2));//写入数据长度
            frame.PutShort((short)msgId);//写入协议号
            frame.PutBytes(data);//写入数据
            socket.Send(frame);
        }
    }
    /// <summary>
    /// 负责转发接收的消息,处理这几个事件
    /// </summary>
    public class USocketListener : SocketListener
    {
        Action onOpen, onClose, onMessage = null;
        public USocketListener(Action onOpen, Action onClose, Action onMessage = null)
        {

        }
        public override void OnClose(USocket us, bool fromRemote)
        {
            Log.Error("connection is closed, fromRemote=" + fromRemote);
            if (onClose != null)
                onClose();
        }

        public override void OnError(USocket us, string err)
        {
            Log.Error("connection occured an error, err=" + err);
        }

        public override void OnIdle(USocket us)
        {

        }

        public override void OnMessage(USocket us, ByteBuf bb)
        {
            if (onMessage != null)
                onMessage();
            int len = bb.ReadShort();
            MessageID msgId = (MessageID)bb.ReadShort();
            if (bb.ReadableBytes() != len)
            {
                Log.Error("数据长度不对!" + msgId.ToString());
                return;
            }
            byte[] data = bb.ReadBytes(len - 4);
            IMessage msgData = null;
            switch (msgId)
            {
                case MessageID.ReqUserLogin:
                    ReqUserLoginMessage userLogin = ReqUserLoginMessage.Parser.ParseFrom(data);
                    msgData = userLogin;
                    break;
                case MessageID.ReqUserRegister:
                    break;
                case MessageID.ResUserLogin:
                    break;
                case MessageID.ResUserRegister:
                    break;
                default:
                    break;
            }
            Messenger.BroadcastAsync<IMessage>(msgId.ToString(), msgData);
        }

        public override void OnOpen(USocket us)
        {
            if (onOpen != null)
                onOpen();
        }
    }
    public sealed class NetworkHelper : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        //Clean up eventTable every time a new level loads.
        public void OnDisable()
        {

        }
    }
}