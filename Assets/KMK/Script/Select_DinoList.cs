using UnityEngine;

public class Select_DinoList : MonoBehaviour
{
    float currentTime = 0;

    [Header("칸 이동 딜레이 시간")]
    [Range(0.0f, 0.7f)]
    public float delayTime = 0.2f;

    public GameObject[] dinoList;
    public int dinoListnum;
    bool isSwapped;

    public GameObject setactive;

    void Start()
    {
        isSwapped = true;
    }

    void Update()
    {
        if (setactive.activeSelf)
        {
            SwapNum();
            Twinkle();
        }
    }

    void SwapNum()
    {
        // 🎯 오른손 조이스틱 X축
        float x = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x;

        if (isSwapped)
        {
            currentTime += Time.deltaTime;
            if (currentTime > delayTime)
            {
                isSwapped = false;
                currentTime = 0;
            }
        }

        if (!isSwapped && Mathf.Abs(x) > 0.5f) // 슬라이드 감지
        {
            isSwapped = true;

            if (x > 0)
            {
                // 오른쪽으로 이동
                dinoListnum = (dinoListnum + 1) % dinoList.Length;
            }
            else
            {
                // 왼쪽으로 이동
                dinoListnum = (dinoListnum - 1 + dinoList.Length) % dinoList.Length;
            }
        }
    }

    void Twinkle()
    {
        for (int i = 0; i < dinoList.Length; i++)
        {
            dinoList[i].SetActive(i == dinoListnum);
        }
    }

}
