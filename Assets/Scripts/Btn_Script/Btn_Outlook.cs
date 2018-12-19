using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Btn_Outlook : MonoBehaviour {

    public void GoToAvatarOutlookModuleOnClick()
    {
        GameManagement GM = GameObject.Find("GameManagement").GetComponent<GameManagement>();
        Transform target = GameObject.Find("role").transform;
        GameObject con_obj = GameObject.Find("StateObject");
        StateObject.instance.id = GM.id;
        StateObject.instance.nickname = GM.nickname;
        StateObject.instance.sign = GM.sign;
        StateObject.instance.gender = GM.gender;
        StateObject.instance.roomno = GM.RoomNo;
        StateObject.instance.roomname = GameObject.Find("RoomNo").GetComponent<Text>().text;
        StateObject.instance.position = target.position;
        StateObject.instance.forward = target.forward;
        StateObject.instance.velocity = target.GetComponent<Rigidbody>().velocity;
        StartCoroutine(EnterPresetsGallery());
        print("click: move to the avatar outlook edit module");
    }
    IEnumerator EnterPresetsGallery() { AsyncOperation op = SceneManager.LoadSceneAsync("PresetsGallery"); yield return new WaitForEndOfFrame(); op.allowSceneActivation = true; }
}
