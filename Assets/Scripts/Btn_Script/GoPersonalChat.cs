using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoPersonalChat : MonoBehaviour {

    public void gopersonalchat()
    {
        if (GetComponentInParent<InteractionControl>())
        {
            GameObject.Find("Chat UI").GetComponent<ChatControl>().Show();
            GameObject.Find("ChatID").GetComponent<InputIDControl>().SetID(GetComponentInParent<InteractionControl>().id, true);
            GameObject.Find("Chat UI").GetComponentInChildren<Dropdown>().value = 3;
        }
        else if (GetComponentInParent<FriendItemControl>())
        {
            GameObject.Find("Chat UI").GetComponent<ChatControl>().Show();
            GameObject.Find("ChatID").GetComponent<InputIDControl>().SetID(GetComponentInParent<FriendItemControl>().id, true);
            GameObject.Find("Chat UI").GetComponentInChildren<Dropdown>().value = 3;
        }
    }
}
