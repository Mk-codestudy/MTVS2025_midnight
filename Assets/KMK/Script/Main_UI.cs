using System;
using UnityEngine;

public class Main_UI : MonoBehaviour
{
    // A버튼을 누르면 앞으로
    // B버튼을 누르면 뒤로

    public int menuNum;

    public GameObject[] contentUI;

    void Start()
    {
        menuNum = 0;
    }

    void Update()
    {
        SwapKey();
        Twinkle();

    }

    void SwapKey()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (menuNum < (contentUI.Length - 1))
            {
                menuNum++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            if (menuNum > 0)
            {
                menuNum--;
            }
        }
    }

    void Twinkle() //아웃라인 리스트의 dinoListnum번째 게임오브젝트만 활성화하는 함수
    {
        for (int i = 0; i < contentUI.Length; i++)
        {
            if (i == menuNum)
            {
                contentUI[i].SetActive(true);
            }
            else
            {
                contentUI[i].SetActive(false);
            }
        }
    }

}
