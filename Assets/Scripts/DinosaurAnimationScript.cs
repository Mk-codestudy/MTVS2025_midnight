using UnityEngine;

public class DinosaurAnimationScript : MonoBehaviour
{
    public Animator anim;


    void Awake()
    {
        anim = GetComponent<Animator>();
    }


    public void SetAnimation(int num)
    {
        anim.SetInteger("count", num);
    }
}
