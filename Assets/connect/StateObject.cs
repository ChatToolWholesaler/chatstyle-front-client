﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System;
using Net;
using UnityEngine.SceneManagement;



public class StateObject : MonoBehaviour {

    public string ip = "vipgz1.idcfengye.com";
    public int pos_port = 10134;
    public int msg_port = 10135;
    public string urlip = "192.168.1.124";
    public ClientSocket pos_Socket;
    public ClientSocket msg_Socket;
    public GameObject prefabs = null;
    public GameObject friendprefabs = null;
    public GameObject blackprefabs = null;
    public GameObject requestprefabs = null;
    public GameObject GM;
    //public Hashtable Players;//存储当前附近的玩家
    public List<GameObject> Players;

    void Awake()
    {
        pos_Socket = new ClientSocket();
        msg_Socket = new ClientSocket();
        //Players = new Hashtable();
        Players = new List<GameObject>();
        GM = GameObject.Find("GameManagement");
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
            LoginModel obj = JsonUtility.FromJson<LoginModel>(postData.text);
            if (obj.code == 200)
            {
                GameObject.Find("HintMessage").GetComponent<hint>().Hint("登陆成功！ id:" + obj.data.user_id);
                GameObject.Find("GameManagement").GetComponent<GameManagement>().LoginOK(obj.data.user_id.ToString(), obj.data.nickname, obj.data.sign, obj.data.gender);
            }
            else if (obj.code == 400)
            {
                GameObject.Find("ErrorText").GetComponent<Text>().text = "该账号已登录";
            }
            else if (obj.code == 403)
            {
                GameObject.Find("ErrorText").GetComponent<Text>().text = "账号不存在或者密码不匹配";
            }
            else {
                Debug.Log(obj.data.user_id);
                Debug.Log(obj.data.nickname);
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
            if (obj.code == 200)
            {
                GameObject.Find("HintMessage").GetComponent<hint>().Hint("注册成功！");
                GameObject.Find("Register UI").GetComponent<LoginControl>().Hide();
                GameObject.Find("Login UI").GetComponent<LoginControl>().Show();
            }
            else
            {
                if (obj.code == 403)
                {
                    GameObject.Find("ErrorText_r").GetComponent<Text>().text = "账号或者密码已存在";
                }//考虑昵称是否存在
                else if (obj.code == 412)
                {
                    GameObject.Find("ErrorText_r").GetComponent<Text>().text = "查询错误";
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
            GoOnlineModel obj = JsonUtility.FromJson<GoOnlineModel>(postData.text);
            if (obj.code == 200)
            {
                GameObject.Find("HintMessage").GetComponent<hint>().Hint("欢迎来到聊吧");
                //GameObject tmpObj = Instantiate(friendprefabs/*, pos, Quaternion.identity*/) as GameObject;
                //GameObject friendlist = GameObject.Find("Runtime UI/Association_Call/ListBg/Friend/FriendList");
                //tmpObj.transform.parent = friendlist.transform;
                //tmpObj.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
                //tmpObj.GetComponent<FriendItemControl>().setup("卿","1","♂","千里之行 ，始于足下");//添加获取社交列表的请求
                WWWForm form0 = new WWWForm();
                form0.AddField("userId", int.Parse(GM.GetComponent<GameManagement>().id));
                form0.AddField("type", 0);
                StartCoroutine(acquire("http://" + GameObject.Find("StateObject").GetComponent<StateObject>().urlip + ":3000/api/v1/friend/getFriendsList", form0, 0));
                WWWForm form1 = new WWWForm();
                form1.AddField("userId", int.Parse(GM.GetComponent<GameManagement>().id));
                form1.AddField("type", 1);
                StartCoroutine(acquire("http://" + GameObject.Find("StateObject").GetComponent<StateObject>().urlip + ":3000/api/v1/friend/getFriendsList", form1, 1));
                WWWForm form2 = new WWWForm();
                form2.AddField("userId", int.Parse(GM.GetComponent<GameManagement>().id));
                form2.AddField("type", 2);
                StartCoroutine(acquire("http://" + GameObject.Find("StateObject").GetComponent<StateObject>().urlip + ":3000/api/v1/friend/getFriendsList", form2, 2));
            }
            else
            {
                if (obj.code == 400)
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

    public IEnumerator acquire(string _url, WWWForm _wForm, int type)
    {
        WWW postData = new WWW(_url, _wForm);
        yield return postData;
        if (postData.error != null)
        {
            Debug.Log(postData.error);
        }
        else
        {
            FriendListModel obj = JsonUtility.FromJson<FriendListModel>(postData.text);
            GameObject list = null;
            GameObject item = null;
            switch (type)
            {
                case 0:
                    list = GameObject.Find("Runtime UI/Association_Call/ListBg/Friend/FriendList");
                    item = friendprefabs;
                    break;
                case 1:
                    list = GameObject.Find("Runtime UI/Association_Call/ListBg/Black/BlackList");
                    item = blackprefabs;
                    break;
                case 2:
                    list = GameObject.Find("Runtime UI/Association_Call/ListBg/Request/RequestList");
                    item = requestprefabs;
                    break;
                default:
                    break;
            }
            foreach (Transform child in list.transform)//添加项前先清空列表
            {
                Destroy(child.gameObject);
            }
            if (obj.code == 200)
            {
                foreach (FriendListData list_item in obj.data)
                {
                    GameObject tmpObj = Instantiate(item/*, pos, Quaternion.identity*/) as GameObject;
                    tmpObj.transform.parent = list.transform;
                    tmpObj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    switch (type)
                    {
                        case 0:
                            tmpObj.GetComponent<FriendItemControl>().setup(list_item.nickname, list_item.friendId.ToString(), (list_item.gender ? "♂" : "♀"), list_item.sign);
                            break;
                        case 1:
                            tmpObj.GetComponent<BlackItemControl>().setup(list_item.nickname, list_item.friendId.ToString(), (list_item.gender ? "♂" : "♀"), list_item.sign);
                            break;
                        case 2:
                            tmpObj.GetComponent<RequestItemControl>().setup(list_item.nickname, list_item.friendId.ToString(), (list_item.gender ? "♂" : "♀"), list_item.sign);
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                if (obj.code == 400)
                {
                    switch (type)
                    {
                        case 0:
                            GameObject.Find("HintMessage").GetComponent<hint>().Hint("获取好友列表失败！");
                            break;
                        case 1:
                            GameObject.Find("HintMessage").GetComponent<hint>().Hint("获取黑名单列表失败！");
                            //Debug.Log(postData.text);
                            break;
                        case 2:
                            GameObject.Find("HintMessage").GetComponent<hint>().Hint("获取好友请求列表失败！");
                            break;
                        default:
                            break;
                    }

                }
                else
                {
                    Debug.Log(obj);
                }
            }

            Debug.Log(postData.text);
        }
    }

    public IEnumerator set_sign(string _url, WWWForm _wForm)
    {
        WWW postData = new WWW(_url, _wForm);
        yield return postData;
        if (postData.error != null)
        {
            Debug.Log(postData.error);
        }
        else
        {
            GoOnlineModel obj = JsonUtility.FromJson<GoOnlineModel>(postData.text);
            if (obj.code == 200)
            {
                GameObject.Find("HintMessage").GetComponent<hint>().Hint("修改成功");
            }
            else
            {
                if (obj.code == 400)
                {
                    GameObject.Find("HintMessage").GetComponent<hint>().Hint("修改失败！");//修改签名失败
                }
                else
                {
                    Debug.Log(obj);
                }
            }

            Debug.Log(postData.text);
        }
    }

    public IEnumerator get_info(string _url, WWWForm _wForm)
    {
        WWW postData = new WWW(_url, _wForm);
        yield return postData;
        if (postData.error != null)
        {
            Debug.Log(postData.error);
        }
        else
        {
            UserInfoModel obj = JsonUtility.FromJson<UserInfoModel>(postData.text);
            if (obj.code == 200)
            {
                GameObject.Find("Information UI").GetComponent<InformationControl>().Show(obj.data.nickname, obj.data.userid.ToString(), (obj.data.gender ? "♂" : "♀"), obj.data.sign);
            }
            else
            {
                if (obj.code == 404)
                {
                    GameObject.Find("HintMessage").GetComponent<hint>().Hint("查看信息失败！");//查看玩家失败
                }
                else
                {
                    Debug.Log(obj);
                }
            }

            Debug.Log(postData.text);
        }
    }

    public IEnumerator apply_pull(string _url, WWWForm _wForm)
    {
        WWW postData = new WWW(_url, _wForm);
        yield return postData;
        if (postData.error != null)
        {
            Debug.Log(postData.error);
        }
        else
        {
            GoOnlineModel obj = JsonUtility.FromJson<GoOnlineModel>(postData.text);
            if (obj.code == 200)
            {
                GameObject.Find("HintMessage").GetComponent<hint>().Hint("操作成功！");
                WWWForm form0 = new WWWForm();
                form0.AddField("userId", int.Parse(GM.GetComponent<GameManagement>().id));
                form0.AddField("type", 0);
                StartCoroutine(acquire("http://localhost:3000/api/v1/friend/getFriendsList", form0, 0));
                WWWForm form1 = new WWWForm();
                form1.AddField("userId", int.Parse(GM.GetComponent<GameManagement>().id));
                form1.AddField("type", 1);
                StartCoroutine(acquire("http://localhost:3000/api/v1/friend/getFriendsList", form1, 1));
                WWWForm form2 = new WWWForm();
                form2.AddField("userId", int.Parse(GM.GetComponent<GameManagement>().id));
                form2.AddField("type", 2);
                StartCoroutine(acquire("http://localhost:3000/api/v1/friend/getFriendsList", form2, 2));
            }
            else
            {
                if (obj.code == 400)
                {
                    GameObject.Find("HintMessage").GetComponent<hint>().Hint("操作错误");//申请好友或者拉黑失败
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

    public void socket(string id, string nickname, float position_x, float position_y, float position_z, float velocity_x, float velocity_y, float velocity_z, float forward_x, float forward_z, int roomno)
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
        if (isinroom)
        {
            position_x -= GM.GetComponent<GameManagement>().SpawnPosition[roomno - 1].x;
            position_y -= GM.GetComponent<GameManagement>().SpawnPosition[roomno - 1].y;
            position_z -= GM.GetComponent<GameManagement>().SpawnPosition[roomno - 1].z;
        }
        obj.p = ((uint)((position_x + 1024) * 32) << 16) | ((uint)((position_y + 1024) * 32));
        obj.v = (((uint)(cur_tex) << 30) | (uint)((velocity_x + 64) * 8) << 20) | ((uint)((velocity_y + 64) * 8) << 10) | ((uint)((velocity_z + 64) * 8));
        obj.r = ((uint)(roomno) << 30) | ((uint)((position_z + 1024) * 32) << 14) | ((uint)((forward_x + 1) * 64) << 7) | ((uint)((forward_z + 1) * 64));
        string json = JsonUtility.ToJson(obj);
        //Debug.Log(json);
        pos_Socket.AsyncSendData(json, 1);
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

            //Debug.Log(data);
            PullModel obj = JsonUtility.FromJson<PullModel>(data);
            if (obj.socketmodel.Length > 0)
            {
                if (obj.socketmodel[0].id != null && obj.socketmodel[0].id != "")//此时为周围玩家加载帧
                {
                    Players.Clear();

                    foreach (Callback_SocketModel item in obj.socketmodel)
                    {
                        int roomno = (int)(item.r >> 30);
                        float position_x = ((float)(item.p >> 16)) / 32 - 1024 + GM.GetComponent<GameManagement>().SpawnPosition[roomno - 1].x;
                        float position_y = ((float)((item.p << 16) >> 16)) / 32 - 1024 + GM.GetComponent<GameManagement>().SpawnPosition[roomno - 1].y;
                        float position_z = ((float)((item.r << 2) >> 16)) / 32 - 1024 + GM.GetComponent<GameManagement>().SpawnPosition[roomno - 1].z;
                        float velocity_x = ((float)(item.v >> 20)) / 8 - 64;
                        float velocity_y = ((float)((item.v << 12) >> 22)) / 8 - 64;
                        float velocity_z = ((float)((item.v << 22) >> 22)) / 8 - 64; ;
                        float forward_x = ((float)((item.r << 18) >> 25)) / 64 - 1;
                        float forward_z = ((float)((item.r << 25) >> 25)) / 64 - 1;
                        int tex = (int)(item.v >> 30);

                        GameObject gobj = null;
                        bool temp = false;
                        foreach (GameObject P in GameObject.FindGameObjectsWithTag("Player"))//判断是否存在这个玩家
                        {
                            if (P.name == item.id)
                            {
                                temp = true;
                                gobj = P;
                                break;
                            }
                        }
                        if (temp)
                        {
                            //gobj.GetComponent<Rigidbody>().velocity = new Vector3(item.velocity_x, item.velocity_y, item.velocity_z);
                            gobj.GetComponent<Rigidbody>().MovePosition(new Vector3(position_x, position_y, position_z));
                            gobj.transform.forward = new Vector3(forward_x, 0f, forward_z);
                            gobj.transform.position = new Vector3(position_x, position_y, position_z);
                            gobj.GetComponent<LoadPlayerTexture>().SetOfficialOutlook(tex);
                            Players.Add(gobj);
                        }
                        else if (item.id == GM.GetComponent<GameManagement>().id)
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
                            tmpObj.GetComponent<LoadPlayerTexture>().SetOfficialOutlook(tex);
                            Players.Add(tmpObj);
                            //GameObject roleInstance = Instantiate(GameObject.Find("role"), new Vector3(item.position_x, item.position_y, item.position_z), Quaternion.Euler(0f, 0f, 0f));
                            //roleInstance.transform.LookAt(roleInstance.transform.position + new Vector3(item.forward_x, 0, item.forward_z));
                            //roleInstance.GetComponent<movement>().setup(item.id, item.nickname);
                        }
                        //GameObject.Find(item.id).GetComponent<Rigidbody>().MovePosition(new Vector3(item.position_x, item.position_y, item.position_z));
                        //GameObject.Find(item.id).transform.LookAt(GameObject.Find(item.id).transform.position + new Vector3(item.forward_x, 0, item.forward_z));
                    }
                    foreach (GameObject Player in GameObject.FindGameObjectsWithTag("Player"))//删除掉可视距离之外的玩家
                    {
                        if (!Players.Contains(Player) && Player.name != "role")
                        {
                            //Debug.Log("破坏");
                            //Debug.Log(Player.name);
                            Destroy(Player);
                        }
                    }
                }
                else//此时未加载周围玩家
                {
                    int i = 0;
                    foreach (GameObject Player in Players)
                    {
                        Callback_SocketModel item = obj.socketmodel[i];
                        int roomno = (int)(item.r >> 30);
                        float position_x = ((float)(item.p >> 16)) / 32 - 1024 + GM.GetComponent<GameManagement>().SpawnPosition[roomno - 1].x;
                        float position_y = ((float)((item.p << 16) >> 16)) / 32 - 1024 + GM.GetComponent<GameManagement>().SpawnPosition[roomno - 1].y;
                        float position_z = ((float)((item.r << 2) >> 16)) / 32 - 1024 + GM.GetComponent<GameManagement>().SpawnPosition[roomno - 1].z;
                        float velocity_x = ((float)(item.v >> 20)) / 8 - 64;
                        float velocity_y = ((float)((item.v << 12) >> 22)) / 8 - 64;
                        float velocity_z = ((float)((item.v << 22) >> 22)) / 8 - 64; ;
                        float forward_x = ((float)((item.r << 18) >> 25)) / 64 - 1;
                        float forward_z = ((float)((item.r << 25) >> 25)) / 64 - 1;
                        Player.GetComponent<Rigidbody>().MovePosition(new Vector3(position_x, position_y, position_z));
                        Player.transform.forward = new Vector3(forward_x, 0f, forward_z);
                        Player.transform.position = new Vector3(position_x, position_y, position_z);
                        i++;
                    }
                }
            }
            else
            {
                if (isinroom)//周围没人（必须排除编辑人物的情况）时删掉所有现存周围玩家
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
        }
        catch (Exception e)
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
        msg_Socket.AsyncSendData(json, 3);
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
                    if (item.username == GameObject.Find("GameManagement").GetComponent<GameManagement>().id)//避免显示自己的上下线消息
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
        catch (Exception e) {
            Debug.Log(e.Message);
            Debug.Log(e.StackTrace);
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

    public static StateObject instance;
    public bool isinroom = true;
    private int deltaframe = 0;//计帧数
    public string id;
    public string nickname;
    public string sign;
    public bool gender;
    public int roomno;
    public string roomname;
    public Vector3 position;
    public Vector3 forward;
    public Vector3 velocity;
    public int cur_tex;

    void Start()
    {
        //避免出现多个该物体
        if (instance != null)
        {
            return;
        }
        else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        SceneManager.sceneLoaded += (var, var2) =>
        {
            if (var.buildIndex == 0)
            {
                if (!isinroom)
                {
                    StartCoroutine(EnterPresetsGallery());
                    
                }
                isinroom = true;
            }
            else
            {
                isinroom = false;
            }
        };
    }
    IEnumerator EnterPresetsGallery()
    {
        yield return new WaitForEndOfFrame();
        GM = GameObject.Find("GameManagement");
        GM.GetComponent<GameManagement>().LoginOK(id, nickname, sign, gender);
        GM.GetComponent<GameManagement>().RoomNo = roomno;
        GameObject.Find("Selectroom UI").GetComponent<SelectroomControl>().Hide();
        GameObject.Find("RoomNo").GetComponent<Text>().text = roomname;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameObject.Find("role").transform.position = position;
        GameObject.Find("role").transform.forward = forward;
        GameObject.Find("role").GetComponent<Rigidbody>().velocity = velocity;
        GameObject.Find("BGM").GetComponent<BGMControl>().LoadBGM(roomno - 1);
        GameObject.Find("BGM").GetComponent<AudioSource>().Play();
        GameObject.Find("Main Camera").GetComponent<Camera>().cullingMask = -1;
        GameObject.Find("Main Camera").GetComponent<Camera>().cullingMask &= ~(1 << 10);//关闭圆点层
        GameObject.Find("Manu").GetComponent<ManuControl>().EnableMouseMove();
        GameObject.Find("Manu").GetComponent<ManuControl>().EnableManuControl();
        GameObject.Find("Map").GetComponent<MapControl>().Show();
        GameObject.Find("role").GetComponent<movement>().EnableMoveControl();
        GameObject.Find("role").GetComponent<movement>().setup("role", nickname);
        GameObject.Find("Chat UI").GetComponent<ChatControl>().HalfShow();
        GameObject.Find("Runtime UI/Association_Call").GetComponent<AssociationControl>().setupinfo(gender, nickname, sign);
        GM.GetComponent<GameManagement>().con_obj = GameObject.Find("StateObject");
        KeepReceive();
        //不要显示登录页面，直接进入房间
    }
    void Update()
    {
        
        if (!isinroom)
        {
            deltaframe++;
            if (deltaframe >= 10)
            {
                deltaframe = 0;
                sendmsg(5, 0f, 0f, 0f, 1, null, null, 1, null);
            }
            if (deltaframe % 3 == 0)
            {
                socket(null, null, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0);
            }
        }
    }
}

[System.Serializable]
public class LoginData
{
    public int user_id;
    public string nickname;
    public string sign;
    public bool gender;
    public string token;
}
[System.Serializable]
public class LoginModel
{
    public int code;
    public LoginData data;
}
[System.Serializable]
public class RegisterData
{
    public int user_id;
}
[System.Serializable]
public class RegisterModel
{
    public int code;
    public RegisterData data;
}
[System.Serializable]
public class GoOnlineModel
{
    public int code;
}
[System.Serializable]
public class UserInfoData
{
    public string nickname;
    public int userid;
    public bool gender;
    public string sign;
}
[System.Serializable]
public class UserInfoModel
{
    public int code;
    public UserInfoData data;
}
[System.Serializable]
public class FriendListData
{
    public string nickname;
    public int friendId;
    public bool gender;
    public string sign;
}
[System.Serializable]
public class FriendListModel
{
    public int code;
    public FriendListData[] data;
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