﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelBlack : MonoBehaviour {

    public void removeblack()
    {
        WWWForm form1 = new WWWForm();
        form1.AddField("initiativeAddId", int.Parse(GameObject.Find("GameManagement").GetComponent<GameManagement>().id));
        form1.AddField("passiveAddId", int.Parse(GetComponentInParent<BlackItemControl>().id));
        form1.AddField("type", 1);
        StartCoroutine(GameObject.Find("StateObject").GetComponent<StateObject>().apply_pull("http://localhost:3000/api/v1/friend/deleteFriend", form1));
    }
}
