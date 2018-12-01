using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignControl : MonoBehaviour {

    public Sprite hide;
    public Sprite show;
    InputField m_InputField;
    void Start()
    {
        //Fetch the Input Field component from the GameObject
        m_InputField = GetComponent<InputField>();
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
        GetComponent<Image>().sprite = hide;
    }

}
