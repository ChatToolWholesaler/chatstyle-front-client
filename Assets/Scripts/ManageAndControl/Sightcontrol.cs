using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sightcontrol : MonoBehaviour {
    //0号元素为普通准星
    //1号元素为发现玩家准星
    public Texture2D[] sightTexture;
    public Sprite[] sights;
    // Use this for initialization

    void Awake()
    {
        sights = new Sprite[sightTexture.Length];
        for (int i = 0; i < sightTexture.Length; i++)
        {
            sights[i] = Sprite.Create(sightTexture[i], new Rect(0, 0, sightTexture[i].width, sightTexture[i].height), new Vector2(0.5f, 0.5f));
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
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

    public void SetSight(int sight)
    {
        GetComponentInChildren<Image>().sprite = sights[sight];
        switch (sight)
        {
            case 0:
                GetComponentInChildren<Image>().color = new Color(0f, 0f, 0f, 130f / 255f);
                break;
            case 1:
                GetComponentInChildren<Image>().color = new Color(0f, 1f, 0f, 130f / 255f);
                break;
            default:
                break;
        }
    }
}
