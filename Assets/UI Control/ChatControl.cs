using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatControl : MonoBehaviour {

    [HideInInspector] public bool m_ChatControl;
    [HideInInspector] public Transform m_Targets;

    private GameObject chattext;
    private GameObject chatitem;

    public void Show()
    {
        DisableMouseMove();
        EnableChatControl();
        GameObject.Find("role").GetComponent<movement>().DisableMoveControl();
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().interactable = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        chattext.GetComponent<CanvasGroup>().alpha = 1;
        chattext.GetComponent<CanvasGroup>().interactable = true;
        chattext.GetComponent<CanvasGroup>().blocksRaycasts = true;
        chatitem.GetComponent<CanvasGroup>().alpha = 1;
        chatitem.GetComponent<CanvasGroup>().interactable = true;
        chatitem.GetComponent<CanvasGroup>().blocksRaycasts = true;
        GameObject.Find("ChatInput").GetComponent<InputField>().ActivateInputField();
        //GameObject.Find("Dropdown").GetComponent<DropdownControl>().change(GameObject.Find("Dropdown").GetComponent<Dropdown>().value);
    }

    public void Hide()
    {
        DisableMouseMove();
        DisableChatControl();
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        chattext.GetComponent<CanvasGroup>().alpha = 0;
        chattext.GetComponent<CanvasGroup>().interactable = false;
        chattext.GetComponent<CanvasGroup>().blocksRaycasts = false;
        chatitem.GetComponent<CanvasGroup>().alpha = 0;
        chatitem.GetComponent<CanvasGroup>().interactable = false;
        chatitem.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void HalfShow()
    {
        EnableMouseMove();
        EnableChatControl();
        GameObject.Find("role").GetComponent<movement>().EnableMoveControl();
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().interactable = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        chattext.GetComponent<CanvasGroup>().alpha = 1;
        chattext.GetComponent<CanvasGroup>().interactable = true;
        chattext.GetComponent<CanvasGroup>().blocksRaycasts = true;
        chatitem.GetComponent<CanvasGroup>().alpha = 0;
        chatitem.GetComponent<CanvasGroup>().interactable = false;
        chatitem.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void DisableMouseMove()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        GameObject gameObject = GameObject.Find("Main Camera");
        gameObject.GetComponent<CameraControl>().DisableAngleControl();
        GameObject.Find("role").GetComponent<movement>().DisableTurnControl();
    }

    public void EnableMouseMove()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible =false;
        GameObject gameObject = GameObject.Find("Main Camera");
        gameObject.GetComponent<CameraControl>().EnableAngleControl();
        GameObject.Find("role").GetComponent<movement>().EnableTurnControl();
    }

    public void EnableChatControl()
    {
        m_ChatControl = true;
    }
    public void DisableChatControl()
    {
        m_ChatControl = false;
    }
    public void Trigger()
    {
        if (chatitem.GetComponent<CanvasGroup>().interactable) {
            HalfShow();
            string text = GetComponentInChildren<InputField>().text;
            if (text == "") return;
            int channel = 1 + GetComponentInChildren<Dropdown>().value;
            GameObject.Find("StateObject").GetComponent<StateObject>().sendmsg(3, m_Targets.position.x, m_Targets.position.y, m_Targets.position.z, GameObject.Find("GameManagement").GetComponent<GameManagement>().RoomNo, GameObject.Find("GameManagement").GetComponent<GameManagement>().id, GameObject.Find("GameManagement").GetComponent<GameManagement>().nickname, channel, text);
            GetComponentInChildren<InputField>().text = "";
            //根据选择的渠道发送消息，空值则无反应，暂时忽略附近(等九屏)，悄悄话(需要等好友列表，id搜寻以及socket转发)
        }
        else { Show(); }
    }
    public void AddContent(int type, float position_x, float position_y, float position_z, int roomno, string id, string nickname, int channel, string content)
    {
        switch (type)
        {
            case 1://记得提高信息查询效率，不查不必要的信息，上下线消息可不添加判断（后台已有判断）
                GameObject.Find("Channel2").GetComponent<Text>().text += ("\n" + content);
                GameObject.Find("Channel0").GetComponent<Text>().text += ("\n" + content);
                break;
            case 2:
                GameObject.Find("Channel2").GetComponent<Text>().text += ("\n" + content);
                GameObject.Find("Channel0").GetComponent<Text>().text += ("\n" + content);
                break;
            case 3://玩家消息需要添加判断是否接受
                switch (channel)
                {
                    case 1://世界
                        GameObject.Find("Channel1").GetComponent<Text>().text += ("\n" + "[世界]" + nickname + "：" + content);
                        GameObject.Find("Channel0").GetComponent<Text>().text += ("\n" + "[世界]" + nickname + "：" + content);
                        //Debug.Log("收到世界消息");
                        break;
                    case 2://房间
                        if (GameObject.Find("GameManagement").GetComponent<GameManagement>().RoomNo == roomno)
                        {
                            GameObject.Find("Channel2").GetComponent<Text>().text += ("\n" + "[房间]" + nickname + "：" + content);
                            GameObject.Find("Channel0").GetComponent<Text>().text += ("\n" + "[房间]" + nickname + "：" + content);
                        }
                        break;
                    case 3://附近，后台已判断是否在范围内，有时间可以前台再加一重判断保险
                        GameObject.Find("Channel3").GetComponent<Text>().text += ("\n" + "[附近]" + nickname + "：" + content);
                        GameObject.Find("Channel0").GetComponent<Text>().text += ("\n" + "[附近]" + nickname + "：" + content);
                        if (id == GameObject.Find("GameManagement").GetComponent<GameManagement>().id)
                        {
                            GameObject.Find("role").GetComponentInChildren<ChatBox>().pop(content);
                        }
                        else
                        {
                            GameObject.Find(id).GetComponentInChildren<ChatBox>().pop(content);//玩家头顶浮现气泡，记得附近局部加载玩家
                        }
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    // Use this for initialization
    void Start () {
        m_Targets = GameObject.Find("role").transform;
        chattext = GameObject.Find("ChatText");
        chatitem = GameObject.Find("ChatItem");
        Hide();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("enter") && m_ChatControl) Trigger();
    }
}
