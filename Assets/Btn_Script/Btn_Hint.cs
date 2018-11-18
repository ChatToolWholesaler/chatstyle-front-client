using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn_Hint : MonoBehaviour {



    public void Hide()
    {
        GetComponentInParent<CanvasGroup>().alpha = 0;
        GetComponentInParent<CanvasGroup>().interactable = false;
        GetComponentInParent<CanvasGroup>().blocksRaycasts = false;
    }
}
