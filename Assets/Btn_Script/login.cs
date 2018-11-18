using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class login : MonoBehaviour {

    private void submit()
    {
        string username = GameObject.Find("username").GetComponent<InputField>().text;
        string password = GameObject.Find("password").GetComponent<InputField>().text;
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        StartCoroutine(GameObject.Find("StateObject").GetComponent<StateObject>().login("http://yangyuqing.vipgz1.idcfengye.com/chat_room/login.php", form));
        //GameObject.Find("GameManagement").GetComponent<GameManagement>().LoginOK("123", "卿");
        //向后台发送数据
    }
}
