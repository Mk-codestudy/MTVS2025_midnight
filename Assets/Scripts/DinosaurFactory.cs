using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DinosaurFactory : MonoBehaviour
{

    public List<GameObject> dinosaurPrefab;

    public Transform startpoint;

    public void InstanteDinosaurEvent(int num)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(startpoint.position, out hit, 2.0f, NavMesh.AllAreas))
        {
            // hit.position이 NavMesh에 가까운 유효 위치
            GameObject dinosaur = Instantiate(dinosaurPrefab[num], hit.position, Quaternion.identity);
            dinosaur.transform.SetParent(null);
            dinosaur.transform.position = hit.position;
            DinosaurMove dinosaurMove = dinosaur.GetComponent<DinosaurMove>();
            dinosaurMove.StartMove(num);
        }
        else
        {
            Debug.LogError("NavMesh 근처에서 적절한 위치를 찾을 수 없습니다.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            InstanteDinosaurEvent(0);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            InstanteDinosaurEvent(1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            InstanteDinosaurEvent(2);
        }
    }

}
