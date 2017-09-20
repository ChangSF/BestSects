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
        btinLogin = transform.Find("btinLogin").GetComponent<Button>();
        btnForgetPwd = transform.Find("btnForgetPwd").GetComponent<Button>();
        btnSignIn = transform.Find("btnSignIn").GetComponent<Button>();
        btnSignUP = transform.Find("btnSignUP").GetComponent<Button>();
        objRegister = transform.Find("objRegister").GetComponent<Canvas>();
        objLogin = transform.Find("objLogin").GetComponent<Canvas>();
        objServers = transform.Find("objServers").GetComponent<Canvas>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
