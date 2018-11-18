using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn_Settings : MonoBehaviour {
    

    public void Call()
    {
        GameObject.Find("Setting_Call").GetComponent<SettingControl>().Show();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
