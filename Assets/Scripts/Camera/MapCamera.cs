using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour {

    [HideInInspector] public Transform m_Targets;

    private void Awake()
    {
        m_Targets = GameObject.Find("role").transform;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(m_Targets.position.x, transform.position.y, m_Targets.position.z);

    }
}
