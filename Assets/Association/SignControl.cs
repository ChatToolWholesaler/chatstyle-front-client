using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignControl : MonoBehaviour {

    public Sprite hide;
    public Sprite show;
    private GameObject stateobject;
    InputField m_InputField;
    void Start()
    {
        //Fetch the Input Field component from the GameObject
        m_InputField = GetComponent<InputField>();
        stateobject = GameObject.Find("StateObject");
    }

    void Update()
    {
        //Check if the Input Field is in focus and able to alter
        if (m_InputField.isFocused)
        {
            //Change the Color of the Input Field's Image to green
            GetComponent<Image>().sprite = show;
        }
    }

    public void submit(string sign)
    {
        //提交签名的code
        WWWForm form1 = new WWWForm();
        form1.AddField("userId", int.Parse(GameObject.Find("GameManagement").GetComponent<GameManagement>().id));
        form1.AddField("sign", sign);
        StartCoroutine(GameObject.Find("StateObject").GetComponent<StateObject>().set_sign("http://localhost:3000/api/v1/user/setSign", form1));
        GetComponent<Image>().sprite = hide;
    }

}
