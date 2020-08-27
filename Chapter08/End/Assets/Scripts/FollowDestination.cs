using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowDestination : MonoBehaviour
{
    public Transform Destination = null;
    private NavMeshAgent ThisAgent = null;

    void Awake()
    {
        ThisAgent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        ThisAgent.SetDestination(Destination.position);
    }
}
