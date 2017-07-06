using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KEngine.UI;
using KSFramework;





public class UIStudentInfo : CSUIController  {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnInit()
    {
        Debug.LogError("OnInit");
    }

    public override void OnOpen(params object[] args)
    {
        Debug.LogError("OnOpen");
    }

    public override void OnClose()
    {
        Debug.LogError("OnOpen");
    }
}
