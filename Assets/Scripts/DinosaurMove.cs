using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class DinosaurMove : MonoBehaviour
{
    public DinosaurAnimationScript dinosaurAnimationScript;

    public Transform target;
    private NavMeshAgent agent;
    private int current_num;

    public VRTestScript vrTestScript;

    private void Awake()
    {
        vrTestScript = GameObject.FindFirstObjectByType<VRTestScript>();
    }


    public void StartMove(int num)
    {
        if (dinosaurAnimationScript == null) dinosaurAnimationScript = GetComponent<DinosaurAnimationScript>();

        current_num = num;
        agent = GetComponent<NavMeshAgent>();

        GameObject targetObject = GameObject.FindWithTag($"target_{num}");

        if (targetObject != null)
            target = targetObject.transform;

        StartCoroutine(WaitUntilOnNavMeshAndMove());
    }


    IEnumerator WaitUntilOnNavMeshAndMove()
    {
        // NavMesh에 올라갈 때까지 대기
        while (!agent.isOnNavMesh)
        {
            yield return null;
        }

        // 이제 SetDestination 호출 가능
        if (target != null)
        {
            dinosaurAnimationScript.SetAnimation(1);
            agent.SetDestination(target.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag($"target_{current_num}"))
        {
            target = null;
            agent.isStopped = true;
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            dinosaurAnimationScript.SetAnimation(2);
            if (vrTestScript != null)
            {
                vrTestScript.isTurnScene = true;
            }
        }
    }

}
