using UnityEngine;

public class DinosaurAnimationScript : MonoBehaviour
{
    public Animator anim;


    void Awake()
    {
        anim = GetComponent<Animator>();
    }
}
