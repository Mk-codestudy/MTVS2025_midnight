using UnityEngine;
using UnityEngine.XR;


public class XRKeyManager : MonoBehaviour
{
    public static XRKeyManager Instance { get; private set; }

    private void Awake()
    {
        // 이미 인스턴스가 존재하면 중복 파괴
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 이동해도 유지
    }

    public int currentType = 0;



    void Update()
    {

        switch (currentType)
        {
            case 0:
                if (OVRInput.GetDown(OVRInput.RawButton.A))
                {
                    Debug.Log("A 버튼 눌림");
                }
                break;
        }
        
    }
}
