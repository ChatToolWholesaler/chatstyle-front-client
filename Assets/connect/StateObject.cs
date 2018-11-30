using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System;
using Net;



public class StateObject : MonoBehaviour {

    public string ip = "vipgz1.idcfengye.com";
    public int pos_port = 10134;
    public int msg_port = 10135;
    public ClientSocket pos_Socket;
    public ClientSocket msg_Socket;
    public GameObject prefabs = null;
    public Hashtable Players;//存储当前附近的玩家

    void Awake()
    {
        pos_Socket = new ClientSocket();
        msg_Socket = new ClientSocket();
        Players = new Hashtable();
    }

    public IEnumerator login(string _url, WWWForm _wForm)
    {
        WWW postData = new WWW(_url, _wForm);
        yield return postData;
        if (postData.error != null)
        {
            Debug.Log(postData.error);
        }
        else
        {
            Debug.Log(postData.text);
            LoginModel obj = JsonUtility.FromJson<LoginModel> (postData.text);
            if (obj.result == 0)
            {
                GameObject.Find("HintMessage").GetComponent<hint>().Hint("登陆成功！");
                GameObject.Find("GameManagement").GetComponent<GameManagement>().LoginOK(obj.username, obj.nickname);
            }
            else if (obj.result == 3)
            {
                GameObject.Find("ErrorText").GetComponent<Text>().text = "该账号已登录";
            }
            else if (obj.result == 1)
            {
                GameObject.Find("ErrorText").GetComponent<Text>().text = "账号不存在或者密码不匹配";
            }
            else {
                Debug.Log(obj.username);
                Debug.Log(obj.nickname);
                GameObject.Find("ErrorText").GetComponent<Text>().text = "登陆错误";
            }
        }
    }
    public IEnumerator register(string _url, WWWForm _wForm)
    {
        WWW postData = new WWW(_url, _wForm);
        yield return postData;
        if (postData.error != null)
        {
            Debug.Log(postData.error);
        }
        else
        {
            RegisterModel obj = JsonUtility.FromJson<RegisterModel>(postData.text);
            if (obj.result == 0)
            {
                GameObject.Find("HintMessage").GetComponent<hint>().Hint("注册成功！");
                GameObject.Find("Register UI").GetComponent<LoginControl>().Hide();
                GameObject.Find("Login UI").GetComponent<LoginControl>().Show();
            }
            else
            {
                if (obj.result == 1)
                {
                    GameObject.Find("ErrorText_r").GetComponent<Text>().text = "账号或者密码已存在";
                }
                else if (obj.result == 3)
                {
                    GameObject.Find("ErrorText_r").GetComponent<Text>().text = "昵称已存在";
                }
                else {
                    Debug.Log(obj);
                }
            }

            Debug.Log(postData.text);
        }
    }

    public IEnumerator go_online(string _url, WWWForm _wForm)
    {
        WWW postData = new WWW(_url, _wForm);
        yield return postData;
        if (postData.error != null)
        {
            Debug.Log(postData.error);
        }
        else
        {
            RegisterModel obj = JsonUtility.FromJson<RegisterModel>(postData.text);
            if (obj.result == 1)
            {
                GameObject.Find("HintMessage").GetComponent<hint>().Hint("欢迎来到聊吧");
            }
            else
            {
                if (obj.result == 0)
                {
                    GameObject.Find("HintMessage").GetComponent<hint>().Hint("连接服务器失败！");//插入数据库失败
                    GameObject.Find("quit room").GetComponent<Btn_Quitroom>().quitroom();
                }
                else
                {
                    Debug.Log(obj);
                }
            }

            Debug.Log(postData.text);
        }
    }

    /*public IEnumerator pull_player(string _url, WWWForm _wForm)
    {
        WWW postData = new WWW(_url, _wForm);
        yield return postData;
        if (postData.error != null)
        {
            Debug.Log(postData.error);
        }
        else
        {
            //PullModel obj = JsonUtility.FromJson<PullModel>(postData.text);
            //对拉回的本房间玩家信息各自克隆玩家
            first_paint(postData.text);
        }
    }*/

