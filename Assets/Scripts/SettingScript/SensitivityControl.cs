using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensitivityControl : MonoBehaviour {

    public void SetTurnSpeed(float turnspeed)
    {
        GameObject.Find("Main Camera").GetComponent<CameraControl>().m_AngleSpeed = turnspeed / 57.3f;
        GameObject.Find("role").GetComponent<movement>().m_TurnSpeed = turnspeed;
    }
    public void SetZoomSpeed(float zoomspeed)
    {
        GameObject.Find("Main Camera").GetComponent<CameraControl>().m_ScrollSpeed = zoomspeed;
    }
}
