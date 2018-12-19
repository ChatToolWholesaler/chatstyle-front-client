using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionControl : MonoBehaviour {

    public string nickname;
    public string id;
    private GameObject friendlist;
    private GameObject blacklist;
    private GameObject addfriend;
    private GameObject addblack;

    public void Show(string nickname, string id, Vector3 pos)
    {
        this.nickname = nickname;
        this.id = id;
        GetComponent<RectTransform>().position = pos;
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().interactable = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        foreach (Transform item in friendlist.transform)
        {
            if (item.GetComponent<FriendItemControl>().GetID() == id)
            {
                addfriend.GetComponent<Button>().interactable = false;
                addfriend.GetComponentInChildren<Text>().text = "已是好友";
            }
        }
        foreach (Transform item in blacklist.transform)
        {
            if (item.GetComponent<BlackItemControl>().GetID() == id)
            {
                addblack.GetComponent<Button>().interactable = false;
                addblack.GetComponentInChildren<Text>().text = "已拉黑";
            }
        }
        //注意检索好友列表以及黑名单是否有这个人
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
        friendlist = GameObject.Find("FriendList");
        blacklist = GameObject.Find("BlackList");
        addfriend = GameObject.Find("InteractionAddFriend");
        addblack = GameObject.Find("InteractionAddBlack");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
