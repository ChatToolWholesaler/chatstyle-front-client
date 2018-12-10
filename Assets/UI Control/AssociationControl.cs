using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssociationControl : MonoBehaviour {

    public void Show()
    {
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().interactable = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void Hide()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public void setupinfo(bool gender, string nickname, string sign)
    {
        GameObject.Find("Runtime UI/Association_Call/ListBg/Information/sex/Text").GetComponent<Text>().text = (gender ? "♂" : "♀");
        GameObject.Find("Runtime UI/Association_Call/ListBg/Information/name/nickname").GetComponent<Text>().text = nickname;
        GameObject.Find("Runtime UI/Association_Call/ListBg/Information/sign/sign").GetComponent<InputField>().text = sign;
    }
    public void Trigger()
    {
        if (GetComponent<CanvasGroup>().interactable)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    // Use this for initialization
    void Start()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("e"))
        {
            Trigger();
        }
    }
}
