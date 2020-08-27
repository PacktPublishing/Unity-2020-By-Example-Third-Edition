public interface IFSMState
{
    FSMStateType StateName { get; }

    void OnEnter();
    void OnExit();
    void DoAction();
    FSMStateType ShouldTransitionToState();
}