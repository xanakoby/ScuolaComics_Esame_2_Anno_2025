using UnityEngine;

public class Level2State : State
{
    public Level2State(GameStateMachine gameStateMachine)
    {
        _owner = gameStateMachine;
    }

    public GameStateMachine _owner { get; }

    public override void OnCollisionEnter()
    {
        throw new System.NotImplementedException();
    }

    public override void OnCollisionExit()
    {
        throw new System.NotImplementedException();
    }

    public override void OnEnd()
    {
        throw new System.NotImplementedException();
    }

    public override void OnFixedUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void OnStart()
    {
        Debug.Log("Sto entrando in Level2");
    }

    public override void OnTriggerEnter()
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerExit()
    {
        throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        throw new System.NotImplementedException();
    }
}
