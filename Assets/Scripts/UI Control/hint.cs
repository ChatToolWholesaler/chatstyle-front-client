using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hint : MonoBehaviour {
    
    private bool is_appear =false;

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

    void Start()
    {
        Hide();
    }

    public void Hint(string message)
    {
        GetComponentInChildren<Text>().text = message;
        Show();
        is_appear = true;
    }

    void Update()
    {
        if (is_appear) {
            GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
            if (GetComponent<CanvasGroup>().alpha <= 0) { Hide(); is_appear = false; }
        }
    }
}
