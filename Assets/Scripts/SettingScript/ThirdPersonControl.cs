using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonControl : MonoBehaviour {
    GameObject maincamera;

    public void setThird(bool is_third)
    {
        maincamera.GetComponent<CameraControl>().is_Third = is_third;
    }

    void Awake()
    {
        maincamera = GameObject.Find("Main Camera");
    }
}
