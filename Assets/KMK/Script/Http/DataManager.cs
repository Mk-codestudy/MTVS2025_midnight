using System;
using OVR.OpenVR;
using UnityEngine;

// 회원가입
[Serializable]
public struct UserRequest
{
    public string id;
    public string password;
    public string name;
}

[Serializable]
public struct UserID
{
    public int userId;
}

//로그인
[Serializable]
public struct UserLogin
{
    public string id;
    public string password;
}

[Serializable]
public struct UserLiginResponse
{
    public int userid;
    public string name;
}

//챗봇 요청
[Serializable]
public struct PostQuest
{
    public int userid;
    public string requesttime;
    public int dinoid;
    public string voice;
}

[Serializable]
public struct ResponseAI
{
    public int userid;
    public int dinoid;
    public string responsetime;
    public string response;
    public string rseponseTTS;
}



public class DataManager : MonoBehaviour
{
    public UserRequest request;
    public UserID userId;
}
