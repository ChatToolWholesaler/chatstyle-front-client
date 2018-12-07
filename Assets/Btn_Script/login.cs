﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class login : MonoBehaviour {

    private void submit()
    {
        string username = GameObject.Find("Login UI/username").GetComponent<InputField>().text;
        string password = GameObject.Find("Login UI/password").GetComponent<InputField>().text;
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        StartCoroutine(GameObject.Find("StateObject").GetComponent<StateObject>().login("http://localhost:3000/api/v1/user/login", form));
        //GameObject.Find("GameManagement").GetComponent<GameManagement>().LoginOK("123", "卿");
        //向后台发送数据
    }
}
