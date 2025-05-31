using DesignPatterns.Generics;
using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Monete del giocatore")]
    [SerializeField] private int startingCoins = 100;
    private int currentCoins;

    [Header("Enemy Vars")]
    public IFactory entitiesFactory { get; private set; }
    public Transform enemySpawnTransform;
    public Path enemyPath;

    [Header("Entities Variants")]
    public EnemyCapsule enemy1;
    public EnemyCapsule enemy2;
    public EnemyCapsule enemy3;

    public int enemyCount = 0;
    public bool allEnemyKilled;

    public TurretCapsule turret1;
    public TurretCapsule turret2;
    public TurretCapsule turret3;

    public int CurrentCoins => currentCoins;

    public override void Awake()
    {
        base.Awake();
        currentCoins = startingCoins;

        entitiesFactory = new Factory();
    }

    public void AddCoins(int amount)
    {
        // TODO: Le monete vengono aggiunte alla distruzione dei nemici o alla rimozione delle torrette. Usare bene i messaggi in maniera intelligente

        currentCoins += amount;
        Debug.Log($"Aggiunti {amount} coins. Coins totali: {currentCoins}");
        // TODO: Aggiungere un evento qui per aggiornare l'UI
    }

    // Metodo per spendere monete
    public bool SpendCoins(int amount)
    {
        if (currentCoins >= amount)
        {
            currentCoins -= amount;
            Debug.Log($"Spesi {amount} coins. Coins rimasti: {currentCoins}");
            // TODO: Aggiungere un evento qui per aggiornare l'UI
            return true;
        }
        else
        {
            Debug.LogWarning("Non hai abbastanza monete!");
            // TODO: Aggiungere un evento qui per aggiornare l'UI
            return false;
        }
    }
    public void SpawnEnemy(EnemyCapsule _enemyCapsule)
    {
        entitiesFactory.Create(_enemyCapsule, enemySpawnTransform.position, Quaternion.identity);
    }
    public void RemoveCountEnemy()
    {
        enemyCount--;
        if(enemyCount <= 0)
        {
            enemyCount = 0;
            allEnemyKilled = true;

            Debug.Log("cambio scena");
        }
    }
    public void SpawnTurret(TurretCapsule _turretCapsule, Vector3 _position, Quaternion _rotation)
    {
        entitiesFactory.Create(_turretCapsule, _position, _rotation);
    }
    public List<Transform> GetPathList()
    {
        return enemyPath.pathPoints;
    }

    public void SellCurrentTurret()
    {
        UIManager.Instance.UnshowTurretStats();
        Publisher.Publish(new SellTurretMessage());
    }
}
