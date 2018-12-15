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
        WWWForm form0 = new WWWForm();
        form0.AddField("userId", int.Parse(GameObject.Find("GameManagement").GetComponent<GameManagement>().id));
        form0.AddField("type", 0);
        StartCoroutine(GameObject.Find("StateObject").GetComponent<StateObject>().acquire("http://localhost:3000/api/v1/friend/getFriendsList", form0, 0));
        WWWForm form1 = new WWWForm();
        form1.AddField("userId", int.Parse(GameObject.Find("GameManagement").GetComponent<GameManagement>().id));
        form1.AddField("type", 1);
        StartCoroutine(GameObject.Find("StateObject").GetComponent<StateObject>().acquire("http://localhost:3000/api/v1/friend/getFriendsList", form1, 1));
        WWWForm form2 = new WWWForm();
        form2.AddField("userId", int.Parse(GameObject.Find("GameManagement").GetComponent<GameManagement>().id));
        form2.AddField("type", 2);
        StartCoroutine(GameObject.Find("StateObject").GetComponent<StateObject>().acquire("http://localhost:3000/api/v1/friend/getFriendsList", form2, 2));
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
