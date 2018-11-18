using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopControl : MonoBehaviour {
    public bool popable;
    void Start()
    {
        popable = true;
    }
    public void SetPopable(bool popable)
    {
        this.popable = popable;
    }
}
