using UnityEngine;

public class FSM : MonoBehaviour
{
    public FSMStateType StartState = FSMStateType.Patrol;

    private IFSMState[] StatePool;
    private IFSMState CurrentState;
    private readonly IFSMState EmptyAction = new EmptyState();

    void Awake()
    {
        StatePool = GetComponents<IFSMState>();
    }

    void Start()
    {
        CurrentState = EmptyAction;
        TransitionToState(StartState);
    }

    void Update()
    {
        CurrentState.DoAction();

        FSMStateType TransitionState = CurrentState.ShouldTransitionToState();

        if (TransitionState != CurrentState.StateName)
        {
            TransitionToState(TransitionState);
        }
    }

    private void TransitionToState(FSMStateType StateName)
    {
        CurrentState.OnExit();
        CurrentState = GetState(StateName);
        CurrentState.OnEnter();

        Debug.Log("Transitioned to " + CurrentState.StateName);
    }

    IFSMState GetState(FSMStateType StateName)
    {
        foreach (var state in StatePool)
        {
            if (state.StateName == StateName)
            {
                return state;
            }
        }

        return EmptyAction;
    }
}
