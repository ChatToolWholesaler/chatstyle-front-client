
using System.IO;

using System.Text;

using System;

using UnityEngine;

using System.Collections;

using System.Collections.Generic;

using System.Net;

using System.Net.Sockets;


namespace Net
{

    public class ByteBuffer
    {

        MemoryStream stream = null;

        BinaryWriter writer = null;

        BinaryReader reader = null;



        public ByteBuffer()
        {

            stream = new MemoryStream();

            writer = new BinaryWriter(stream);

        }



        public ByteBuffer(byte[] data)
        {

            if (data != null)
            {

                stream = new MemoryStream(data);

                reader = new BinaryReader(stream);

            }
            else
            {

                stream = new MemoryStream();

                writer = new BinaryWriter(stream);

            }

        }



        public void Close()
        {

            if (writer != null) writer.Close();

            if (reader != null) reader.Close();



            stream.Close();

            writer = null;

            reader = null;

            stream = null;

        }



        public void WriteByte(byte v)
        {

            writer.Write(v);

        }



        public void WriteInt(int v)
        {

            writer.Write((int)v);

        }



        public void WriteShort(ushort v)
        {

            writer.Write((ushort)v);

        }



        public void WriteLong(long v)
        {

            writer.Write((long)v);

        }



        public void WriteFloat(float v)
        {

            byte[] temp = BitConverter.GetBytes(v);

            Array.Reverse(temp);

            writer.Write(BitConverter.ToSingle(temp, 0));

        }



        public void WriteDouble(double v)
        {

            byte[] temp = BitConverter.GetBytes(v);

            Array.Reverse(temp);

            writer.Write(BitConverter.ToDouble(temp, 0));

        }



        public void WriteString(string v)
        {

            byte[] bytes = Encoding.UTF8.GetBytes(v);

            //writer.Write((ushort)bytes.Length);

            writer.Write(bytes);

        }



        public void WriteBytes(byte[] v)
        {

            writer.Write((int)v.Length);

            writer.Write(v);

        }



        public byte ReadByte()
        {

            return reader.ReadByte();

        }



        public int ReadInt()
        {

            return (int)reader.ReadInt32();

        }



        public ushort ReadShort()
        {

            return (ushort)reader.ReadInt16();

        }



        public long ReadLong()
        {

            return (long)reader.ReadInt64();

        }



        public float ReadFloat()
        {

            byte[] temp = BitConverter.GetBytes(reader.ReadSingle());

            Array.Reverse(temp);

            return BitConverter.ToSingle(temp, 0);

        }



        public double ReadDouble()
        {

            byte[] temp = BitConverter.GetBytes(reader.ReadDouble());

            Array.Reverse(temp);

            return BitConverter.ToDouble(temp, 0);

        }



        public string ReadString(int count)
        {

            //ushort len = ReadShort();

            //byte[] buffer = new byte[len];

            //buffer = reader.ReadBytes(len);

            byte[] buffer = new byte[count];

            buffer = reader.ReadBytes(count);

            return Encoding.UTF8.GetString(buffer);

        }



        public byte[] ReadBytes()
        {

            int len = ReadInt();

            return reader.ReadBytes(len);

        }



        public byte[] ToBytes()
        {

            writer.Flush();

            return stream.ToArray();

        }



        public void Flush()
        {

            writer.Flush();

        }

    }

    public class ClientSocket

    {
        public List<Action> functionsToRunInMainThread = new List<Action>();
        private  byte[] result;

        private  Socket clientSocket;

        //是否已连接的标识

        public bool IsConnected = false;

        public int type;
        public GameObject hintmessage;
        public GameObject stateobject;

        public ClientSocket()
        {

            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            hintmessage = GameObject.Find("HintMessage");
            stateobject = GameObject.Find("StateObject");
            result = new byte[1024];
        }



        /// <summary>

        /// 连接指定IP和端口的服务器

        /// </summary>

        /// <param name="ip"></param>

        /// <param name="port"></param>

