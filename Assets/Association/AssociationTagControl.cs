using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssociationTagControl : MonoBehaviour {

    GameObject friendtag;
    GameObject blacktag;
    GameObject requesttag;
    GameObject friend;
    GameObject black;
    GameObject request;

    void Start()
    {
        friendtag = GameObject.Find("FriendTag");
        blacktag = GameObject.Find("BlackTag");
        requesttag = GameObject.Find("RequestTag");
        friend = GameObject.Find("Friend");
        black = GameObject.Find("Black");
        request = GameObject.Find("Request");
    }
    public void show(GameObject obj)
    {
        obj.GetComponent<CanvasGroup>().alpha = 1;
        obj.GetComponent<CanvasGroup>().interactable = true;
        obj.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    public void hide(GameObject obj)
    {
        obj.GetComponent<CanvasGroup>().alpha = 0;
        obj.GetComponent<CanvasGroup>().interactable = false;
        obj.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void toggle(bool is_on)
    {
        if (is_on)
        {
            GetComponent<Toggle>().interactable = false;
            switch (name)
            {
                case "FriendTag":
                    change(0);
                    blacktag.GetComponent<Toggle>().isOn = false;
                    blacktag.GetComponent<Toggle>().interactable = true;
                    requesttag.GetComponent<Toggle>().isOn = false;
                    requesttag.GetComponent<Toggle>().interactable = true;
                    break;
                case "BlackTag":
                    change(1);
                    friendtag.GetComponent<Toggle>().isOn = false;
                    friendtag.GetComponent<Toggle>().interactable = true;
                    requesttag.GetComponent<Toggle>().isOn = false;
                    requesttag.GetComponent<Toggle>().interactable = true;
                    break;
                case "RequestTag":
                    change(2);
                    friendtag.GetComponent<Toggle>().isOn = false;
                    friendtag.GetComponent<Toggle>().interactable = true;
                    blacktag.GetComponent<Toggle>().isOn = false;
                    blacktag.GetComponent<Toggle>().interactable = true;
                    break;
                default:
                    break;
            }
        }
    }

    public void change(int value)
    {
        switch (value)
        {
            case 0:
                show(friend);
                hide(black);
                hide(request);
                break;
            case 1:
                hide(friend);
                show(black);
                hide(request);
                break;
            case 2:
                hide(friend);
                hide(black);
                show(request);
                break;
            default:
                break;
        }
        //GameObject.Find("ChatInput").GetComponent<InputField>().ActivateInputField();
        /*chatitem.GetComponent<CanvasGroup>().alpha = 0;
        chatitem.GetComponent<CanvasGroup>().interactable = false;
        chatitem.GetComponent<CanvasGroup>().blocksRaycasts = false;*/
    }
}
