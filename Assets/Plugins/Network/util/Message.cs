/*
 * Advanced C# messenger by Ilya Suzdalnitski. V1.0
 * 
 * Based on Rod Hyde's "CSharpMessenger" and Magnus Wolffelt's "CSharpMessenger Extended".
 * 
 * Features:
 	* Prevents a MissingReferenceException because of a reference to a destroyed message handler.
 	* Option to log all messages
 	* Extensive error detection, preventing silent bugs
 * 
 * Usage examples:
 	1. Messenger.AddListener<GameObject>("prop collected", PropCollected);
 	   Messenger.Broadcast<GameObject>("prop collected", prop);
 	2. Messenger.AddListener<float>("speed changed", SpeedChanged);
 	   Messenger.Broadcast<float>("speed changed", 0.5f);
 * 
 * Messenger cleans up its evenTable automatically upon loading of a new level.
 * 
 * Don't forget that the messages that should survive the cleanup, should be marked with Messenger.MarkAsPermanent(string)
 * 
 */

//#define LOG_ALL_MESSAGES
//#define LOG_ADD_LISTENER
//#define LOG_BROADCAST_MESSAGE
//#define LOG_BROADCAST_PROCESS
//#define REQUIRE_LISTENER

using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void Callback();
public delegate void Callback<T>(T arg1);
public delegate void Callback<T, U>(T arg1, U arg2);
public delegate void Callback<T, U, V>(T arg1, U arg2, V arg3);

static public class Messenger
{
    #region Internal variables

    //Disable the unused variable warning
#pragma warning disable 0414
    //Ensures that the MessengerHelper will be created automatically upon start of the game.
    static private MessengerHelper messengerHelper = (new GameObject("MessengerHelper")).AddComponent<MessengerHelper>();
#pragma warning restore 0414
    static public Dictionary<string, Delegate> m_eventListenTable = new Dictionary<string, Delegate>();     // 监听列表

    //Message handlers that should never be removed, regardless of calling Cleanup
    static public List<string> permanentMessages = new List<string>();

    #endregion

    #region Broadcast class define

    // for async broadcast
    public abstract class AsyncMsgInfoBase
    {
        public string eventType;
        protected string strLog;

        public AsyncMsgInfoBase(string type)
        {
            eventType = type;
        }
        public virtual void Excute()
        {
            strLog = "";

#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
            strLog = "MESSENGER EXCUTE\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\tInvoking \t\"" + eventType + "\"";
#endif
        }
    }
    public class AsyncMsgInfo : AsyncMsgInfoBase
    {
        private Callback cb;

        public AsyncMsgInfo(string type) : base(type) { }
        public void SetParam(Callback c)
        {
            cb = c;
        }
        public override void Excute()
        {
            base.Excute();

#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
            strLog += "\n" + "none param";
            Delegate d = cb as Delegate;
            if (d != null)
            {
                Delegate[] cb_list = d.GetInvocationList();
                foreach (Delegate cb_tmp in cb_list)
                {
                    strLog += "\n" + "{" + cb_tmp.Target + " -> " + cb_tmp.Method + "}";
                }
            }
            Debug.Log(strLog);
#endif
            cb();
        }
    }
    public class AsyncMsgInfo<T> : AsyncMsgInfoBase
    {
        private Callback<T> cb;
        private T param1;

        public AsyncMsgInfo(string type) : base(type) { }
        public void SetParam(Callback<T> c, T p)
        {
            cb = c;
            param1 = p;
        }
        public override void Excute()
        {
            base.Excute();

#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
            strLog += "[Param1]" + param1.ToString();
            Delegate d = cb as Delegate;
            if (d != null)
            {
                Delegate[] cb_list = d.GetInvocationList();
                foreach (Delegate cb_tmp in cb_list)
                {
                    strLog += "\n" + "{" + cb_tmp.Target + " -> " + cb_tmp.Method + "}";
                }
            }
            Debug.Log(strLog);
#endif
            cb(param1);
        }
    }
    public class AsyncMsgInfo<T, U> : AsyncMsgInfoBase
    {
        private Callback<T, U> cb;
        private T param1;
        private U param2;

