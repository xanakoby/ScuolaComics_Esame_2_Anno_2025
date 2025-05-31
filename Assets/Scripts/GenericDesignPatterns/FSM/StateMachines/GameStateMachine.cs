    using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    public GenericStateMachine<ELevel> StateMachine;
    [SerializeField] ELevel levelGame;
    private void Awake()
    {
        StateMachine = new GenericStateMachine<ELevel>();

        StateMachine.RegisterState(ELevel.Level1, new Level1State(this));
        StateMachine.RegisterState(ELevel.Level2, new Level2State(this));

        SetState(levelGame);
    }
    public void SetState(ELevel newState)
    {
        StateMachine.SetState(newState);
    }
    void Update()
    {
        StateMachine.OnUpdate();
    }
}
