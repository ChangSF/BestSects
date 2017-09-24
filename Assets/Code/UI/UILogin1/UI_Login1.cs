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
    private InputField inputID, inputPassword, inputRegID,inputRegPwd,inputRegPwdAgain;
    private ToggleGroup toggleGroupRegister;
    private Text labBtmServer;
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
    private List<GameObject> serverlistGOs=new List<GameObject>();
    private GameObject togExampleServer;
  //  private Transform serverContent;
    private int selectedIndex = 0;



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
        inputID= transform.Find("imgBackground/objLogin/imgGroupLogin/inputID").GetComponent<InputField>();
        inputPassword= transform.Find("imgBackground/objLogin/imgGroupLogin/inputPassword").GetComponent<InputField>();
        inputRegID = transform.Find("imgBackground/objRegister/imgGroupRegister/inputRegID").GetComponent<InputField>();
        inputRegPwd = transform.Find("imgBackground/objRegister/imgGroupRegister/inputRegPwd").GetComponent<InputField>();
        inputRegPwdAgain = transform.Find("imgBackground/objRegister/imgGroupRegister/inputRegPwdAgain").GetComponent<InputField>();
        labBtmServer= transform.Find("imgBackground/objServers/txtBeChosen/labBtmServer").GetComponent<Text>();

        toggleGroupRegister = transform.Find("imgBackground/objServers/imgGroupRegister/ScrollView/Viewport/Content").GetComponent<ToggleGroup>();




        togExampleServer = transform.Find("imgBackground/objServers/togExampleServer").GetComponent<Toggle>().gameObject;

        btnForgetPwd.onClick.RemoveAllListeners();
        btnForgetPwd.onClick.AddListener(ForgetPwd);

        btnRegister.onClick.RemoveAllListeners();
        btnRegister.onClick.AddListener(ToRegisterStep);

        btnLogin.onClick.RemoveAllListeners();
        btnLogin.onClick.AddListener(() => 
        {
            if (!bGetServerList)
            {
                Debug.LogError("Serverlist is not available");
                return;
            }
            Debug.LogError("logining");
            Login_LoginServer();

        });

        btnSignIn.onClick.RemoveAllListeners();
        btnSignIn.onClick.AddListener(Reg_LoginServer);


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
        serverlist = new List<Serverlist>();

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


    private void Login_LoginServer()
    {
        if (inputID.text == "" || inputPassword.text=="")
        {
            return;
        }
        StartCoroutine(Routine_i());
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

    private void Reg_LoginServer()
    {
        string account = inputRegID.text;
        string pwd = inputRegPwd.text;
        string pwdAgain = inputRegPwdAgain.text;
        if (pwd != pwdAgain)
        {
            Debug.LogError("两次输入不一致！");
            return;
        }
        StartCoroutine(Routine_j());
    }
    private IEnumerator Routine_j()
    {
        string l_username = inputRegID.text;
        string l_password = inputRegPwd.text;

        string url = "http://" + loginServer["ip"] + ":" + loginServer["port"] + "/rEgIsTER?USerNAmE=" + l_username + "&PAsSWoRD=" + l_password;

        Debug.LogError(url);
        WWW www = new WWW(url);

        yield return www;
        Debug.LogError(www.text);
        if (www.error == null || www.error == "")
        {
            username = l_username;
            password = l_password;
            ProcessLoginServerData(www.text);

        }
        else
        {
            Debug.LogError(www.error);
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
            this.roles = new List<Roles>();

            int x = roles.Count;
            for(int i = 0; i < x; i++)
            {
                Roles j = new Roles();
                j.level = roles[i]["level"].ToString();
                j.nickname = roles[i]["nickname"].ToString();
                j.serverID = roles[i]["serverID"].ToString();
                j.sex = roles[i]["sex"].ToString();
                j.uid = roles[i]["uid"].ToString();
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
        int x = serverlist.Count;
        int y = serverlistGOs.Count;
        
        for (int i=0;i<Mathf.Max(x,y);i++)
        {
            if (i > x-1){
               serverlistGOs[i].SetActive(false) ;
               
            }
            else 
            {
                if (i > y - 1)
                {
                    GameObject j = Instantiate(togExampleServer, toggleGroupRegister.transform);

                    serverlistGOs.Insert(i, j);
                }
              
              
                InitServerItem(serverlistGOs[i], serverlist[i]);
            }

           



        }
        if (serverlistGOs.Count > 0)
        {
            serverlistGOs[0].GetComponent<Toggle>().isOn=true;
        }

    }

    private void InitServerItem(GameObject argA,Serverlist argB)
    {
        argA.SetActive(true);
        argA.name = argB.server;
        Text serverNum = argA.transform.Find("imgNumBox/imgMsgBox/labServerNum").GetComponent<Text>();
        Text serverName = argA.transform.Find("imgNumBox/labServerName").GetComponent<Text>();
        Image serverState = argA.transform.Find("imgNumBox/imgServerState").GetComponent<Image>();
        serverName.text = argB.name;
        serverNum.text = argB.server+"区";
        serverState.enabled = false;
        Toggle l_toggle = argA.GetComponent<Toggle>();
        l_toggle.onValueChanged.RemoveAllListeners();
        int index = argA.name.ToInt32();

        l_toggle.onValueChanged.AddListener((bool isON) =>
        {
            if (isON)
            {
                selectedIndex = index-1;
                Debug.LogError("选择了=> "+index.ToString());
                labBtmServer.text = index.ToString() + "区  " +argB.name;
            }

        });



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
       NetworkModule.Instance.Connect(serverlist[selectedIndex].ip, serverlist[selectedIndex].port.ToInt32());
    }
}




