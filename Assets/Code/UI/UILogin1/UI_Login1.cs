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

    private JsonData token;
    private string username,password;
    private Dictionary<string,string> loginServer;
    private List<Serverlist> serverlist;
    private struct Serverlist {
        public string server;
        public string ip;
        public string port;
        public string name;

    }
    private List<Roles> roles;
    private struct Roles
    {
        public  string level;
        public string nickname;
        public string serverID;
        public string sex;
        public string uid;
    }
    private bool bGetServerList = false;
    private List<GameObject> serverlistGOs;



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


        btnSignUP.onClick.RemoveAllListeners();
        btnSignUP.onClick.AddListener(ConnectServer);


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
        int x = serverListData.Count;
        serverlist.Clear();

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
            StartCoroutine(Routine_i());
        }
    }

    private IEnumerator Routine_i()
    {
        string l_username = inputID.text;
        string l_password = inputPassword.text;

        string url = "http://" + loginServer["ip"] + ":" + loginServer["port"] + "/loGIN?UsERnaME=" + l_username + "&pASsWOrD=" + l_password;

        Debug.LogError(url);
        WWW www = new WWW(url);

        yield return www;
        Debug.LogError(www.text);
        if( www.error == null || www.error == "")
        {
            username = l_username;
            password = l_password;
            ProcessLoginServerData(www.text);

        }
        else
        {
            Debug.LogError("网络出错,无法访问登录服务器!");
        }
    }



    private void ProcessLoginServerData(string jsonText)
    {
        JsonReader reader = new JsonReader(jsonText);
        JsonData data = new JsonData();
        data = JsonMapper.ToObject(reader);
        string code = data["code"].ToString();
        if (code.ToInt32() == 0)
        {
            JsonData roles = data["roles"];
            this.roles.Clear();
            int x = roles.Count;
            for(int i = 0; i < x; i++)
            {
                Roles j = new Roles();
                j.level = data["level"].ToString();
                j.nickname = data["nickname"].ToString();
                j.serverID = data["serverID"].ToString();
                j.sex = data["sex"].ToString();
                j.uid = data["uid"].ToString();
                this.roles.Insert(i, j);
            }
            token = data["token"];
            Cookie.Set("token",token);
            Cookie.Set("roles",this.roles);
            objLogin.enabled = false;
            objServers.enabled = true;
            objRegister.enabled = false;
            InitServerList();
        }
        else
        {
            Debug.LogError("error code =>"+code);
        }
    }

    private void InitServerList()
    {
        foreach (Serverlist k in serverlist)
        {

        }
    }



    private void ForgetPwd()
    {


    }
    private void LoginServer()
    {
        ReqLoginMessage msg = new ReqLoginMessage();
        Debug.LogError("z => " + token);
        msg.Token = token.ToString();
        msg.Username = this.username;
        msg.Channel = 0; //玩家渠道
        msg.DeviceId = "abcdef"; //设备号
        NetworkModule.Instance.Send(MessageID.ReqLogin, msg);


    }

    private void ToRegisterStep()
    {
        objLogin.enabled = false;
        objRegister.enabled = true;
        objServers.enabled = false;

    }

    private void ConnectServer()
    {
       // NetworkModule.Instance.Connect(ip,port);
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