    public void socket(string id,string nickname, float position_x, float position_y, float position_z, float velocity_x, float velocity_y, float velocity_z, float forward_x, float forward_z, int roomno)
    {//记得定时请求一次周围人
        SocketModel obj = new SocketModel();
        obj.id = id;
        obj.nickname = nickname;
        /*obj.position_x = position_x;
        obj.position_y = position_y;
        obj.position_z = position_z;
        obj.velocity_x = velocity_x;
        obj.velocity_y = velocity_y;
        obj.velocity_z = velocity_z;
        obj.forward_x = forward_x;
        obj.forward_z = forward_z;
        obj.roomno = roomno;*/
        obj.p = ((uint)((position_x + 1024) * 32) << 16)| ((uint)((position_y + 1024) * 32));
        obj.v = ((uint)((velocity_x + 64) * 8) << 20) | ((uint)((velocity_y + 64) * 8) << 10) | ((uint)((velocity_z + 64) * 8));
        obj.r = ((uint)(roomno) << 30) | ((uint)((position_z + 1024) * 32) << 14) | ((uint)((forward_x + 1) * 64) << 7) | ((uint)((forward_z + 1) * 64));
        string json = JsonUtility.ToJson(obj);
        //Debug.Log(json);
        pos_Socket.AsyncSendData(json,1);
        //AsyncCallback callback = new AsyncCallback(paint);
    }

    /*public void first_paint(string data)//用于首次加载角色
    {
        PullModel obj = JsonUtility.FromJson<PullModel>(data);
        if (obj.socketmodel.Length > 0) {
            foreach (SocketModel item in obj.socketmodel)
            {
                GameObject roleInstance = Instantiate(GameObject.Find("role"), new Vector3(item.position_x, item.position_y, item.position_z), Quaternion.Euler(0f, 0f, 0f));
                roleInstance.transform.LookAt(roleInstance.transform.position + new Vector3(item.forward_x, 0, item.forward_z));
                roleInstance.GetComponent<movement>().setup(item.id, item.nickname);
            }
        }
    }*/

    public void paint(string data)//用于线上角色绘制回调
    {
        try
        {
            Players.Clear();
            //Debug.Log(data);
            PullModel obj = JsonUtility.FromJson<PullModel>(data);
            if (obj.socketmodel.Length > 0)
            {
                foreach (Callback_SocketModel item in obj.socketmodel)
                {
                    float position_x = ((float)(item.p >> 16)) / 32 - 1024;
                    float position_y = ((float)((item.p << 16) >> 16)) / 32 - 1024;
                    float position_z = ((float)((item.r << 2) >> 16)) / 32 - 1024;
                    float velocity_x = ((float)(item.v >> 20)) / 8 - 64;
                    float velocity_y = ((float)((item.v << 12) >> 22)) / 8 - 64;
                    float velocity_z = ((float)((item.v << 22) >> 22)) / 8 - 64; ;
                    float forward_x = ((float)((item.r << 18) >> 25)) / 64 - 1;
                    float forward_z = ((float)((item.r << 25) >> 25)) / 64 - 1;
                    int roomno = (int)(item.r >> 30);
                    GameObject gobj;
                    if ((gobj = GameObject.Find(item.id)) != null && gobj.tag == "Player")//记得优化性能
                    {
                        //gobj.GetComponent<Rigidbody>().velocity = new Vector3(item.velocity_x, item.velocity_y, item.velocity_z);
                        gobj.GetComponent<Rigidbody>().MovePosition(new Vector3(position_x, position_y, position_z));
                        gobj.transform.forward = new Vector3(forward_x, 0f, forward_z);
                        gobj.transform.position = new Vector3(position_x, position_y, position_z);
                        Players.Add(item.id, gobj);
                    }
                    else if (item.id == GameObject.Find("GameManagement").GetComponent<GameManagement>().id)//记得优化性能
                    {
                        /*gobj = GameObject.Find("role");
                        //gobj.GetComponent<Rigidbody>().velocity = new Vector3(item.velocity_x, item.velocity_y, item.velocity_z);
                        gobj.GetComponent<Rigidbody>().MovePosition(new Vector3(item.position_x, item.position_y, item.position_z));
                        gobj.transform.forward = new Vector3(item.forward_x, 0f, item.forward_z);
                        gobj.transform.position = new Vector3(item.position_x, item.position_y, item.position_z);
                        Players.Add(item.id, gobj);*/
                    }
                    else
                    {
                        GameObject tmpObj = Instantiate(prefabs/*, pos, Quaternion.identity*/) as GameObject;
                        tmpObj.transform.position = new Vector3(position_x, position_y, position_z);
                        tmpObj.transform.forward = new Vector3(forward_x, 0f, forward_z);
                        tmpObj.GetComponent<Rigidbody>().velocity = new Vector3(velocity_x, velocity_y, velocity_z);
                        tmpObj.GetComponent<movement>().setup(item.id, item.nickname);
                        Players.Add(item.id, tmpObj);
                        //GameObject roleInstance = Instantiate(GameObject.Find("role"), new Vector3(item.position_x, item.position_y, item.position_z), Quaternion.Euler(0f, 0f, 0f));
                        //roleInstance.transform.LookAt(roleInstance.transform.position + new Vector3(item.forward_x, 0, item.forward_z));
                        //roleInstance.GetComponent<movement>().setup(item.id, item.nickname);
                    }
                    //GameObject.Find(item.id).GetComponent<Rigidbody>().MovePosition(new Vector3(item.position_x, item.position_y, item.position_z));
                    //GameObject.Find(item.id).transform.LookAt(GameObject.Find(item.id).transform.position + new Vector3(item.forward_x, 0, item.forward_z));
                }
                foreach (GameObject Player in GameObject.FindGameObjectsWithTag("Player"))//删除掉可视距离之外的玩家
                {
                    if (!Players.Contains(Player.name) && Player.name != "role") 
                    {
                        //Debug.Log("破坏");
                        //Debug.Log(Player.name);
                        Destroy(Player);
                    }
                }
                Players.Clear();
            }
            else
            {
                foreach (GameObject Player in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (Player.name != "role")
                    {
                        Destroy(Player);
                    }
                }
            }
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log(e.StackTrace);
            return;
        }
    }

