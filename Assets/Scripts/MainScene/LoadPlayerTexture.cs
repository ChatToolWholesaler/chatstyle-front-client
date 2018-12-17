using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadPlayerTexture : MonoBehaviour {
    // Use this for initialization

    [HideInInspector] public bool isControlledByUser = false;
    const int albedoWidth = 512;
    const int albedoHeight = 512;
    public Texture2D defaultAlbedo;
    public Texture2D officialPreset0, officialPreset1, officialPreset2, officialPreset3;
    public int inUseOfficialPresetIndex;

    void Start()
    {
        if (gameObject.name == "role") { isControlledByUser = true; }

        if (isControlledByUser)
        {
            //依据本地存储的纹理更新模型
            //创建文件读取流
            string rootPath = Application.persistentDataPath + "/Presets";

            if (!PlayerPrefs.HasKey("inUsePresetIndex"))
            {
                //PlayerPrefs.SetInt("inUsePresetIndex", 0);//【】缺乏新手引导，先这么写，因为如果key为空说明本地没有存储任何皮肤
                //更新材质
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", defaultAlbedo);
            }
            else
            {
                Texture2D texture = new Texture2D(albedoWidth, albedoHeight);
                FileStream fileStream = new FileStream(rootPath + "/" + PlayerPrefs.GetInt("inUsePresetIndex") + "/albedo.png", FileMode.Open, FileAccess.Read);
                fileStream.Seek(0, SeekOrigin.Begin);
                //创建文件长度缓冲区
                byte[] bytes = new byte[fileStream.Length];
                //读取文件
                fileStream.Read(bytes, 0, (int)fileStream.Length);
                //释放文件读取流
                fileStream.Close();
                fileStream.Dispose();
                fileStream = null;
                //创建Texture
                texture.LoadImage(bytes);

                //更新材质
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", texture);
            }
        }
        else
        {
            //int inUseOfficialPresetIndex = 0;//【】
            switch (inUseOfficialPresetIndex)
            {
                case 0:
                    //更新材质
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", officialPreset0); break;
                case 1:
                    //更新材质
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", officialPreset1); break;
                case 2:
                    //更新材质
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", officialPreset2); break;
                case 3:
                    //更新材质
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", officialPreset3); break;
            }
        }
    }
}
