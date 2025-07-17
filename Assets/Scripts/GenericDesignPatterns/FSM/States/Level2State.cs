using System.Collections;
using UnityEngine;

public class Level2State : State
{
    Coroutine startGame;
    bool allEnemyDead;
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
        UIManager.Instance.startGame.onClick.RemoveListener(StartGame);
        _owner.StopCoroutine(startGame);
        GameManager.Instance.allEnemyKilled = false;
    }

    public override void OnFixedUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void OnStart()
    {
        Debug.Log("Sto entrando in Level2");
        GameManager.Instance.allEnemyKilled = false;
        StartGame();
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
        if (allEnemyDead)
        {
            //fine level 2
        }
    }
    public void StartGame()
    {
        startGame = _owner.StartCoroutine(StartLevel1());
    }

    private IEnumerator StartLevel1()
    {
        GameManager.Instance.enemyCount = 10;
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(1.5f);
            GameManager.Instance.SpawnEnemy(GameManager.Instance.enemy1);
        }
        yield return new WaitForSeconds(3f);
        //ne piazzo altri 3 e controllo se sono morti tutti
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(1f);
            GameManager.Instance.SpawnEnemy(GameManager.Instance.enemy1);
        }
        yield return new WaitForSeconds(2f);
        //ne piazzo altri 3 e controllo se sono morti tutti
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(3f);
            GameManager.Instance.SpawnEnemy(GameManager.Instance.enemy1);
        }
        yield break;
    }
}
