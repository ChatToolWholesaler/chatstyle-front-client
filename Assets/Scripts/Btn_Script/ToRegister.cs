using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToRegister : MonoBehaviour {

    public void toregister()
    {
        GameObject.Find("Login UI").GetComponent<LoginControl>().Hide();
        GameObject.Find("Register UI").GetComponent<LoginControl>().Show();
    }
}
