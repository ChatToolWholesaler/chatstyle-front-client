using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn_Close : MonoBehaviour {

    public void Close()
    {
        GetComponentInParent<CanvasGroup>().alpha = 0;
        GetComponentInParent<CanvasGroup>().interactable = false;
        GetComponentInParent<CanvasGroup>().blocksRaycasts = false;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
