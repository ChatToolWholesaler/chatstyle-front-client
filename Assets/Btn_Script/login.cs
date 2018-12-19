using System.Collections;
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
        Debug.Log("http://" + GameObject.Find("StateObject").GetComponent<StateObject>().urlip + ":3000/api/v1/user/login");
        StartCoroutine(GameObject.Find("StateObject").GetComponent<StateObject>().login("http://" + GameObject.Find("StateObject").GetComponent<StateObject>().urlip + ":3000/api/v1/user/login", form));
        //GameObject.Find("GameManagement").GetComponent<GameManagement>().LoginOK("123", "卿","个性签名",true);
        //向后台发送数据
    }
}