    /*public void pullmsg(int no ,string id)//不发消息，只收上下线消息
    {
        MsgModel msgmodel = new MsgModel();
        msgmodel.type = 4;
        msgmodel.position_x = 0;
        msgmodel.position_y = 0;
        msgmodel.position_z = 0;
        msgmodel.roomno = no;
        msgmodel.username = id;
        msgmodel.nickname = null;
        msgmodel.channel = 0;
        msgmodel.content = null;
        string json = JsonUtility.ToJson(msgmodel);
        msg_Socket.AsyncSendData(json,2);
        //Callback_MsgModel result = JsonUtility.FromJson<Callback_MsgModel>(res);
    }
    public void pull_back(string data)//用于上下线信息回调
    {
        try
        {
            Callback_MsgModel obj = JsonUtility.FromJson<Callback_MsgModel>(data);
            if (obj.msgmodel.Length > 0)
            {
                foreach (MsgModel item in obj.msgmodel)
                {

                    if (item.type != 1 && item.type != 2 && item.type != 3) 
                    {
                        Debug.Log("信息类型错误！ 错误type："+ item.type);
                        continue;
                    }
                    if (item.type == 3)
                    {
                        GameObject.Find("Chat UI").GetComponent<ChatControl>().AddContent(item.type, item.position_x, item.position_y, item.position_z, item.roomno, item.username, item.nickname, item.channel, item.content);
                        continue;
                    }
                    if (item.username == GameObject.Find("GameManagement").GetComponent<GameManagement>().id)//避免绘制自己
                    {
                        continue;
                    }
                    if (item.type == 1 && item.roomno == GameObject.Find("GameManagement").GetComponent<GameManagement>().RoomNo)
                    {
                        GameObject roleInstance = Instantiate(GameObject.Find("role"), new Vector3(item.position_x, item.position_y, item.position_z), Quaternion.Euler(0f, 0f, 0f));
                        roleInstance.GetComponent<movement>().setup(item.username, item.nickname);
                        GameObject.Find("Chat UI").GetComponent<ChatControl>().AddContent(item.type, item.position_x, item.position_y, item.position_z, item.roomno, item.username, item.nickname, item.channel, item.content);
                    }
                    if (item.type == 2 && item.roomno == GameObject.Find("GameManagement").GetComponent<GameManagement>().RoomNo)
                    {
                        Destroy(GameObject.Find(item.username));
                        GameObject.Find("Chat UI").GetComponent<ChatControl>().AddContent(item.type, item.position_x, item.position_y, item.position_z, item.roomno, item.username, item.nickname, item.channel, item.content);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log(data);
            return;
        }
    }
    */
    public void sendmsg(int type, float position_x, float position_y, float position_z, int roomno, string id, string nickname, int channel, string content)//发各种消息，收玩家发的消息
    {
        MsgModel msgmodel = new MsgModel();
        msgmodel.type = type;
        msgmodel.position_x = position_x;
        msgmodel.position_y = position_y;
        msgmodel.position_z = position_z;
        msgmodel.roomno = roomno;
        msgmodel.username = id;
        msgmodel.nickname = nickname;
        msgmodel.channel = channel;
        msgmodel.content = content;
        string json = JsonUtility.ToJson(msgmodel);
        msg_Socket.AsyncSendData(json,3);
        //Callback_MsgModel result = JsonUtility.FromJson<Callback_MsgModel>(res);
        //将消息显示在聊天框中
    }
    public void KeepReceive()
    {
        msg_Socket.KeepReceive();
    }
    public void send_back(string data)//用于接收到玩家发送的消息时的回调
    {
        Debug.Log(data);
        try
        {
            Callback_MsgModel obj = JsonUtility.FromJson<Callback_MsgModel>(data);
            if (obj.msgmodel.Length > 0)
            {
                foreach (MsgModel item in obj.msgmodel)
                {
                    if (item.type != 1 && item.type != 2 && item.type != 3)
                    {
                        Debug.Log("信息类型错误！ 错误type：" + item.type);
                        continue;
                    }
                    if (item.type == 3)
                    {
                        GameObject.Find("Chat UI").GetComponent<ChatControl>().AddContent(item.type, item.position_x, item.position_y, item.position_z, item.roomno, item.username, item.nickname, item.channel, item.content);
                        continue;
                    }
                    if (item.username == GameObject.Find("GameManagement").GetComponent<GameManagement>().id)//避免绘制自己
                    {
                        continue;
                    }
                    if (item.type == 1 && item.roomno == GameObject.Find("GameManagement").GetComponent<GameManagement>().RoomNo)
                    {
                        //GameObject roleInstance = Instantiate(prefabs, new Vector3(item.position_x, item.position_y, item.position_z), Quaternion.Euler(0f, 0f, 0f)) as GameObject;
                        //roleInstance.GetComponent<movement>().setup(item.username, item.nickname);
                        GameObject.Find("Chat UI").GetComponent<ChatControl>().AddContent(item.type, item.position_x, item.position_y, item.position_z, item.roomno, item.username, item.nickname, item.channel, item.content);
                    }
                    if (item.type == 2 && item.roomno == GameObject.Find("GameManagement").GetComponent<GameManagement>().RoomNo)
                    {
                        foreach (GameObject Player in GameObject.FindGameObjectsWithTag("Player"))
                        {
                            if (Player.name == item.username)
                            {
                                Destroy(Player);
                            }
                        }
                        //Destroy(GameObject.Find(item.username));
                        GameObject.Find("Chat UI").GetComponent<ChatControl>().AddContent(item.type, item.position_x, item.position_y, item.position_z, item.roomno, item.username, item.nickname, item.channel, item.content);
                    }
                }
            }
            
            //将消息显示在聊天框中
        }
        catch(Exception e) {
            Debug.Log(e.Message);
            Debug.Log(data);
            return;
        }
    }

