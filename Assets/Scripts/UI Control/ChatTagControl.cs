using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatTagControl : MonoBehaviour {

    GameObject chattag0;
    GameObject chattag1;
    GameObject chattag2;
    GameObject chattag3;
    GameObject history0;
    GameObject history1;
    GameObject history2;
    GameObject history3;
    GameObject scroll0;
    GameObject scroll1;
    GameObject scroll2;
    GameObject scroll3;

    void Start()
    {
        chattag0 = GameObject.Find("ChatTag0");
        chattag1 = GameObject.Find("ChatTag1");
        chattag2 = GameObject.Find("ChatTag2");
        chattag3 = GameObject.Find("ChatTag3");
        history0 = GameObject.Find("HistoryText0");
        history1 = GameObject.Find("HistoryText1");
        history2 = GameObject.Find("HistoryText2");
        history3 = GameObject.Find("HistoryText3");
        scroll0 = GameObject.Find("Scrollbar0");
        scroll1 = GameObject.Find("Scrollbar1");
        scroll2 = GameObject.Find("Scrollbar2");
        scroll3 = GameObject.Find("Scrollbar3");
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
                case "ChatTag0":
                    change(0);
                    chattag1.GetComponent<Toggle>().isOn = false;
                    chattag1.GetComponent<Toggle>().interactable = true;
                    chattag2.GetComponent<Toggle>().isOn = false;
                    chattag2.GetComponent<Toggle>().interactable = true;
                    chattag3.GetComponent<Toggle>().isOn = false;
                    chattag3.GetComponent<Toggle>().interactable = true;
                    break;
                case "ChatTag1":
                    change(1);
                    chattag0.GetComponent<Toggle>().isOn = false;
                    chattag0.GetComponent<Toggle>().interactable = true;
                    chattag2.GetComponent<Toggle>().isOn = false;
                    chattag2.GetComponent<Toggle>().interactable = true;
                    chattag3.GetComponent<Toggle>().isOn = false;
                    chattag3.GetComponent<Toggle>().interactable = true;
                    break;
                case "ChatTag2":
                    change(2);
                    chattag0.GetComponent<Toggle>().isOn = false;
                    chattag0.GetComponent<Toggle>().interactable = true;
                    chattag1.GetComponent<Toggle>().isOn = false;
                    chattag1.GetComponent<Toggle>().interactable = true;
                    chattag3.GetComponent<Toggle>().isOn = false;
                    chattag3.GetComponent<Toggle>().interactable = true;
                    break;
                case "ChatTag3":
                    change(3);
                    chattag0.GetComponent<Toggle>().isOn = false;
                    chattag0.GetComponent<Toggle>().interactable = true;
                    chattag1.GetComponent<Toggle>().isOn = false;
                    chattag1.GetComponent<Toggle>().interactable = true;
                    chattag2.GetComponent<Toggle>().isOn = false;
                    chattag2.GetComponent<Toggle>().interactable = true;
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
                show(history0);
                show(scroll0);
                hide(history1);
                hide(scroll1);
                hide(history2);
                hide(scroll2);
                hide(history3);
                hide(scroll3);
                break;
            case 1:
                hide(history0);
                hide(scroll0);
                show(history1);
                show(scroll1);
                hide(history2);
                hide(scroll2);
                hide(history3);
                hide(scroll3);
                break;
            case 2:
                hide(history0);
                hide(scroll0);
                hide(history1);
                hide(scroll1);
                show(history2);
                show(scroll2);
                hide(history3);
                hide(scroll3);
                break;
            case 3:
                hide(history0);
                hide(scroll0);
                hide(history1);
                hide(scroll1);
                hide(history2);
                hide(scroll2);
                show(history3);
                show(scroll3);
                break;
            default:
                break;
        }
        GameObject.Find("ChatInput").GetComponent<InputField>().ActivateInputField();
        /*chatitem.GetComponent<CanvasGroup>().alpha = 0;
        chatitem.GetComponent<CanvasGroup>().interactable = false;
        chatitem.GetComponent<CanvasGroup>().blocksRaycasts = false;*/
    }
}
