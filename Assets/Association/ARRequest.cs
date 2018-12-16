using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARRequest : MonoBehaviour {

    public void acceptrequest()
    {
        WWWForm form1 = new WWWForm();
        form1.AddField("userId", int.Parse(GameObject.Find("GameManagement").GetComponent<GameManagement>().id));
        form1.AddField("friendId", int.Parse(GetComponentInParent<RequestItemControl>().id));
        form1.AddField("agree", 1);
        StartCoroutine(GameObject.Find("StateObject").GetComponent<StateObject>().apply_pull("http://localhost:3000/api/v1/friend/acceptFriend", form1));
    }
    public void rejectrequest()
    {
        WWWForm form1 = new WWWForm();
        form1.AddField("userId", int.Parse(GameObject.Find("GameManagement").GetComponent<GameManagement>().id));
        form1.AddField("friendId", int.Parse(GetComponentInParent<RequestItemControl>().id));
        form1.AddField("agree", 0);
        StartCoroutine(GameObject.Find("StateObject").GetComponent<StateObject>().apply_pull("http://localhost:3000/api/v1/friend/acceptFriend", form1));
    }
}
