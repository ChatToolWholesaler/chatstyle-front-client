using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManuControl : MonoBehaviour {

    private string Call;
    [HideInInspector] public bool m_ManuControl;

    public void EnableManuControl()
    {
        m_ManuControl = true;
    }
    public void DisableManuControl()
    {
        m_ManuControl = false;
    }

    public void DisableMouseMove()
    {
        GameObject gameObject = GameObject.Find("Main Camera");
        gameObject.GetComponent<CameraControl>().DisableAngleControl();
        GameObject.Find("role").GetComponent<movement>().DisableTurnControl();
    }

    public void EnableMouseMove()
    {
        GameObject gameObject = GameObject.Find("Main Camera");
        gameObject.GetComponent<CameraControl>().EnableAngleControl();
        GameObject.Find("role").GetComponent<movement>().EnableTurnControl();
    }

    public void Show()
    {
        DisableMouseMove();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().interactable = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void Hide()
    {
        EnableMouseMove();
        GameObject.Find("Setting_Call").GetComponent<SettingControl>().Hide();
        GameObject.Find("Association_Call").GetComponent<AssociationControl>().Hide();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    private void Trigger()
    {
        if (GetComponent<CanvasGroup>().interactable) Hide();
        else { Show(); }
    }

    // Use this for initialization
    void Start ()
    {
        DisableManuControl();
        Hide();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        DisableMouseMove();
        Call = "Call";
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown(Call) && m_ManuControl) Trigger();
	}
}
