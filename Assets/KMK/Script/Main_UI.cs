using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class Main_UI : MonoBehaviour
{
    // A버튼을 누르면 앞으로
    // B버튼을 누르면 뒤로

    public int menuNum;
    public GameObject[] contentUI;

    public TMP_InputField[] loginFields;
    public TMP_InputField[] joinFields;

    void Start()
    {
        //menuNum = 0;
        contentUI[0].SetActive(true);
    }


    void Update()
    {
        //if (contentUI[0].activeSelf && Input.GetKeyDown(KeyCode.A)) // 첫 번째 메인화면 넘기기 (메인 로고 떠있으면서 a키를 누르면...)
        //{
        //    contentUI[0].SetActive(false);
        //    contentUI[1].SetActive(true);
        //}

        //if (contentUI[3].activeSelf && Input.GetKeyDown(KeyCode.A))
        //{
        //    contentUI[3].SetActive(false);
        //    contentUI[4].SetActive(true);
        //}
    }

    //로그인 버튼
    public async void Login_btn() //로그인은 contentUI 1번
    {
        //인풋필드의 텍스트들 클래스에 밀어넣고
        UserLogin userLogin = new UserLogin();
        userLogin.id = loginFields[0].text;
        print("userLogin.id :: " + userLogin.id);
        print("loginFields[0] :: " + loginFields[0].text);
        userLogin.password = loginFields[1].text;

        //데이터매니저에 저장하고
        DataManager.datamg.userLogin = userLogin;

        // 비동기 로그인 요청
        bool loginSuccess = await HTTPMnanger.htpmg.PostLogin(userLogin);

        if (loginSuccess)
        {
            // 로딩 표시 띄우면 좋긴할텐데
            contentUI[1].SetActive(false);
            contentUI[3].SetActive(true);
        }
        else
        {
            Debug.LogError("로그인 통신 실패!!!!!!!!!");
        }
    }

    // intro -> list
    public void Go_to_List_GetMenu_btn()
    {
        contentUI[3].SetActive(false);
        contentUI[4].SetActive(true);
        UIManager.Instance.number = 4;
    }

    //메인화면 넘기기 버튼
    public void ManiUI_GetMenu_btn()
    {
        contentUI[0].SetActive(false); //첫 번째 메인화면 넘기기
        contentUI[1].SetActive(true); //회원가입 켠다
        UIManager.Instance.number = 2;
    }

    //회원가입 버튼
    public void Join_GetMenu_btn()
    {
        UIManager.Instance.number = 3;
        contentUI[1].SetActive(false); //로그인 끄고
        contentUI[2].SetActive(true); //회원가입 켠다
    }

    public void Join_Post_btn()
    {
        UserRequest join = new UserRequest();
        join.id = joinFields[0].text; //Id
        join.password = joinFields[1].text; //패스워드
        join.name = joinFields[2].text; //이름

        DataManager.datamg.request = join; //데이터 매니저에 저장하기
        HTTPMnanger.htpmg.PostJoin(join).Forget(); //통신에 요청보내기

        //귀찮으니까 통신이 잘됐던 안됐던 걍 넘어갑시다
        contentUI[2].SetActive(false);
        contentUI[3].SetActive(true);
        UIManager.Instance.number = 1;
    }


    public void GoToIntro_btn()
    {
        print("버튼 클릭 1");

        contentUI[2].SetActive(false);
        contentUI[3].SetActive(true);
        UIManager.Instance.number = 1;
    }


}
