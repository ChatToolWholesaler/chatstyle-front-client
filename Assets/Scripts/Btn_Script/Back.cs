using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back : MonoBehaviour {

    public void back()
    {
        GameObject.Find("Login UI").GetComponent<LoginControl>().Show();
        GameObject.Find("Selectroom UI").GetComponent<SelectroomControl>().Hide();
    }
}
