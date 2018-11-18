using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationControl : MonoBehaviour {

    public GameObject informationsex;
    public GameObject informationnickname;
    public GameObject informationsign;
    public GameObject informationid;
    public GameObject deletefriend;
    public GameObject friendlist;
    public string id;

    public void Show(string nickname, string id, string sex, string sign)
    {
        informationsex.GetComponent<Text>().text = sex;
        this.id = id;
        informationnickname.GetComponent<Text>().text = nickname;
        informationsign.GetComponent<Text>().text = sign;
        informationid.GetComponent<Text>().text = "ID: " + id;
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().interactable = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        deletefriend.GetComponent<Button>().interactable = false;
        foreach (Transform item in friendlist.transform)
        {
            if (item.GetComponent<FriendItemControl>().GetID() == id)
            {
                deletefriend.GetComponent<Button>().interactable = true;
            }
            
        }
        //注意检索好友列表是否有这个人
    }

    public void Hide()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    // Use this for initialization
    void Start()
    {
        Hide();
        informationsex = GameObject.Find("InformationSex");
        informationnickname = GameObject.Find("InformationNickname");
        informationsign = GameObject.Find("InformationSign");
        informationid = GameObject.Find("InformationID");
        deletefriend = GameObject.Find("DeleteFriend");
        friendlist = GameObject.Find("FriendList");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
