using UnityEngine;
using UnityEngine.UI;

public class AIChatLogSponer : MonoBehaviour
{
    // 챗봇 채팅로그 만들기

    [Header("프리펩")]
    public GameObject humanChat;
    public GameObject aiChat;

    [Header("content")]
    public GameObject content;



    public void MakeChatLog()
    {
        //인간 채팅로그 먼저 만들고
        GameObject human = Instantiate(humanChat, content.transform);
        ChatTextEditor edit = human.GetComponent<ChatTextEditor>();
        edit.EditChat(DataManager.datamg.responseAI.request); //인간의 질문 텍스트화


        //AI 답변 만들고
        GameObject ai = Instantiate(aiChat, content.transform);
        ChatTextEditor edit2 = ai.GetComponent<ChatTextEditor>();
        edit2.EditChat(DataManager.datamg.responseAI.response); //챗봇의 대답으로 바꾸기

        LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>()); //크기 갱신하기

    }

}
