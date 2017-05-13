using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsharpTest : MonoBehaviour {
    AudioSource audio;
	// Use this for initialization
	void Start () {
        audio.Play();
	}
	
	// Update is called once per frame
	void Update () {
        //Resources.LoadAssetAtPath
        
    }

    private void OnGUI()
    {
        GUILayout.Label("AAAAAAAAAAA");
        
    }
}
