using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    public float RunVelocity = 0.1f;
    public string AnimationRunParamName = "Run";
    public string AnimationSpeedParamName = "Speed";

    private NavMeshAgent ThisNavMeshAgent = null;
    private Animator ThisAnimator = null;
    private float MaxSpeed;

    void Awake()
    {
        ThisNavMeshAgent = GetComponent<NavMeshAgent>();
        ThisAnimator = GetComponent<Animator>();
        MaxSpeed = ThisNavMeshAgent.speed;
    }

    void Update()
    {
        ThisAnimator.SetBool(AnimationRunParamName, ThisNavMeshAgent.velocity.magnitude > RunVelocity);
        ThisAnimator.SetFloat(AnimationSpeedParamName, ThisNavMeshAgent.velocity.magnitude / MaxSpeed);
    }
}
