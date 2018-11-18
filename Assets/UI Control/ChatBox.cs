using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBox : MonoBehaviour {

    private GameObject popsetting;
    private float time;
    private Vector3 curscale;

    public void pop(string data)
    {
        if (popsetting.GetComponent<PopControl>().popable)
        {
            if (data.Length > 14) data = data.Insert(14, "\n");
            GetComponentInChildren<Text>().text = data;
            GetComponentInChildren<CanvasGroup>().alpha = 1;
            GetComponentInChildren<CanvasGroup>().interactable = true;
            GetComponentInChildren<CanvasGroup>().blocksRaycasts = true;
            time = 0f;
        }
    }

    // Use this for initialization
    void Start () {
        popsetting = GameObject.Find("show_bubble");
        GetComponentInChildren<CanvasGroup>().alpha = 0;
        GetComponentInChildren<CanvasGroup>().interactable = false;
        GetComponentInChildren<CanvasGroup>().blocksRaycasts = false;
        time = 0f;
        curscale = transform.localScale;
    }
	
	// Update is called once per frame
	void Update () {
        if (GetComponentInChildren<CanvasGroup>().interactable)
        {
            time += Time.deltaTime;
            //transform.LookAt(GameObject.Find("Main Camera").transform.position);
            transform.rotation = GameObject.Find("Main Camera").transform.rotation;
            transform.localScale = curscale * ((transform.position - GameObject.Find("Main Camera").transform.position).magnitude / 16);
        }
        if (time > 5f)
        {
            time = 0f;
            GetComponentInChildren<CanvasGroup>().alpha = 0;
            GetComponentInChildren<CanvasGroup>().interactable = false;
            GetComponentInChildren<CanvasGroup>().blocksRaycasts = false;
        }
    }
}
