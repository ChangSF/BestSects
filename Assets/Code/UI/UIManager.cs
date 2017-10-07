using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSFramework;
using KEngine.UI;
using XLua;
/// <summary>
/// UI管理器类，常驻内存，对UI生命周期做管理
/// </summary>
public class UIManager
{
    private UIManager instance = null;
    public UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UIManager();
            }
            return instance;
        }
    }
    public void Init()
    {

    }
    public void RegAllUIMessage()
    {
        UILogic<UI_Login>.Instance.Init();
    }

}
public abstract class UILogic
{
    public abstract void Init();
    public abstract void RegMessage();
    public abstract void UnResMessage();
    public abstract void ClearTempData();
}
[Hotfix]
public class UILogic<T> : UILogic where T : UIController
{
    private T _ctrl = null;

    public T Controller
    {
        get
        {
            return _ctrl;
        }
    }
    private static UILogic<T> instance = null;
    public static UILogic<T> Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UILogic<T>();
            }
            return instance;
        }
    }
    public UILogic()
    {

    }
    /// <summary>
    /// 将UI注册到logic上面
    /// </summary>
    /// <param name="uiController"></param>
    public void UIReg(T uiController)
    {
        _ctrl = uiController;
    }

    public void UIUnReg()
    {
        _ctrl = null;
    }

    public override void Init()
    {
        throw new System.NotImplementedException();
    }

    public override void RegMessage()
    {
        throw new System.NotImplementedException();
    }

    public override void UnResMessage()
    {
        throw new System.NotImplementedException();
    }

    public override void ClearTempData()
    {
        throw new System.NotImplementedException();
    }
}