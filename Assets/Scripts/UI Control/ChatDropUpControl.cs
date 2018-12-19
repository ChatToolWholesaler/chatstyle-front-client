using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatDropUpControl : MonoBehaviour {
    public GameObject toIDInputField;
    public GameObject chatinput;
    public void OnChange(int cur)
    {
        if (cur == 3)
        {
            if (!toIDInputField.GetComponent<InputIDControl>().is_call)
            {
                GameObject.Find("InputToId UI").GetComponent<InputIDControl>().Show();
            }
        }
    }
    public void ShowPrefix()
    {
        chatinput.GetComponent<InputField>().text = "To>>> " + toIDInputField.GetComponent<InputField>().text + ":";
        GameObject.Find("ChatInput").GetComponent<InputField>().ActivateInputField();
    }
    void Start()
    {
        toIDInputField = GameObject.Find("ChatID");
        chatinput = GameObject.Find("ChatInput");
    }
}
