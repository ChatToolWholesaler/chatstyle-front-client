using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToLogin : MonoBehaviour {

    public void tologin()
    {
        GameObject.Find("Login UI").GetComponent<LoginControl>().Show();
        GameObject.Find("Register UI").GetComponent<LoginControl>().Hide();
    }
}
