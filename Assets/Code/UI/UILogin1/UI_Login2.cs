using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Login2 : MonoBehaviour {
    private Button btnRegister, btinLogin, btnForgetPwd, btnSignIn, btnSignUP;
    private Canvas objRegister, objLogin, objServers;

    // Use this for initialization
    void Start () {
        btnRegister = transform.Find("imgBackground/objLogin/imgGroupRegister/btnRegister").GetComponent<Button>();
        btinLogin = transform.Find("imgBackground/objLogin/imgGroupLogin/btnLogin").GetComponent<Button>();
        btnForgetPwd = transform.Find("imgBackground/objLogin/imgGroupLogin/btnForgetPwd").GetComponent<Button>();
        btnSignIn = transform.Find("imgBackground/objRegister/imgGroupRegister/btnSignIn").GetComponent<Button>();
        btnSignUP = transform.Find("imgBackground/objServes/imgGroupHaveLogined/btnSignUp").GetComponent<Button>();
        objRegister = transform.Find("imgBackground/objRegister").GetComponent<Canvas>();
        objLogin = transform.Find("imgBackground/objLogin").GetComponent<Canvas>();
        objServers = transform.Find("imgBackground/objServers").GetComponent<Canvas>();
        btnRegister.onClick.RemoveAllListeners();
        btnRegister.onClick.AddListener(ToRegisterStep);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void ToRegisterStep()
    {
        objLogin.enabled = false;
        objRegister.enabled = true;
        objServers.enabled = false;


    }
}
