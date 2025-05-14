using TMPro;
using UnityEngine;

public class ChatTextEditor : MonoBehaviour
{

    public TMP_Text tmp;


    public void EditChat(string chat)
    {
        tmp.text = chat;
    }

}
