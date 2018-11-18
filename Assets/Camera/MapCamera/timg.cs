using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timg : MonoBehaviour {

    private RectTransform recttransform;
    [HideInInspector] public Transform m_Targets;
    // Use this for initialization
    void Start () {
        recttransform = GetComponent<RectTransform>();
        m_Targets = GameObject.Find("role").transform;
    }
	
	// Update is called once per frame
	void Update () {
        float turn = 0f;
        if (m_Targets.forward.x >= 0) turn = Mathf.Acos(m_Targets.forward.z);
        if (m_Targets.forward.x < 0) turn = 2 * Mathf.PI - Mathf.Acos(m_Targets.forward.z);
        turn *= (180f / Mathf.PI);
        Quaternion turnRotation = Quaternion.Euler(0f, 0f, -turn);
        recttransform.rotation = turnRotation;
        //recttransform.LookAt(new Vector3(recttransform.position.x + m_Targets.forward.x, recttransform.position.y - m_Targets.forward.z,0f));
    }
}
