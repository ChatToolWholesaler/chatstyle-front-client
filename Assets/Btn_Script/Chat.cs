using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour {

    public void chat()
    {
        if (!GameObject.Find("Runtime UI/Chat UI/ChatItem").GetComponent<CanvasGroup>().interactable)
        {
            GameObject.Find("Chat UI").GetComponent<ChatControl>().Show();
        }
        
    }
}
