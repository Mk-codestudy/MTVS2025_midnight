using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance { get; private set; }

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
    private Main_UI main_UI;

    public List<GameObject> uiObjects;

    public int number = 0;

    public Button loginButton;
    public Button joinButton;
    public Button joinpostButton;

    private Button currentButton;
    private bool hasSelected = false;


    private void Start()
    {
        SetOnOffUI(0, true);
    }

    private void Update()
    {

        if (number == 0 && OVRInput.GetDown(OVRInput.RawButton.A))
        {
            main_UI = uiObjects[0].GetComponent<Main_UI>();
            main_UI.ManiUI_GetMenu_btn();
        }
        else if (number == 1 && OVRInput.GetDown(OVRInput.RawButton.A))
        {
            main_UI.Go_to_List_GetMenu_btn();
        }
        else if (number == 2)
        {
            SelectLoginOrJoin();
        }
        else if (number == 3)
        {
            CurrentButtonChange(joinpostButton);
            CurrentButtonClick(joinpostButton);
        }
        else if (number == 4)
        {

        }
    }

    public void SelectLoginOrJoin()
    {
        Vector2 rightStick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        if (!hasSelected)
        {
            if (rightStick.x < -0.5f)
            {
                CurrentButtonChange(loginButton);
                hasSelected = true;
            }
            else if (rightStick.x > 0.5f)
            {
                CurrentButtonChange(joinButton);
                hasSelected = true;
            }
        }

        // 조이스틱이 중립으로 돌아오면 다시 선택 가능
        if (rightStick.magnitude < 0.2f)
        {
            hasSelected = false;
        }

        CurrentButtonClick(currentButton);
    }


    public void CurrentButtonChange(Button button)
    {
        button.Select();
        currentButton = button;
    }

    public void CurrentButtonClick(Button button)
    {
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            if (currentButton == button)
            {
                button.onClick.Invoke(); // 버튼 클릭 이벤트 실행
            }
        }
    }





    public void SetOnOffUI(int num, bool open)
    {
        uiObjects[num].SetActive(open);
    }

    public void MoveScene(int num)
    {
        SceneManager.LoadScene(num);
    }
}
