using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterRoom : MonoBehaviour {

    // Use this for initialization
    public void enterroom(int number)
    {
        GameObject.Find("GameManagement").GetComponent<GameManagement>().EnterRoom(number,GetComponentInChildren<Text>().text);
    }
}
