using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPlayerControl : MonoBehaviour {

    public void SetMapPlayer(bool i)
    {
        if (i)
        {
            GameObject.Find("MapCamera (1)").GetComponent<Camera>().depth = -2;
        }
        else
        {
            GameObject.Find("MapCamera (1)").GetComponent<Camera>().depth = -4;
        }
    }
}