        public AsyncMsgInfo(string type) : base(type) { }
        public void SetParam(Callback<T, U> c, T p1, U p2)
        {
            cb = c;
            param1 = p1;
            param2 = p2;
        }
        public override void Excute()
        {
            base.Excute();

#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
            Debug.Log("[Param1]" + param1.ToString() + " [param2]" + param2.ToString());

            Delegate d = cb as Delegate;
            if (d != null)
            {
                Delegate[] cb_list = d.GetInvocationList();
                foreach (Delegate cb_tmp in cb_list)
                {
                    Debug.Log("{" + cb_tmp.Target + " -> " + cb_tmp.Method + "}");
                }
            }
#endif
            cb(param1, param2);
        }
    }
    public class AsyncMsgInfo<T, U, V> : AsyncMsgInfoBase
    {
        private Callback<T, U, V> cb;
        private T param1;
        private U param2;
        private V param3;

        public AsyncMsgInfo(string type) : base(type) { }
        public void SetParam(Callback<T, U, V> c, T p1, U p2, V p3)
        {
            cb = c;
            param1 = p1;
            param2 = p2;
            param3 = p3;
        }
        public override void Excute()
        {
            base.Excute();

#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
            Debug.Log("[Param1]" + param1.ToString() + " [param2]" + param2.ToString() + " [param3]" + param3.ToString());

            Delegate d = cb as Delegate;
            if (d != null)
            {
                Delegate[] cb_list = d.GetInvocationList();
                foreach (Delegate cb_tmp in cb_list)
                {
                    Debug.Log("{" + cb_tmp.Target + " -> " + cb_tmp.Method + "}");
                }
            }
#endif
            cb(param1, param2, param3);
        }
    }

    #endregion

    static private List<AsyncMsgInfoBase> m_eventTypeAsyncList = new List<AsyncMsgInfoBase>();    // 事件队列
    static private List<AsyncMsgInfoBase> m_eventForUpdate = new List<AsyncMsgInfoBase>();        // 准备更新的列表

    #region Helper methods
    //Marks a certain message as permanent.
    static public void MarkAsPermanent(string eventType)
    {
#if LOG_ALL_MESSAGES
	Debug.Log("Messenger MarkAsPermanent \t\"" + eventType + "\"");
#endif
        permanentMessages.Add(eventType);
    }


    static public void Cleanup()
    {
#if LOG_ALL_MESSAGES
	Debug.Log("MESSENGER Cleanup. Make sure that none of necessary listeners are removed.");
#endif

        List<string> messagesToRemove = new List<string>();

        foreach (KeyValuePair<string, Delegate> pair in m_eventListenTable)
        {
            bool wasFound = false;

            foreach (string message in permanentMessages)
            {
                if (pair.Key == message)
                {
                    wasFound = true;
                    break;
                }
            }

            if (!wasFound)
                messagesToRemove.Add(pair.Key);
        }

        foreach (string message in messagesToRemove)
        {
            Remove(message);
        }

    }

    static private void Remove(string message)
    {
        // remove from ayncevent
        int nCount = m_eventTypeAsyncList.Count;
        for (int i = 0; i < nCount; ++i)
        {
            if (m_eventTypeAsyncList[i].eventType == message)
            {
                m_eventTypeAsyncList.RemoveAt(i);

                i = 0;
                nCount = m_eventTypeAsyncList.Count;
            }
        }

        // remove from m_eventListenTable
        m_eventListenTable.Remove(message);
    }
    #endregion