    public bool is_pos_connected()
    {
        return pos_Socket.is_connected();
    }

    public bool is_msg_connected()
    {
        return msg_Socket.is_connected();
    }

    public bool begin_connect()
    {
        //pos_Socket.ConnectServer(ip, pos_port);//消息系统与游戏系统使用不同的端口
       // msg_Socket.ConnectServer(ip, msg_port);
        return (pos_Socket.ConnectServer(ip, pos_port) && msg_Socket.ConnectServer(ip, msg_port));
    }
    public void close_connect()
    {
        pos_Socket.CloseServer();
        msg_Socket.CloseServer();
    }

}

[System.Serializable]
public class LoginModel
{
    public int result;
    public string username;
    public string nickname;
}
[System.Serializable]
public class RegisterModel
{
    public int result;
}
[System.Serializable]
public class Callback_SocketModel
{
    public string id;
    public string nickname;
    /*public float position_x;
    public float position_y;
    public float position_z;
    public float velocity_x;
    public float velocity_y;
    public float velocity_z;
    public float forward_x;
    public float forward_z;
    public int roomno;*/
    public uint p;//前16位存储position_x,后16位存储position_y
    public uint v;//分10位存储velocity_xyz
    public uint r;//前2位存储场景号，后数16位存储position_z,后数14位分7位存储forward
}
[System.Serializable]
public class SocketModel//简化变量名并浓缩为三个int型数据，想办法取得服务器接收到至少一次id与nickname的信号(例如回传id为自身id的同时nickname为null)，之后不再传输id与nickname（null）。
{
    public string id;
    public string nickname;
    /*public float position_x;
    public float position_y;
    public float position_z;
    public float velocity_x;
    public float velocity_y;
    public float velocity_z;
    public float forward_x;
    public float forward_z;
    public int roomno;*/
    public uint p;//前16位存储position_x,后16位存储position_y
    public uint v;//分10位存储velocity_xyz
    public uint r;//前2位存储场景号，后数16位存储position_z,后数14位分7位存储forward
}
[System.Serializable]
public class PullModel
{
    public Callback_SocketModel[] socketmodel;
}
[System.Serializable]
public class MsgModel
{
    public int type;
    /*类型说明:
    1:(发送)上线信息
    2:(发送)下线信息
    3:(发送)玩家发的消息
    4:取得上下线信息
    5:取得玩家发的消息
    */
    public float position_x;
    public float position_y;
    public float position_z;//信息位置
    public int roomno;//信息房间
    public string username;
    public string nickname;//发送者信息
    public int channel;//消息范围 1世界2频道3附近4私聊
    public string content;//消息内容
}
[System.Serializable]
public class Callback_MsgModel
{
    public MsgModel[] msgmodel;
}