        public bool ConnectServer(string address, int port)

        {
            if (clientSocket.Connected) { GameObject.Find("HintMessage").GetComponent<hint>().Hint("该账号尚未断线，请稍后重试！");return false; }

            IPAddress mIp = IPAddress.Parse(address);

            //IPAddress[] Ips = Dns.GetHostAddresses(address);

            IPEndPoint ip_end_point = new IPEndPoint(mIp , port);

            try
            {

                clientSocket.Connect(ip_end_point);

                //clientSocket.SetSocketOption(SocketOptionLevel.Tcp,SocketOptionName.NoDelay,true);

                IsConnected = true;

                Debug.Log("连接服务器成功");

                return true;
            }

            catch(SocketException exc)

            {

                IsConnected = false;

                Debug.Log("连接服务器失败，错误信息："+ exc.Message);
                GameObject.Find("HintMessage").GetComponent<hint>().Hint("连接服务器失败！");

                return false;

            }

            //服务器下发数据长度

        }

        public void CloseServer()
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            IsConnected = false;
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public bool is_connected()
        {
            return clientSocket.Connected;
        }

        /// <summary>

        /// 发送数据给服务器

        /// </summary>

        public string SendData(string data)//同步发送消息

        {

            if (IsConnected == false) { hintmessage.GetComponent<hint>().Hint("网络连接中断"); return null; }

            try
            {
                ByteBuffer buffer = new ByteBuffer();



                buffer.WriteString(data);

                clientSocket.Send(WriteMessage(buffer.ToBytes()));
                /*AsyncCallback callback=null;
                object state=null;
                clientSocket.BeginSend(WriteMessage(buffer.ToBytes()),0, 1024, 0, callback, state);*/
            }
            catch {
                IsConnected = false;

                hintmessage.GetComponent<hint>().Hint("网络连接中断");

                return null;
            }




            int receiveLength = clientSocket.Receive(result);

            ByteBuffer buffer_r = new ByteBuffer(result);

            string data_r = buffer_r.ReadString(receiveLength);

            return data_r;

        }

        /*public void callback(IAsyncResult i)
        {
            ByteBuffer buffer_r = new ByteBuffer(result);

            string data_r = buffer_r.ReadString(1024);

            if (result == null|| data_r=="") { GameObject.Find("HintMessage").GetComponent<hint>().Hint("网络连接中断"); CloseServer(); return; }
            else {

            }

            //若为null或“”则断开连接并报错，否则回调到上层的paint（）
        }*/
        

        public void AsyncSendData(string data,int type)//异步发送消息

