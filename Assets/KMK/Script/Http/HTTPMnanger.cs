using System;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;


public class HTTPMnanger : MonoBehaviour
{
    public static HTTPMnanger htpmg;
    public string url; //통신 링크

    [Header("테스트용 클래스")]
    public UserRequest user;
    UserLogin userLogin;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PostJoin().Forget();
        }
    }


    // 회원가입
    public async UniTaskVoid PostJoin()
    {
        string urls = url + "/userjoin";
        Debug.Log("PostJoin 설정!" + urls);
        UserID? response = await Post_Join_async(urls, user);
        if (response.HasValue)
            Debug.Log($"UserId: {response.Value.userId}");
        else
            Debug.Log("응답이 없습니다.");
    }

    public async UniTask<UserID?> Post_Join_async(string url, UserRequest user)
    {
        string json = JsonUtility.ToJson(user);
        Debug.Log("Json : " + json);
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);

        using var request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        try
        {
            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                return null;
            }
            // JSON 응답 역직렬화
            return JsonUtility.FromJson<UserID>(request.downloadHandler.text);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            return null;
        }
    }


    // 로그인
    public async UniTaskVoid PostLogin()
    {
        string urls = url + "/userlogin";
        Debug.Log("PostJoin 설정!" + urls);
        UserLoginResponse? response = await Post_Login_async(urls, userLogin);
        if (response.HasValue)
            Debug.Log($"Login_info: {response}");
        else
            Debug.Log("응답이 없습니다.");
    }


    public async UniTask<UserLoginResponse?> Post_Login_async(string url, UserLogin login)
    {
        string json = JsonUtility.ToJson(login);
        Debug.Log("Json : " + json);
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);

        using var request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        try
        {
            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                return null;
            }
            // JSON 응답 역직렬화
            return JsonUtility.FromJson<UserLoginResponse>(request.downloadHandler.text);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            return null;
        }
    }



}
