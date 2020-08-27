using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    public float RunVelocity = 0.1f;
    public string AnimationRunParamName = "Run";

    private NavMeshAgent ThisNavMeshAgent;
    private Animator ThisAnimator;

    void Awake()
    {
        ThisNavMeshAgent = GetComponent<NavMeshAgent>();
        ThisAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        ThisAnimator.SetBool(AnimationRunParamName, ThisNavMeshAgent.velocity.magnitude > RunVelocity);
    }
}
