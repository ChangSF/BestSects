using System.Collections;
using System.Collections.Generic;
using KSFramework;
using UnityEngine;
using UnityEngine.UI;
using XLua;
using BestSects.protocol;



public class UILogin1 : CSUIController
{
    private string serverListUrl="http://47.94.220.1/serverlist.html";
    public Button btnLogin, btnForgetPwd, btnRegister;
    public GameObject objLogen, objServers, objRegister;
    private Cookie Cookie;

    // Use this for initialization
    void Start () {
        btnLogin.onClick.RemoveAllListeners();
        btnLogin.onClick.AddListener(LoginServer);
        btnForgetPwd.onClick.RemoveAllListeners();
        btnForgetPwd.onClick.AddListener(ForgetPwd);
        btnRegister.onClick.RemoveAllListeners();
        btnRegister.onClick.AddListener(ToRegisterStep);
        Lua2csMessenger.Instance.AddListener("NetworkConnect", delegate () {
        OnOpen();

        }
        );
    }

    // Update is called once per frame
    void Update () {
		
	}
    void OnOpen() {
        Cookie.Get("serverListJson", delegate () {
            Log.Error("无法获取到服务器列表!"); }
     
        );



    }

    void LoginServer(){




    }
    void ForgetPwd() {


    }
    
    void ToRegisterStep (){

        objLogen.GetComponent<Canvas>().enabled = false;
        objServers.GetComponent<Canvas>().enabled = false;
        objRegister.GetComponent<Canvas>().enabled = true;
    }

    void Server() {
        ReqLoginMessage msg;
        msg.Token = token;

        
        
        
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

