using UnityEngine;

public class Select_DinoList : MonoBehaviour
{
    //좌우키로 움직일때마다 List Swap

    float currentTime = 0;

    [Header("칸 이동 딜레이 시간")]
    [Range(0.0f, 0.7f)]
    public float delayTime = 0.2f;

    public GameObject[] dinoList;
    public int dinoListnum;
    bool isSwapped;


    void Start()
    {
        
    }

    void Update()
    {
        SwapNum();
        Twinkle();
    }

    void SwapNum()
    {
        float x = Input.GetAxis("Horizontal");
        print(x);

        if (isSwapped) //스왑 한번 하면
        {
            currentTime += Time.deltaTime; //0.3초만큼 기다리
            if (currentTime > delayTime) //기다리고 나면 리셋
            {
                isSwapped = false;
                currentTime = 0;
            }
        }

        if ( !isSwapped && x != 0)
        {
            if (x > 0) // 오른쪽으로 갔을 때
            {
                isSwapped = true;

                if (dinoListnum >= 2)
                {
                    dinoListnum = 0;
                }
                else
                {
                    dinoListnum++;
                }
            }

            if (x < 0) // 왼쪽으로 갔을 때
            {
                isSwapped = true;
                if (dinoListnum <= 0)
                {
                    dinoListnum = 2;
                }
                else
                {
                    dinoListnum--;
                }
            }
        }
    }

    void Twinkle() //아웃라인 리스트의 dinoListnum번째 게임오브젝트만 활성화하는 함수
    {
        for (int i = 0; i < dinoList.Length; i++)
        {
            if (i == dinoListnum)
            {
                dinoList[i].SetActive(true);
            }
            else
            {
                dinoList[i].SetActive (false);
            }
        }
    }


}
