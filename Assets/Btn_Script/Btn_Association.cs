using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn_Association : MonoBehaviour {

    public void Call()
    {
        GameObject.Find("Association_Call").GetComponent<AssociationControl>().Show();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
