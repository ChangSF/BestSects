using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSFramework;
using UnityEngine.UI;


public class UI_Login1 : CSUIController
{
    private Button btnRegister, btinLogin, btnForgetPwd, btnSignIn, btnSignUP;
    private Canvas objRegister, objLogin, objServers;


    public override void OnInit()
    {
     UILogic<UI_Login1>.Instance.UIReg(this);
        Debug.LogError(GetType().ToString() + ":OnInit");
        btnRegister = transform.Find("imgBackground/objLogin/imgGroupRegister/btnRegister").GetComponent<Button>();
        btinLogin = transform.Find("imgBackground/objLogin/imgGroupLogin/btnLogin").GetComponent<Button>();
        btnForgetPwd =transform.Find("imgBackground/objLogin/imgGroupLogin/btnForgetPwd").GetComponent<Button>();
        btnSignIn = transform.Find("imgBackground/objRegister/imgGroupRegister/btnSignIn").GetComponent<Button>();
        btnSignUP = transform.Find("imgBackground/objServers/imgGroupHaveLogined/btnSignUP").GetComponent<Button>();
        objRegister = transform.Find("imgBackground/objRegister").GetComponent<Canvas>();
        objLogin = transform.Find("imgBackground/objLogin").GetComponent<Canvas>();
        objServers = transform.Find("imgBackground/objServers").GetComponent<Canvas>();


        btnForgetPwd.onClick.RemoveAllListeners();
        btnForgetPwd.onClick.AddListener(ForgetPwd);

        btnRegister.onClick.RemoveAllListeners();
        btnRegister.onClick.AddListener(ToRegisterStep);

    }

    public override void OnOpen(params object[] args)
    {
        Debug.LogError(GetType().ToString() + ":OnOpen");
    }

    public override void OnClose()
    {
        UILogic<UI_Login1>.Instance.UIUnReg();
        Debug.LogError(GetType().ToString() + ":OnClose");
    }

    public void ForgetPwd()
    {


    }

    public void ToRegisterStep()
    {
        objLogin.enabled = false;
        objRegister.enabled = true;
        objServers.enabled = false;


    }


}





//function WND_Login:LoginServer()
//    local msg = ReqLoginMessage();
//Log.Error("z =>"..tostring(self.token));
//    msg.Token =tostring(self.token);--登录令牌
//    msg.Username = self.username;--帐号
//    msg.Channel = 0;--玩家渠道
//    msg.DeviceId = "abcdef";--设备号
//    NetworkModule.Instance:Send(MessageID.ReqLogin, msg);
//end

