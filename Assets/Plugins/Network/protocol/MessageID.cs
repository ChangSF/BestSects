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
        ReqUserLogin = 110,
        ReqUserRegister = 111,

        ResUserLogin = 210,
        ResUserRegister = 211,
    }

    public class MessageMap
    {
        static readonly Dictionary<MessageID, System.Type> map = new Dictionary<MessageID, System.Type>()
        {
            { MessageID.ReqUserLogin,typeof( ReqUserLoginMessage) },
        };
        public static System.Type GetMsgType(MessageID msgId)
        {
            return map[msgId];
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
