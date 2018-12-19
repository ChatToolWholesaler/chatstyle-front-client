using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteFriend : MonoBehaviour {

    public void removefriend()
    {
        WWWForm form1 = new WWWForm();
        form1.AddField("initiativeAddId", int.Parse(GameObject.Find("GameManagement").GetComponent<GameManagement>().id));
        form1.AddField("passiveAddId", int.Parse(GetComponentInParent<InformationControl>().id));
        form1.AddField("type", 0);
        StartCoroutine(GameObject.Find("StateObject").GetComponent<StateObject>().apply_pull("http://" + GameObject.Find("StateObject").GetComponent<StateObject>().urlip + ":3000/api/v1/friend/deleteFriend", form1));
    }
}
