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
            GameObject.Find("Information UI").GetComponent<InformationControl>().Show(nickname, id, sex, sign);
        }
        else if (GetComponentInParent<BlackItemControl>())
        {
            nickname = GetComponentInParent<BlackItemControl>().nickname;
            id = GetComponentInParent<BlackItemControl>().id;
            sex = GetComponentInParent<BlackItemControl>().sex;
            sign = GetComponentInParent<BlackItemControl>().sign;
            GameObject.Find("Information UI").GetComponent<InformationControl>().Show(nickname, id, sex, sign);
        }
        else if (GetComponentInParent<RequestItemControl>())
        {
            nickname = GetComponentInParent<RequestItemControl>().nickname;
            id = GetComponentInParent<RequestItemControl>().id;
            sex = GetComponentInParent<RequestItemControl>().sex;
            sign = GetComponentInParent<RequestItemControl>().sign;
            GameObject.Find("Information UI").GetComponent<InformationControl>().Show(nickname, id, sex, sign);
        }
        else if (GetComponentInParent<InteractionControl>() != null)
        {
            nickname = GetComponentInParent<InteractionControl>().nickname;
            id = GetComponentInParent<InteractionControl>().id;
            sex = "♂";//从数据库查询！！！
            sign = "个性签名：千里之行，始于足下。";//从数据库查询！！！
            Debug.Log("查看的id：" + int.Parse(id));
            WWWForm form1 = new WWWForm();
            form1.AddField("user_id", int.Parse(id));
            StartCoroutine(GameObject.Find("StateObject").GetComponent<StateObject>().get_info("http://localhost:3000/api/v1/user/getUserInfo", form1));
        }
        
    }

    void start()
    {
        
    }
}
