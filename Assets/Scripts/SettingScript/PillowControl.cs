using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillowControl : MonoBehaviour {


    public void SetPillow(bool pillow)
    {
        GameObject.Find("Chat UI").GetComponent<ChatControl>().accept_pillow = pillow;
    }
}
