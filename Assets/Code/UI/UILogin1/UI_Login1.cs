using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSFramework;
using UnityEngine.UI;
using LitJson;
using BestSects.protocol;
using BestSects.Net;
using KEngine.UI;

public class UI_Login1 : CSUIController
{
    private Button btnRegister, btnLogin, btnForgetPwd, btnSignIn, btnSignUP;
    private Canvas objRegister, objLogin, objServers;
    private InputField inputID, inputPassword;

    private string token;
    private string username;
    private Dictionary<string,string> loginServer;
    private List<Serverlist> serverlist;
    private struct Serverlist {
        public string server;
        public string ip;
        public string port;
        public string name;

    }
    private bool bGetServerList = false;




    public override void OnInit()
    {
     UILogic<UI_Login1>.Instance.UIReg(this);
        Debug.LogError(GetType().ToString() + ":OnInit");
        btnRegister = transform.Find("imgBackground/objLogin/imgGroupRegister/btnRegister").GetComponent<Button>();
        btnLogin = transform.Find("imgBackground/objLogin/imgGroupLogin/btnLogin").GetComponent<Button>();
        btnForgetPwd =transform.Find("imgBackground/objLogin/imgGroupLogin/btnForgetPwd").GetComponent<Button>();
        btnSignIn = transform.Find("imgBackground/objRegister/imgGroupRegister/btnSignIn").GetComponent<Button>();
        btnSignUP = transform.Find("imgBackground/objServers/imgGroupHaveLogined/btnSignUp").GetComponent<Button>();
        objRegister = transform.Find("imgBackground/objRegister").GetComponent<Canvas>();
        objLogin = transform.Find("imgBackground/objLogin").GetComponent<Canvas>();
        objServers = transform.Find("imgBackground/objServers").GetComponent<Canvas>();
        inputID= transform.Find("imgBackground/objLogin/imgGroupLogin").GetComponent<InputField>();
        inputPassword= transform.Find("imgBackground/objLogin/imgGroupLogin").GetComponent<InputField>();

        btnForgetPwd.onClick.RemoveAllListeners();
        btnForgetPwd.onClick.AddListener(ForgetPwd);

        btnRegister.onClick.RemoveAllListeners();
        btnRegister.onClick.AddListener(ToRegisterStep);

        btnLogin.onClick.RemoveAllListeners();
        btnLogin.onClick.AddListener(() => 
        {
            if (!bGetServerList)
            {
                return;
            }
            Login_LoginServer();

        });
    }

    public override void OnOpen(params object[] args)
    {
        Debug.LogError(GetType().ToString() + ":OnOpen");
        Debug.LogError("================================ WND_Login:OnOpen ============================");
        Messenger.AddListener("NetworkConnect", () =>
       {
           Debug.LogError("连接成功");
           LoginServer();
       });
        Messenger.AddListener("CurrentPlayerInfo", (JsonData a) =>
        {

            if (a["Code"].ToString().Equals("0"))
            {
                Cookie.Set("CurrentPlayerInfo", a["PlayerInfo"]);
                UIModule.Instance.CloseWindow("WND_Login1");
                UIModule.Instance.OpenWindow("WND_Introduce", "user1");

            }
        });

            object serverListJson = Cookie.Get("serverListJson", () =>
            {
                Debug.LogError("无法获取到服务器列表");
                return null;
            });
        Debug.LogError(serverListJson.ToString());

        JsonReader reader = new JsonReader(serverListJson.ToString());

        JsonData data = new JsonData();
        data =  JsonMapper.ToObject(reader);

        loginServer = new Dictionary<string, string>();
        JsonData LoginServerData = data["login"];
        loginServer.Add("ip", LoginServerData["ip"].ToString());
        loginServer.Add("port", LoginServerData["port"].ToString());

        List<string> gameServerList = new List<string>();
        JsonData serverListData = data["game"];
        int x;
        if (!int.TryParse(serverListData["Count"].ToString(),out x))
        {
            x = 0;
        }

        for (int i=0;i<x; i++)
        {
            JsonData item = serverListData[i];
            Serverlist j = new Serverlist();
            j.server = item["server"].ToString();
            j.ip = item["ip"].ToString();
            j.port = item["port"].ToString();
            j.name = item["name"].ToString();
            serverlist.Insert(i,j );
            Debug.LogError(item["ip"].ToString());
            bGetServerList = true;
        }


    }

    public override void OnClose()
    {
        UILogic<UI_Login1>.Instance.UIUnReg();
        Debug.LogError(GetType().ToString() + ":OnClose");
    }


    public void Login_LoginServer()
    {
        if (inputID.text == "" || inputPassword.text=="")
        {



        }


    }




    public void ForgetPwd()
    {


    }
    public void LoginServer()
    {
        ReqLoginMessage msg = new ReqLoginMessage();
        Debug.LogError("z => " + token);
        msg.Token = token;
        msg.Username = this.username;
        msg.Channel = 0; //玩家渠道
        msg.DeviceId = "abcdef"; //设备号
        NetworkModule.Instance.Send(MessageID.ReqLogin, msg);


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

