using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Btn_Outlook : MonoBehaviour {

    public void GoToAvatarOutlookModuleOnClick()
    {
        StartCoroutine(EnterPresetsGallery());
        print("click: move to the avatar outlook edit module");
    }
    IEnumerator EnterPresetsGallery() { AsyncOperation op = SceneManager.LoadSceneAsync("PresetsGallery"); yield return new WaitForEndOfFrame(); op.allowSceneActivation = true; }
}
