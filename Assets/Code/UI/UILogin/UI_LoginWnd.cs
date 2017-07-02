using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSFramework;

public class UI_LoginWnd : CSUIController
{



    public override void OnInit()
    {
        Debug.LogError(GetType().ToString() + ":OnInit");
    }

    public override void OnOpen(params object[] args)
    {
        Debug.LogError(GetType().ToString() + ":OnOpen");
    }

    public override void OnClose()
    {
        Debug.LogError(GetType().ToString() + ":OnClose");
    }
}
