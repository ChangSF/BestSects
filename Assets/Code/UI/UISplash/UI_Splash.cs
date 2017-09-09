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
    public Text tips;
    public string url = @"http://47.94.220.1/serverlist.html";
    float currentProgress;
    Tweener tweener;
    bool bGetServerList = false;
    bool bGameStart = false;
    // Use this for initialization
    void Start()
    {
        Messenger.AddListener(GameDefine.MessageId_Local.GameStart.ToString(), () => { bGameStart = true; });
        currentProgress = 0f;
        slider.value = 0f;
        tweener = slider.DOValue(currentProgress, 0.5f);
        tips.text = url;
        StartCoroutine(GetServerList());
    }

    // Update is called once per frame
    void Update()
    {
        //progress.text = Game.Instance.LuaModule.InitProgress.ToString();
        //currentProgress = (float)Game.Instance.LuaModule.InitProgress / 100f;
        //tweener.ChangeStartValue(slider.value).ChangeEndValue(currentProgress, 0.5f).PlayForward();
        if (Game.Instance.LuaModule.InitProgress >= 1 && bGetServerList && bGameStart)
        {
            UIModule.Instance.OpenWindow(wndName, args);
            Destroy(gameObject);
        }
        else if (bGetServerList || bGameStart)
        {
            currentProgress = 50f;
            tweener.ChangeStartValue(slider.value).ChangeEndValue(currentProgress, 0.5f).PlayForward();
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

    IEnumerator GetServerList()
    {
        while (!bGetServerList)
        {
            WWW www = new WWW(url);
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                tips.text = "网络异常!请检查网络后重试(3)!";
                yield return new WaitForSecondsRealtime(1f);
                tips.text = "网络异常!请检查网络后重试(2)!";
                yield return new WaitForSecondsRealtime(1f);
                tips.text = "网络异常!请检查网络后重试(1)!";
                yield return new WaitForSecondsRealtime(1f);
                tips.text = "网络异常!正在重试!";
                continue;
            }
            Cookie.Set("serverListJson", www.text);
            bGetServerList = true;
            tips.text = "网络正常!";
        }
    }

}
