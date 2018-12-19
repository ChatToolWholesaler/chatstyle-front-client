using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequestItemControl : MonoBehaviour {

    public string nickname = "卿1";
    public string id = "1234";
    public string sex = "♂";
    public string sign = "千里之行，始于足下。";

    public void setup(string nickname, string id, string sex, string sign)
    {
        this.nickname = nickname;
        this.id = id;
        this.sex = sex;
        this.sign = sign;
        GetComponentInChildren<Text>().text = nickname;
    }
}
