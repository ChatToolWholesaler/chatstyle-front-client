using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputIDControl : MonoBehaviour {

    public string id;//目前玩家输入的id
    public bool is_call;//判断是否为外部私聊入口

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

    public void EditSetID(string id)
    {
        SetID(id, false);
    }

    public void SetID(string id , bool is_call)
    {
        this.id = id;
        GetComponent<InputField>().text = id;
        this.is_call = is_call;
        GameObject.Find("Chat UI").GetComponentInChildren<ChatDropUpControl>().ShowPrefix();
    }
    // Use this for initialization
    void Start()
    {
        if (GetComponent<CanvasGroup>())
        {
            Hide();
        }
       
    }

    // Update is called once per frame
    void Update()
    {

    }
}
