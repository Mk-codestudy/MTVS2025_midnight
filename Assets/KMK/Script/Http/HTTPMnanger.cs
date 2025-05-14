using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;


public class HTTPMnanger : MonoBehaviour
{
    public static HTTPMnanger htpmg;
    public string url; //통신 링크

    public string aiurl; //AI통신 링크

    public bool loginSuccess;
    public bool isAiRepeated;

    [Header("테스트용 클래스")]
    public UserRequest user;
    //public AudioClip voice;


    private void Awake()
    {
        if (htpmg == null) htpmg = this;
        else Destroy(gameObject);
        //싱글턴

        DontDestroyOnLoad(gameObject);
        Debug.Log("DontDestroyOnLoad상에 HttpManager 생성 :: 백엔드 통신관리 및 통신정보 저장소");
    }

    #region 회원가입

    // 회원가입
    public async UniTaskVoid PostJoin(UserRequest userRequest)
    {
        string urls = url + "/userjoin";
        Debug.Log("PostJoin 설정!" + urls);
        UserID? response = await Post_Join_async(urls, userRequest);
        if (response.HasValue)
        {
            DataManager.datamg.userId = response.Value;
            Debug.Log($"UserId: {response.Value.userId}");
        }
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

    #endregion

    #region 로그인
    // 로그인
    public async UniTask<bool> PostLogin(UserLogin userLogin)
    {
        string urls = url + "/userlogin";
        Debug.Log("PostJoin 설정!" + urls);
        UserLoginResponse? response = await Post_Login_async(urls, userLogin);
        if (response.HasValue)
        {
            Debug.Log($"Login_info: {response}");
            loginSuccess = true;
            return true;
        }
        else
        {
            Debug.Log("응답이 없습니다.");
            return false;
        }
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
    #endregion

    //임시로 오디오 파일 캐싱해서 클래스 하나 만들기

    public async UniTaskVoid MakeClass(byte[] wavBytes)
    {
        //byte[] wavBytes = File.ReadAllBytes(@"C:\Users\sapph\Downloads\voice.wav"); //테스트용 보이스 파일을 쓰는 중

        PostQuest postData = new PostQuest
        {
            userid = 0,
            requesttime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm"),
            dinoid = 0,
            voice = wavBytes //나중에 수정하기(미리 녹음된 걸로)
        };

        string urls = aiurl + "/ask-dino/";
        print(urls);

        ResponseAI? response = await Post_QuestToAI(urls, postData);

        if (response.HasValue)
        {
            DataManager.datamg.responseAI = response.Value;
            Debug.Log($"ResponseAI: {response.Value}");
            //챗봇 스택추가하기
            isAiRepeated = true;
        }
        else
        {
            Debug.Log("응답이 없습니다.");
        }
    }

    //음성 질문하기
    async UniTask<ResponseAI?> Post_QuestToAI(string urls, PostQuest postQuest)
    {
        //string json = JsonUtility.ToJson(postQuest);
        WWWForm form = new WWWForm();

        form.AddField("userid", postQuest.userid);
        form.AddField("requesttime", postQuest.requesttime);
        form.AddField("dinoid", postQuest.dinoid);
        // 파일 추가 (필드명, 데이터, 파일명, MIME 타입)
        form.AddBinaryData("voice", postQuest.voice, "voice.wav", "audio/wav");


        using (UnityWebRequest request = UnityWebRequest.Post(urls, form))
        {
            // 필요시 헤더 추가
            // request.SetRequestHeader("Authorization", "Bearer ...");

            await request.SendWebRequest().ToUniTask();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Success: " + request.downloadHandler.text);

                // 3. 멀티파트 응답 파싱
                return ParseMultipartResponse(request);

            }
            else
            {
                Debug.LogError("Error: " + request.error);
                return null;
            }
        }
    }

    private ResponseAI? ParseMultipartResponse(UnityWebRequest request)
    {
        // 1. boundary 추출
        string contentType = request.GetResponseHeader("Content-Type");
        if (string.IsNullOrEmpty(contentType)) return null;

        // boundary=로 시작하는 부분 찾기
        string boundary = null;
        var tokens = contentType.Split(';');
        foreach (var token in tokens)
        {
            var trimmed = token.Trim();
            if (trimmed.StartsWith("boundary="))
            {
                boundary = "--" + trimmed.Substring("boundary=".Length);
                break;
            }
        }
        if (boundary == null) return null;

        // 2. 응답 데이터 파싱
        byte[] data = request.downloadHandler.data;
        string responseStr = System.Text.Encoding.UTF8.GetString(data);

        // 3. 파트 분리
        var parts = responseStr.Split(new string[] { boundary }, StringSplitOptions.RemoveEmptyEntries);

        // 4. 각 파트에서 데이터 추출
        ResponseAI result = new ResponseAI();
        foreach (var part in parts)
        {
            int headerEnd = part.IndexOf("\r\n\r\n");
            if (headerEnd < 0) continue;
            string header = part.Substring(0, headerEnd);
            string body = part.Substring(headerEnd + 4);

            if (header.Contains("name=\"userid\""))
                int.TryParse(body.Trim(), out result.userid);
            else if (header.Contains("name=\"dinoid\""))
                int.TryParse(body.Trim(), out result.dinoid);
            else if (header.Contains("name=\"responsetime\""))
                result.responsetime = body.Trim();
            else if (header.Contains("name=\"request\""))
                result.request = body.Trim();
            else if (header.Contains("name=\"response\""))
                result.response = body.Trim();
            else if (header.Contains("name=\"rseponseTTS\""))
            {
                // 파일 파트는 바이너리로 추출해야 함
                // (아래는 간단한 예시, 실제로는 바이트 오프셋 계산 필요)
                // body의 앞뒤 \r\n 제거
                var bodyBytes = System.Text.Encoding.UTF8.GetBytes(body.Trim());
                result.rseponseTTS = bodyBytes;
            }
        }
        return result;
    }

}