    #region Message logging and exception throwing
    static public void OnListenerAdding(string eventType, Delegate listenerBeingAdded)
    {
#if LOG_ALL_MESSAGES || LOG_ADD_LISTENER
	Debug.Log("MESSENGER OnListenerAdding \"" + eventType + "\"{" + listenerBeingAdded.Target + " -> " + listenerBeingAdded.Method + "}");
#endif

        if (!m_eventListenTable.ContainsKey(eventType))
        {
            m_eventListenTable.Add(eventType, null);
        }

        Delegate d = m_eventListenTable[eventType];
        if (d != null && d.GetType() != listenerBeingAdded.GetType())
        {
            throw new ListenerException(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventType, d.GetType().Name, listenerBeingAdded.GetType().Name));
        }
    }

    static public bool OnListenerRemoving(string eventType, Delegate listenerBeingRemoved)
    {
#if LOG_ALL_MESSAGES
	Debug.Log("MESSENGER OnListenerRemoving \t\"" + eventType + "\"\t{" + listenerBeingRemoved.Target + " -> " + listenerBeingRemoved.Method + "}");
#endif

        if (m_eventListenTable.ContainsKey(eventType))
        {
            Delegate d = m_eventListenTable[eventType];

            if (d == null)
            {
                throw new ListenerException(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", eventType));
            }
            else if (d.GetType() != listenerBeingRemoved.GetType())
            {
                throw new ListenerException(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType, d.GetType().Name, listenerBeingRemoved.GetType().Name));
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    static public void OnBroadcasting(string eventType, bool bAsync)
    {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
        Debug.Log("MESSENGER SEND " + (bAsync ? "ASYNC\t" : "\t") + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\tInvoking \t\"" + eventType + "\"");
#endif

#if REQUIRE_LISTENER
        if (!m_eventListenTable.ContainsKey(eventType))
        {
            throw new BroadcastException(string.Format("Broadcasting message \"{0}\" but no listener found. Try marking the message with Messenger.MarkAsPermanent.", eventType));
        }
#endif
    }

    static public BroadcastException CreateBroadcastSignatureException(string eventType)
    {
        return new BroadcastException(string.Format("Broadcasting message \"{0}\" but listeners have a different signature than the broadcaster.", eventType));
    }

    public class BroadcastException : Exception
    {
        public BroadcastException(string msg)
            : base(msg)
        {
        }
    }

    public class ListenerException : Exception
    {
        public ListenerException(string msg)
            : base(msg)
        {
        }
    }
    #endregion

    #region AddListener
    //No parameters
    static public void AddListener(string eventType, Callback handler)
    {
        OnListenerAdding(eventType, handler);
        m_eventListenTable[eventType] = (Callback)m_eventListenTable[eventType] + handler;
    }

    //Single parameter
    static public void AddListener<T>(string eventType, Callback<T> handler)
    {
        OnListenerAdding(eventType, handler);
        m_eventListenTable[eventType] = (Callback<T>)m_eventListenTable[eventType] + handler;
    }

    //Two parameters
    static public void AddListener<T, U>(string eventType, Callback<T, U> handler)
    {
        OnListenerAdding(eventType, handler);
        m_eventListenTable[eventType] = (Callback<T, U>)m_eventListenTable[eventType] + handler;
    }

    //Three parameters
    static public void AddListener<T, U, V>(string eventType, Callback<T, U, V> handler)
    {
        OnListenerAdding(eventType, handler);
        m_eventListenTable[eventType] = (Callback<T, U, V>)m_eventListenTable[eventType] + handler;
    }
    #endregion

    #region RemoveListener
    //No parameters
    static public void RemoveListener(string eventType, Callback handler)
    {
        if (OnListenerRemoving(eventType, handler))
        {
            m_eventListenTable[eventType] = (Callback)m_eventListenTable[eventType] - handler;
            if (m_eventListenTable[eventType] == null)
            {
                Remove(eventType);
            }
        }
    }

    //Single parameter
    static public void RemoveListener<T>(string eventType, Callback<T> handler)
    {
        if (OnListenerRemoving(eventType, handler))
        {
            m_eventListenTable[eventType] = (Callback<T>)m_eventListenTable[eventType] - handler;
            if (m_eventListenTable[eventType] == null)
            {
                Remove(eventType);
            }
        }
    }

    //Two parameters
    static public void RemoveListener<T, U>(string eventType, Callback<T, U> handler)
    {
        if (OnListenerRemoving(eventType, handler))
        {
            m_eventListenTable[eventType] = (Callback<T, U>)m_eventListenTable[eventType] - handler;
            if (m_eventListenTable[eventType] == null)
            {
                Remove(eventType);
            }
        }
    }

    //Three parameters
    static public void RemoveListener<T, U, V>(string eventType, Callback<T, U, V> handler)
    {
        if (OnListenerRemoving(eventType, handler))
        {
            m_eventListenTable[eventType] = (Callback<T, U, V>)m_eventListenTable[eventType] - handler;
            if (m_eventListenTable[eventType] == null)
            {
                Remove(eventType);
            }
        }
    }
    #endregion

    #region Broadcast definition

    //No parameters
    static public void Broadcast(string eventType)
    {
        DoBroadcast(eventType, false);
    }
    static public void BroadcastAsync(string eventType)
    {
        DoBroadcast(eventType, true);
    }

    //Single parameter
    static public void Broadcast<T>(string eventType, T arg1)
    {
        DoBroadcast<T>(eventType, arg1, false);
    }
    static public void BroadcastAsync<T>(string eventType, T arg1)
    {
        DoBroadcast<T>(eventType, arg1, true);
    }

    //Two parameters
    static public void Broadcast<T, U>(string eventType, T arg1, U arg2)
    {
        DoBroadcast<T, U>(eventType, arg1, arg2, false);
    }
    static public void BroadcastAsync<T, U>(string eventType, T arg1, U arg2)
    {
        DoBroadcast<T, U>(eventType, arg1, arg2, true);
    }

    //Three parameters
    static public void Broadcast<T, U, V>(string eventType, T arg1, U arg2, V arg3)
    {
        DoBroadcast<T, U, V>(eventType, arg1, arg2, arg3, false);
    }
    static public void BroadcastAsync<T, U, V>(string eventType, T arg1, U arg2, V arg3)
    {
        DoBroadcast<T, U, V>(eventType, arg1, arg2, arg3, true);
    }

    #endregion

    #region Broadcast implementation

    static void ReadyBroadcast(AsyncMsgInfoBase info, bool bAsync)
    {
        if (info != null)
        {
            if (bAsync)
            {
                m_eventTypeAsyncList.Add(info);
            }
            else
            {
                info.Excute();
                RemoveMsgInfo(info);
            }
        }
    }

    static private void DoBroadcast(string eventType, bool bAsync)
    {
        OnBroadcasting(eventType, bAsync);
        ReadyBroadcast(GetMsgInfo(eventType), bAsync);
    }
    static private void DoBroadcast<T>(string eventType, T arg1, bool bAsync)
    {
        OnBroadcasting(eventType, bAsync);
        ReadyBroadcast(GetMsgInfo(eventType, arg1), bAsync);
    }
    static private void DoBroadcast<T, U>(string eventType, T arg1, U arg2, bool bAsync)
    {
        OnBroadcasting(eventType, bAsync);
        ReadyBroadcast(GetMsgInfo(eventType, arg1, arg2), bAsync);
    }
    static private void DoBroadcast<T, U, V>(string eventType, T arg1, U arg2, V arg3, bool bAsync)
    {
        OnBroadcasting(eventType, bAsync);
        ReadyBroadcast(GetMsgInfo(eventType, arg1, arg2, arg3), bAsync);
    }

    #endregion

    #region Broadcast implementation for Async
    static public void Update()
    {
        // add all event to update list
        int nCount = m_eventTypeAsyncList.Count;
        for (int i = 0; i < nCount; ++i)
        {
            m_eventForUpdate.Add(m_eventTypeAsyncList[i]);
        }
        m_eventTypeAsyncList.Clear();

        // call back
        DoProcessCallbacks(m_eventForUpdate);
        m_eventForUpdate.Clear();
    }

    static public void DoProcessCallbacks(List<AsyncMsgInfoBase> eventTypeList)
    {
        if (eventTypeList.Count > 0)
        {
            int nCount = eventTypeList.Count;
            for (int i = 0; i < nCount; ++i)
            {
                if (m_eventListenTable.ContainsKey(eventTypeList[i].eventType))
                {
                    eventTypeList[i].Excute();
                    RemoveMsgInfo(eventTypeList[i]);
                }
                else
                {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
                    Debugger.LogError("MESSENGER ASYNC EXCUTE FAIL:has unlistened\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\tInvoking \t\"" + eventTypeList[i].eventType + "\"");
#endif
                }
            }
        }
    }
    #endregion

    #region 对象池
    static private Dictionary<string, AsyncMsgInfoBase> m_eventTypeList_Pool = new Dictionary<string, AsyncMsgInfoBase>();    // 缓存池，每个事件都缓存

    // 得到一个对象
    static AsyncMsgInfoBase GetMsgInfo(string eventType)
    {
        Delegate d;
        if (m_eventListenTable.TryGetValue(eventType, out d))
        {
            Callback callback = d as Callback;

            if (callback != null)
            {
                AsyncMsgInfo info = GetFromPool(eventType) as AsyncMsgInfo;
                if (info == null)
                {
                    info = new AsyncMsgInfo(eventType);
                }
                info.SetParam(callback);

                return info;
            }
        }

        return null;
    }

    static AsyncMsgInfoBase GetMsgInfo<T>(string eventType, T arg1)
    {
        Delegate d;
        if (m_eventListenTable.TryGetValue(eventType, out d))
        {
            Callback<T> callback = d as Callback<T>;
            if (callback != null)
            {
                AsyncMsgInfo<T> info = GetFromPool(eventType) as AsyncMsgInfo<T>;
                if (info == null)
                {
                    info = new AsyncMsgInfo<T>(eventType);
                }
                info.SetParam(callback, arg1);

                return info;
            }
        }

        return null;
    }

    static AsyncMsgInfoBase GetMsgInfo<T, U>(string eventType, T arg1, U arg2)
    {
        Delegate d;
        if (m_eventListenTable.TryGetValue(eventType, out d))
        {
            Callback<T, U> callback = d as Callback<T, U>;
            if (callback != null)
            {
                AsyncMsgInfo<T, U> info = GetFromPool(eventType) as AsyncMsgInfo<T, U>;
                if (info == null)
                {
                    info = new AsyncMsgInfo<T, U>(eventType);
                }
                info.SetParam(callback, arg1, arg2);

                return info;
            }
        }

        return null;
    }

    static AsyncMsgInfoBase GetMsgInfo<T, U, V>(string eventType, T arg1, U arg2, V arg3)
    {
        Delegate d;
        if (m_eventListenTable.TryGetValue(eventType, out d))
        {
            Callback<T, U, V> callback = d as Callback<T, U, V>;
            if (callback != null)
            {
                AsyncMsgInfo<T, U, V> info = GetFromPool(eventType) as AsyncMsgInfo<T, U, V>;
                if (info == null)
                {
                    info = new AsyncMsgInfo<T, U, V>(eventType);
                }
                info.SetParam(callback, arg1, arg2, arg3);

                return info;
            }
        }

        return null;
    }


    static void RemoveMsgInfo(AsyncMsgInfoBase eventInfo)
    {
        AddToPool(eventInfo);
    }

    static void AddToPool(AsyncMsgInfoBase eventInfo)
    {
        if (m_eventTypeList_Pool.ContainsKey(eventInfo.eventType) == false)
        {
            m_eventTypeList_Pool.Add(eventInfo.eventType, eventInfo);
        }
    }

    static AsyncMsgInfoBase GetFromPool(string eventType)
    {
        if (m_eventTypeList_Pool.ContainsKey(eventType))
        {
            AsyncMsgInfoBase obj = m_eventTypeList_Pool[eventType];
            m_eventTypeList_Pool.Remove(eventType);
            return obj;
        }
        else
        {
            return null;
        }
    }

    #endregion
}
public sealed class MessengerHelper : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    //Clean up eventTable every time a new level loads.
    public void OnLevelWasLoaded(int unused)
    {
        //Messenger.Cleanup();
    }
    public void OnDisable()
    {
        Messenger.Cleanup();
    }
    private void Update()
    {
        Messenger.Update();
    }
}
