using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;



public class register : MonoBehaviour
{

    public void submit()

    {

        string username = GameObject.Find("Register UI/username_r").GetComponent<InputField>().text;

        string password = GameObject.Find("Register UI/password_r").GetComponent<InputField>().text;

        string nickname = GameObject.Find("Register UI/nickname").GetComponent<InputField>().text;

        bool gender = (GameObject.Find("Register UI/gender").GetComponent<Dropdown>().value == 0 ? true : false);

        //向后台发送数据

        if (!IsNumeric(username))

        {
            GameObject.Find("ErrorText_r").GetComponent<Text>().text = "用户名必须是数字！";
            return;

        }
        if (nickname=="role")
        {
            GameObject.Find("ErrorText_r").GetComponent<Text>().text = "非法昵称！";
            return;
        }
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("nickname", nickname);
        form.AddField("gender", (gender ? 1 : 0));
        StartCoroutine(GameObject.Find("StateObject").GetComponent<StateObject>().register("http://localhost:3000/api/v1/user/register", form));

    }
    public bool IsNumeric(string str)
    {
        if (str == null || str.Length == 0)
            return false;
        ASCIIEncoding ascii = new ASCIIEncoding();
        byte[] bytestr = ascii.GetBytes(str);
        foreach (byte c in bytestr)
        {
            if (c < 48 || c > 57)
            {
                return false;
            }
        }
        return true;
    }
}