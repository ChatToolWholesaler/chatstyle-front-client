using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chat : MonoBehaviour {

    public void chat()
    {
        GameObject.Find("Chat UI").GetComponent<ChatControl>().Show();
    }
}
