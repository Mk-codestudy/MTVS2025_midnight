using UnityEngine;
using UnityEngine.AI;
public class DinosaurMove : MonoBehaviour
{
    public DinosaurAnimationScript dinosaurAnimationScript;


    public Transform target;
    private NavMeshAgent agent;
    private Animator dinosaurAnim;
    public DinosaurType type;


    void Start()
    {
        if (dinosaurAnimationScript == null) dinosaurAnimationScript = GetComponent<DinosaurAnimationScript>();

        Animator anim = dinosaurAnimationScript.anim;

        dinosaurAnim = anim;

        agent = GetComponent<NavMeshAgent>();
        if (target != null)
        {
            dinosaurAnim.SetInteger("count", 1);
            agent.SetDestination(target.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("target"))
        {
            target = null;
            agent.isStopped = true;
            dinosaurAnim.SetInteger("count", 0);
        }
    }

}