        {
            if (functionsToRunInMainThread.Count > 0) {
                Action func = functionsToRunInMainThread[0];
                functionsToRunInMainThread.RemoveAt(0);
                func();
            }
            
            this.type = type;
            if ( !IsConnected &&!clientSocket.Connected) { hintmessage.GetComponent<hint>().Hint("网络连接中断，请稍后重试"); CloseServer();return; }
           

            try
            {
                ByteBuffer buffer = new ByteBuffer();

                buffer.WriteString(data);

                byte[] send = WriteMessage(buffer.ToBytes());

                clientSocket.BeginSend(send, 0, send.Length, 0, asyncResult => {
                    clientSocket.EndSend(asyncResult);
                }, null);
                
                /*object state=null;
                clientSocket.BeginSend(WriteMessage(buffer.ToBytes()),0, 1024, 0, callback, state);*/
            }
            catch(Exception e)
            {
                IsConnected = false;

                hintmessage.GetComponent<hint>().Hint("网络连接中断");
                Debug.Log(e.Message);

                CloseServer();

                return;
            }
            

            clientSocket.BeginReceive(result, 0, result.Length, 0,asyncResult => {
                //推测极有可能接收回调被长时间挂起
                    try
                    {
                        int receiveLength = clientSocket.EndReceive(asyncResult);
                        if (receiveLength == 0)
                        {
                            functionsToRunInMainThread.Add(() => {
                            hintmessage.GetComponent<hint>().Hint("网络连接中断，请稍后重试");
                            });

                            CloseServer();
                        }
                    ByteBuffer buffer_r = new ByteBuffer(result);

                        string data_r = buffer_r.ReadString(receiveLength);
                    //记得处理粘包
                        functionsToRunInMainThread.Add(() => {
                            switch (type)
                            {
                                case 1:
                                    stateobject.GetComponent<StateObject>().paint(data_r);
                                    break;
                                case 2:
                                    //stateobject.GetComponent<StateObject>().pull_back(data_r);
                                    break;
                                case 3:
                                    stateobject.GetComponent<StateObject>().send_back(data_r);
                                    break;
                                default:
                                    break;
                            }
                        });
                    
                    }
                    catch(ObjectDisposedException e)
                    {
                            Debug.Log(e.Message);
                            Debug.Log(e.StackTrace);
                            Debug.Log(e.ObjectName);
                        
                        /*IsConnected = false;
                        functionsToRunInMainThread.Add(() => {
                            hintmessage.GetComponent<hint>().Hint("网络连接中断");
                        });
                        Debug.Log(e.Message);
                    

                        CloseServer();*/
                    }//由于回调线程无法与主线程同时修改游戏对象，故加入回调序列供主线程使用
                
            }, null);
            /*int receiveLength = clientSocket.BeginReceive(result, 0, 1024, 0, Callback, null);

            ByteBuffer buffer_r = new ByteBuffer(result);

            string data_r = buffer_r.ReadString(receiveLength);

            return data_r;*/

        }

        public void KeepReceive()
        {
            /*if (functionsToRunInMainThread.Count > 0)
            {
                Action func = functionsToRunInMainThread[0];
                functionsToRunInMainThread.RemoveAt(0);
                func();
            }*/
            if (!clientSocket.Connected) {
                functionsToRunInMainThread.Add(() => {
                    hintmessage.GetComponent<hint>().Hint("网络连接中断，请稍后重试");
                }); CloseServer(); return; }

            clientSocket.BeginReceive(result, 0, result.Length, 0, asyncResult => {
                //推测极有可能接收回调被长时间挂起
                try
                {
                    int receiveLength = clientSocket.EndReceive(asyncResult);
                    if (receiveLength == 0)
                    {
                        functionsToRunInMainThread.Add(() => {
                            hintmessage.GetComponent<hint>().Hint("网络连接中断，请稍后重试");
                        });
                        
                        CloseServer();
                    }
                    else
                    {
                        ByteBuffer buffer_r = new ByteBuffer(result);

                        string data_r = buffer_r.ReadString(receiveLength);
                        //记得处理粘包
                        functionsToRunInMainThread.Add(() => {
                            stateobject.GetComponent<StateObject>().send_back(data_r);
                        });
                        KeepReceive();
                    }
                    

                }
                catch (ObjectDisposedException e)
                {
                    Debug.Log(e.Message);
                    Debug.Log(e.StackTrace);
                    Debug.Log(e.ObjectName);

                    /*IsConnected = false;
                    functionsToRunInMainThread.Add(() => {
                        hintmessage.GetComponent<hint>().Hint("网络连接中断");
                    });
                    Debug.Log(e.Message);


                    CloseServer();*/
                }//由于回调线程无法与主线程同时修改游戏对象，故加入回调序列供主线程使用

            }, null);
        }



        /// <summary>

        /// 数据转换，网络发送需要两部分数据，一是数据长度，二是主体数据

        /// </summary>

        /// <param name="message"></param>

        /// <returns></returns>

        private  byte[] WriteMessage(byte[] message)

        {

            MemoryStream ms = null;

            using (ms = new MemoryStream())

            {

                ms.Position = 0;

                BinaryWriter writer = new BinaryWriter(ms);

                //ushort msglen = (ushort)message.Length;

                //writer.Write(msglen);

                writer.Write(message);

                writer.Flush();

                return ms.ToArray();

            }

        }

    }

}
