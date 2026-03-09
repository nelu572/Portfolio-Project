using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    
    [SerializeField] private float detectRange = 15f;
    [SerializeField] private LayerMask obstacleLayer;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Init(Transform playerTransform)
    {
        player = playerTransform;
    }

    void Update()
    {
        if (player == null)
        {
            Debug.Log("플레이어 없음");
            return;
        }
        if (CanSeePlayer())
        {
            agent.SetDestination(player.position);
        }
    }

    public bool CanSeePlayer()
    {
        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 dir = (player.position - origin);
        float distance = dir.magnitude;

        // 거리 체크
        if (distance > detectRange)
        {
            return false;
        }
        dir.Normalize();

        // raycast 체크 (벽 가림 여부)
        if (Physics.Raycast(origin, dir, distance, obstacleLayer))
        {
            return false; // 벽에 막힘
        }

        return true;
    }
}