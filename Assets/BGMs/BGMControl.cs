using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMControl : MonoBehaviour {

    public AudioClip[] BGMs;
    public void LoadBGM(int num)
    {
        if (num < BGMs.Length)
        {
            GetComponent<AudioSource>().clip = BGMs[num];
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
