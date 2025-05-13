using System;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;


public class HTTPMnanger : MonoBehaviour
{
    public static HTTPMnanger htpmg;
    public string url; //통신 링크

    public UserRequest user;


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


    // 사용 예시
    public async UniTaskVoid PostJoin()
    {
        string urls = url + "/request";
        UserID? response = await Post_Join_async(url, user);
        if (response.HasValue)
            Debug.Log($"UserId: {response.Value.userId}");
        else
            Debug.Log("응답이 없습니다.");
    }


    public async UniTask<UserID?> Post_Join_async(string url, UserRequest user)
    {
        string json = JsonUtility.ToJson(user);
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



}
