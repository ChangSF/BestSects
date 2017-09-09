using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
using System;
using BestSects.protocol;

namespace BestSects.Net
{
    
    public enum MessageID : short
    {
        //本地消息
        NetworkConnect = 0,
        NetworkDisconnect = 1,

        ////请求
        //ReqUserLogin = 110,
        //ReqUserRegister = 111,
        ////响应
        //ResUserLogin = 210,
        //ResUserRegister = 211,

        ReqHeartbeat = 1000,
        /// <summary> 登录请求 </summary>
        ReqLogin = 1101,
        /// <summary> 注册请求 </summary>
        ReqRegister = 1102,
        /// <summary> 获取随机名称请求 </summary>
        ReqRandomName = 1103,



        ResponseStartIndex = 2000,
        /// <summary> 登录响应 </summary>
        ResLogin = ResponseStartIndex+101,
        /// <summary> 注册响应 </summary>
        ResRegister = ResponseStartIndex + 102,
        /// <summary> 获取随机名称响应 </summary>
        ResRandomName = ResponseStartIndex + 103,
        /// <summary> 属性变化通知响应 </summary>
        ResPropertyChange = ResponseStartIndex + 104,

    }

    public class MessageMap
    {
        //static readonly Dictionary<MessageID, System.Type> map = new Dictionary<MessageID, System.Type>()
        //{
        //    { MessageID.ReqLogin,typeof( ResLoginMessage) },
        //};
        //public static System.Type GetMsgType(MessageID msgId)
        //{
        //    return map[msgId];
        //}
        public static IMessage GetMessageData(MessageID messageID,byte[] data)
        {
            IMessage msgData = null;
            switch (messageID)
            {
                case MessageID.ResLogin:
                    ResLoginMessage resLoginMessage = ResLoginMessage.Parser.ParseFrom(data);
                    msgData = resLoginMessage;
                    break;
                case MessageID.ResRegister:
                    break;
                case MessageID.ResRandomName:
                    break;
                case MessageID.ResPropertyChange:
                    break;
                default:
                    break;
            }
            return msgData;
        }
    }
}
//public class MyMessage
//{
//    /// <summary>
//    /// 服务端下行通知消息集合
//    /// </summary>
//    static Dictionary<Type, Action<object>> serverPushResponseNotifys = new Dictionary<Type, Action<object>>();

//    //Single parameter
//    static public void AddListener<T>(string eventType, Callback<T> handler)
//    {
//        OnListenerAdding(eventType, handler);
//        eventTable[eventType] = (Callback<T>)eventTable[eventType] + handler;
//        //把订阅的带有下行协议的消息单独汇总,方便比对服务端下行cmd后发送通知
//        Type type = typeof(T);
//        if (!serverPushResponseNotifys.ContainsKey(type))
//        {
//            serverPushResponseNotifys.Add(type, (obj) =>
//            {
//                Broadcast<T>(eventType, (T)obj);
//            });
//        }
//    }

//    /// <summary>
//    /// 广播服务端下行消息
//    /// </summary>
//    /// <param name="type">Type.</param>
//    /// <param name="arg">Argument.</param>
//    public static void Broadcast(object arg)
//    {
//        Type t = arg.GetType();
//        if (serverPushResponseNotifys.ContainsKey(t))
//        {
//            serverPushResponseNotifys[t](arg);
//        }
//    }
//}
