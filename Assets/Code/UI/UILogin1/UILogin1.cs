using System.Collections;
using System.Collections.Generic;
using KSFramework;
using UnityEngine;
using UnityEngine.UI;


public class UILogin1 : MonoBehaviour {
    private string serverListUrl="http://47.94.220.1/serverlist.html";
    public Button btnLogin;
    public Button btnForgetPwd;
    public Button btnRegister;


    // Use this for initialization
    void Start () {
        btnLogin.onClick.RemoveAllListeners();
        btnLogin.onClick.AddListener(Login_LoginServer);
        btnForgetPwd.onClick.RemoveAllListeners();
        btnForgetPwd.onClick.AddListener(forgetPwd);
        btnRegister.onClick.RemoveAllListeners();
        btnRegister.onClick.AddListener(forgetPwd);

    }

    // Update is called once per frame
    void Update () {
		
	}

    void Login_LoginServer(){




    }
    void forgetPwd() {


    }

    void toRegisterStep (){

    }

}   
