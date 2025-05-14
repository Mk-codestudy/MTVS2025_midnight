using DG.Tweening;
using Meta.WitAi.Lib;
using UnityEngine;
using UnityEngine.Audio;
using System.IO;

public class Dinoinfo_UI : MonoBehaviour
{
    [Header("이거 붙은캔버스")]
    public CanvasGroup canvasGroup;
    
    [Header("페이드 인 속도 : 추천 2.5")]
    [Range(0.1f, 3f)]
    public float fadeDuration = 1f;

    [Header("공룡 설명 UI 리스트")]
    public GameObject[] dinoInfoList;

    [Header("음성을 듣고 있다는 표식")]
    public GameObject micUi;

    [Header("통신 대기 UI")]
    public GameObject convertingUi;

    [Header("챗봇 대화 로그")]
    public GameObject chatLogUi;

    //음성 녹음 관련
    RecordSoundCenter soundCenter;
    AIChatLogSponer chatLogSponer;


    public void Fadein() //이 UI 활성화할 때마다 페이드 인으로 멋있게 들어오도록 하는 함수. activeself false인 상태에서 이 함수 실행하면 페이드인으로 켜짐
    {
        // 시작 시 완전히 투명하게 설정
        canvasGroup.alpha = 0f;
        gameObject.SetActive(true);
        // 페이드 인 실행
        canvasGroup.DOFade(1f, fadeDuration);
    }

    private void Awake()
    {
        soundCenter = GetComponent<RecordSoundCenter>();
        chatLogSponer = GetComponent<AIChatLogSponer>();
    }

    void Update()
    {
        if (gameObject.activeSelf) //이 UI가 활성되어 있을 때만
        {
            PressRecord(); //녹음 관련 로직
        }

        if (HTTPMnanger.htpmg.isAiRepeated) // AI 챗봇이 대답 생성을 마쳤을 때 
        {
            HTTPMnanger.htpmg.isAiRepeated = false;
            // 여기에다가 AI 챗봇 편집하기
            chatLogSponer.MakeChatLog(); //업데이트하고
            //UI 창 바꿔주기
            convertingUi.SetActive(false);
            chatLogUi.SetActive(true);

            //챗봇 음성 틀어주기
            //soundCenter.audios.Play();
        }

    }

    void PressRecord()
    {
        // 중지 키를 누르면 녹음이다. 우선 개발 중인 지금은 R 키를 눌러서 녹음한다. 알아서 바꿔주씨오
        if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger)) // 오른쪽 중간 트리거 누르기 시작 //R 키를 누르면 녹음시작
        //if (Input.GetKeyDown(KeyCode.R)) //R 키를 누르면 녹음시작
        {
            //UI 조작
            for (int i = 0; i < dinoInfoList.Length; i++) //다이노 인포리스트는 전부 끄기
            {
                dinoInfoList[i].gameObject.SetActive(false);
            }
            chatLogUi.SetActive(false );
            micUi.SetActive(true); //마이크 켜기

            //녹음 로직

            soundCenter.StartRecording();

        }
        else if (OVRInput.GetUp(OVRInput.RawButton.RHandTrigger)) // 오른쪽 중간 트리거로 녹음 종료
        {
            // UI 조작
            micUi.SetActive(false);
            convertingUi.SetActive(true); // 로딩중!!!

            // 녹음 로직
            byte[] recordedSound = soundCenter.StopRecording();

            // 플랫폼에 따라 저장 경로 다르게 설정
#if UNITY_ANDROID && !UNITY_EDITOR
    string filePath = Path.Combine(Application.persistentDataPath, "recorded.wav");
#else
            string filePath = "C:/Users/mana9/Documents/recorded.wav";
#endif

            // 파일 저장
            File.WriteAllBytes(filePath, recordedSound);
            Debug.Log($"🎙 WAV 저장 경로: {filePath}");

            // 녹음 완료 후 통신 전송
            HTTPMnanger.htpmg.MakeClass(recordedSound).Forget();
            Debug.Log("녹음 파일 캐치완료! AI 전송중. . .");
        }
    }
}
