using UnityEngine;
using static Oculus.Interaction.Samples.MRPassthrough;


public class PassthroughTest : MonoBehaviour
{
    public OVRPassthroughLayer passthroughLayer;
    public GameObject passthrough_gameobject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 또는 버튼 입력
        {
            passthrough_gameobject.SetActive(!passthrough_gameobject.activeSelf);
        }
    }

}
