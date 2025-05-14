using UnityEngine;

public class VRTestScript : MonoBehaviour
{
    public bool isTurnScene = false;

    // Update is called once per frame
    void Update()
    {
        if (isTurnScene)
        {
            if (OVRInput.GetDown(OVRInput.RawButton.B))
            {
                GameManager.Instance.MoveScene("MRScene");
            }
        }
    }
}
