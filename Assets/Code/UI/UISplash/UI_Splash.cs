using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KSFramework;
using DG.Tweening;
using KEngine.UI;

public class UI_Splash : UIController
{
    public string wndName = "";
    public string args = "";
    public Image bg;
    public Text progress;
    public Slider slider;

    float currentProgress;
    Tweener tweener;
    // Use this for initialization
    void Start()
    {
        currentProgress = 0f;
        //http://47.94.220.1/serverlist.html
        slider.value = 0f;
        tweener = slider.DOValue(currentProgress, 0.5f);

    }

    // Update is called once per frame
    void Update()
    {
        progress.text = Game.Instance.LuaModule.InitProgress.ToString();
        currentProgress = (float)Game.Instance.LuaModule.InitProgress / 100f;
        tweener.ChangeStartValue(slider.value).ChangeEndValue(currentProgress, 0.5f).PlayForward();
        if (currentProgress == 1f)
        {
            UIModule.Instance.OpenWindow(wndName, args);
            Destroy(gameObject);
        }
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
