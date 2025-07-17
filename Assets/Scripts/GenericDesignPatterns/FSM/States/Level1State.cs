using System.Collections;
using UnityEngine;

public class Level1State : State
{
    Coroutine startGame;
    bool allEnemyDead;
    public Level1State(GameStateMachine gameStateMachine)
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
        Debug.Log("Sto entrando in Level1");

        GameManager.Instance.allEnemyKilled = false;

        UIManager.Instance.startGame.onClick.AddListener(StartGame);
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
        if (GameManager.Instance.allEnemyKilled)
        {
            //LevelManager.Instance.ChangeScene("GameScene2");
            _owner.SetState(ELevel.Level2);
        }
    }

    public void StartGame()
    {
        startGame = _owner.StartCoroutine(StartLevel1());
    }

    private IEnumerator StartLevel1()
    {
        GameManager.Instance.enemyCount = 7;
        for(int i =0; i<4; i++)
        {
            yield return new WaitForSeconds(1f);
            GameManager.Instance.SpawnEnemy(GameManager.Instance.enemy1);
        }
        yield return new WaitForSeconds(2f);
        //ne piazzo altri 3 e controllo se sono morti tutti
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(2f);
            GameManager.Instance.SpawnEnemy(GameManager.Instance.enemy1);
        }
        yield break;
    }
}
