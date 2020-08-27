using UnityEngine;

public class AttackState : MonoBehaviour, IFSMState
{
    public FSMStateType StateName { get { return FSMStateType.Attack; } }
    public ParticleSystem WeaponPS = null;

    private Transform ThisPlayer = null;

    void Awake()
    {
        ThisPlayer = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    public void OnEnter()
    {
        WeaponPS.Play();
    }

    public void OnExit()
    {
        WeaponPS.Stop();
    }

    public void DoAction()
    {
        Vector3 Dir = (ThisPlayer.position - transform.position).normalized;
        Dir.y = 0;
        transform.rotation = Quaternion.LookRotation(Dir, Vector3.up);
    }

    public FSMStateType ShouldTransitionToState()
    {
        return FSMStateType.Attack;
    }
}
