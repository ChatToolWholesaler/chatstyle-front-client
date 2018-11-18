using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn_Quitroom : MonoBehaviour {
    public void quitroom()
    {
        //GameObject obj = GameObject.Find("GameManagement");
        //GameObject.Find("StateObject").GetComponent<StateObject>().sendmsg(1, 0, 0, 0, obj.GetComponent<GameManagement>().RoomNo, obj.GetComponent<GameManagement>().id, obj.GetComponent<GameManagement>().nickname, 2, obj.GetComponent<GameManagement>().nickname + "下线了");
        GameObject.Find("StateObject").GetComponent<StateObject>().close_connect();
        //断开连接
        GameObject.Find("BGM").GetComponent<AudioSource>().Stop();
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.name != "role")
            {
                GameObject.Destroy(player);
            }
        }
        GameObject.Find("Information UI").GetComponent<InformationControl>().Hide();
        GameObject.Find("Chat UI").GetComponent<ChatControl>().Hide();
        GameObject.Find("Manu").GetComponent<ManuControl>().Hide();
        GameObject.Find("Main Camera").GetComponent<Camera>().cullingMask = 1 << 5;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameObject.Find("Manu").GetComponent<ManuControl>().DisableMouseMove();
        GameObject.Find("Manu").GetComponent<ManuControl>().DisableManuControl();
        GameObject.Find("Map").GetComponent<MapControl>().Hide();
        GameObject.Find("role").GetComponent<movement>().DisableMoveControl();
        GameObject.Find("Login UI").GetComponent<LoginControl>().Hide();
        GameObject.Find("Selectroom UI").GetComponent<SelectroomControl>().Show();
        //注意取消用户聊天框控制权
    }
}
