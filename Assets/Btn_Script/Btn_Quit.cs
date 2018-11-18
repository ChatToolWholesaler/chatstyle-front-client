using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn_Quit : MonoBehaviour {
    public void Quit()
    {
        GameObject.Find("StateObject").GetComponent<StateObject>().close_connect();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

}
