﻿using System;
using UnityEngine;
using System.Collections;
using System.IO;
using KEngine;
using KEngine.UI;
using SLua;
using UnityEngine.UI;

namespace KSFramework
{
    /// <summary>
    /// 自动根据UIName和UITemplateName，寻找适合的Lua脚本执行
    /// </summary>
    public class LuaUIController : KEngine.UI.UIController
    {
        /// <summary>
        /// 一般编辑器模式下用于reload时用，记录上一次OnOpen的参数
        /// </summary>
        public object[] LastOnOpenArgs { get; private set; }
        LuaTable _luaTable;

        /// <summary>
        /// Lua Script for this UI 's path
        /// </summary>
        public string UILuaPath
        {
            get
            {
                var relPath = string.Format("UI/{0}/{0}", UITemplateName);
                return relPath;
            }
        }

        /// <summary>
        /// Whether or not cache mode
        /// </summary>
        protected virtual bool IsCachedLuaTable
        {
            get
            {
                return LuaModule.CacheMode;
            }
        }

        public override void OnInit()
        {
            base.OnInit();

            if (!CheckInitScript(true))
            {
                CSUIController csController = this.gameObject.GetComponent<CSUIController>();
                if (csController)
                    csController.OnInit();
                return;
            }
        }

        /// <summary>
        /// 调用Lua:OnOpen函数
        /// </summary>
        /// <param name="args"></param>
        public override void OnOpen(params object[] args)
        {
            // 编辑器模式下，记录
            LastOnOpenArgs = args;

            base.OnOpen(args);
            if (!CheckInitScript())
            {
                CSUIController csController = this.gameObject.GetComponent<CSUIController>();
                if (csController)
                    csController.OnOpen(args);
                return;
            }

            var onOpenFuncObj = _luaTable["OnOpen"];
            if (onOpenFuncObj == null)
            {
                Log.LogError("Not Exists `OnOpen` in lua: {0}", UITemplateName);
                return;
            }

            var newArgs = new object[args.Length + 1];
            newArgs[0] = _luaTable;
            for (var i = 0; i < args.Length; i++)
            {
                newArgs[i + 1] = args[i];
            }

            (onOpenFuncObj as LuaFunction).call(newArgs);
        }

        public override void OnClose()
        {
            base.OnClose();
            if (!CheckInitScript())
            {
                CSUIController csController = this.gameObject.GetComponent<CSUIController>();
                if (csController)
                    csController.OnClose();
                return;
            }
            var closeFunc = _luaTable["OnClose"];
            if (closeFunc != null)
            {
                (closeFunc as LuaFunction).call(_luaTable);
            }
        }

        /// <summary>
        /// Try to load script and init.
        /// Script will be cached,
        /// But in development, script cache can be clear, which will be load and init in the next time
        /// 
        /// 开发阶段经常要使用Lua热重载，热重载过后，要确保OnInit重新执行
        /// </summary>
        bool CheckInitScript(bool showWarn = false)
        {
            if (!IsCachedLuaTable)
            {
                ClearLuaTableCache();
            }

            var relPath = UILuaPath;

            var luaModule = KSGame.Instance.LuaModule;
            object scriptResult;
            if (!luaModule.TryImport(relPath, out scriptResult))
            {
                if (showWarn)
                    Log.LogWarning("Import UI Lua Script failed: {0}", relPath);
                return false;
            }
            //存在lua脚本，那么优先使用lua
            if (this.gameObject.GetComponent<CSUIController>())
            {
                if (this.gameObject.GetComponent<CSUIController>().useLuaFirst)
                    Destroy(this.gameObject.GetComponent<CSUIController>());
                else
                    return false;
            }
            scriptResult = KSGame.Instance.LuaModule.CallScript(relPath);
            Debuger.Assert(scriptResult is LuaTable, "{0} Script Must Return Lua Table with functions!", UITemplateName);

            _luaTable = scriptResult as LuaTable;

            var newFuncObj = _luaTable["New"]; // if a New function exist, new a table!
            if (newFuncObj != null)
            {
                var newTableObj = (newFuncObj as LuaFunction).call(this);
                _luaTable = newTableObj as LuaTable;
            }

            var outlet = this.GetComponent<UILuaOutlet>();
            if (outlet != null)
            {
                for (var i = 0; i < outlet.OutletInfos.Count; i++)
                {
                    var outletInfo = outlet.OutletInfos[i];

                    var gameObj = outletInfo.Object as GameObject;
                    if (gameObj != null)
                        _luaTable[outletInfo.Name] = gameObj.GetComponent(outletInfo.ComponentType);
                    else
                        _luaTable[outletInfo.Name] = gameObj;
                    if (_luaTable[outletInfo.Name] == null)
                    {
                        //switch (outletInfo.ComponentType)
                        //{
                        //    case "UnityEngine.RectTransform":
                        //        _luaTable[outletInfo.Name] = gameObj.GetComponent<RectTransform>();
                        //        break;
                        //    case "UnityEngine.Transform":
                        //        _luaTable[outletInfo.Name] = gameObj.GetComponent<Transform>();
                        //        break;
                        //    case "UnityEngine.AudioSource":
                        //        _luaTable[outletInfo.Name] =gameObj.GetComponent<AudioSource>();
                        //        break;
                        //    default:
                        //        _luaTable[outletInfo.Name] = outletInfo.Object;
                        //        Debug.LogError("导入未知类型=> " + outletInfo.ComponentType);
                        //        break;
                        //}
                        _luaTable[outletInfo.Name] = gameObj.GetComponent(outletInfo.ComponentType.Substring(outletInfo.ComponentType.LastIndexOf('.') + 1));
                    }

                    if (_luaTable[outletInfo.Name] == null)
                    {
                        Debug.LogError("导入未知类型=> " + outletInfo.ComponentType);

                    }
                }

            }


            var luaInitObj = _luaTable["OnInit"];
            Debuger.Assert(luaInitObj is LuaFunction, "Must have OnInit function - {0}", UIName);

            // set table variable `Controller` to this
            _luaTable["Controller"] = this;

            (luaInitObj as LuaFunction).call(_luaTable, this);

            return true;
        }

        public UnityEngine.Object GetControl(string typeName, string uri, Transform findTrans)
        {
            return GetControl(typeName, uri, findTrans);
        }

        public UnityEngine.Object GetControl(string typeName, string uri)
        {
            return GetControl(typeName, uri, null, true);
        }

        public UnityEngine.Object GetControl(string typeName, string uri, Transform findTrans, bool isLog)
        {
            if (findTrans == null)
                findTrans = transform;

            Transform trans = findTrans.Find(uri);
            if (trans == null)
            {
                if (isLog)
                    Log.LogError("Get UI<{0}> Control Error: " + uri, this);
                return null;
            }

            if (typeName == "GameObject")
                return trans.gameObject;

            return trans.GetComponent(typeName);
        }

        /// <summary>
        /// 清理Lua脚本缓存，下次执行时将重新加载Lua
        /// </summary>
        public void ClearLuaTableCache()
        {
            _luaTable = null;

            var luaModule = KSGame.Instance.LuaModule;
            luaModule.ClearCache(UILuaPath);
            Log.Warning("Reload Lua: {0}", UILuaPath);
        }

    }
}
