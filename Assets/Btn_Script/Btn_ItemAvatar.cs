using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn_ItemAvatar : MonoBehaviour {

    private string nickname;
    private string id;
    private string sex;
    private string sign;

    public void Call()
    {
        if (GetComponentInParent<FriendItemControl>())
        {
            nickname = GetComponentInParent<FriendItemControl>().nickname;
            id = GetComponentInParent<FriendItemControl>().id;
            sex = GetComponentInParent<FriendItemControl>().sex;
            sign = GetComponentInParent<FriendItemControl>().sign;
        }
        else if (GetComponentInParent<BlackItemControl>())
        {
            nickname = GetComponentInParent<BlackItemControl>().nickname;
            id = GetComponentInParent<BlackItemControl>().id;
            sex = GetComponentInParent<BlackItemControl>().sex;
            sign = GetComponentInParent<BlackItemControl>().sign;
        }
        else if (GetComponentInParent<RequestItemControl>())
        {
            nickname = GetComponentInParent<RequestItemControl>().nickname;
            id = GetComponentInParent<RequestItemControl>().id;
            sex = GetComponentInParent<RequestItemControl>().sex;
            sign = GetComponentInParent<RequestItemControl>().sign;
        }
        else if (GetComponentInParent<InteractionControl>() != null)
        {
            nickname = GetComponentInParent<RequestItemControl>().nickname;
            id = GetComponentInParent<RequestItemControl>().id;
            sex = "♂";//从数据库查询！！！
            sign = "个性签名：千里之行，始于足下。";//从数据库查询！！！
        }
        GameObject.Find("Information UI").GetComponent<InformationControl>().Show(nickname, id, sex, sign);
    }

    void start()
    {
        
    }
}
