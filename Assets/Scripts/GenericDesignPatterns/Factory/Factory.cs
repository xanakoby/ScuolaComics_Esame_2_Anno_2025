using UnityEngine;

public class Factory : IFactory
{
    ObjectPooler<EnemyController> enemyPooler;
    ObjectPooler<TurretController> turretPooler;

    public ISpawnable Create(EnemyCapsule _EnemyCapsule, Vector3 spawnPosition, Quaternion rotation)
    {
        try
        {
            if (enemyPooler == null)
            {
                enemyPooler = new ObjectPooler<EnemyController>(_EnemyCapsule.enemyPrefab);
            }
            EnemyController spawnedItem = enemyPooler.Get();

            //qui è dove setto l'oggetto dall'object pool
            spawnedItem.transform.SetPositionAndRotation(spawnPosition, rotation);

            if (!spawnedItem.gameObject.activeSelf)
            {
                spawnedItem.gameObject.SetActive(true);
            }
            else
            {
                spawnedItem.onDestroyTrigger += () =>
                {
                    enemyPooler.Set(spawnedItem);
                };
            }

            if (spawnedItem == null || spawnedItem is not ISpawnable) //  typeof(newFoodBase.GetType()) != IFood
            {
                Debug.LogError($"Errore nella Factory. Nessun prefab creato per il FoodBase {_EnemyCapsule.Name}");
                return default;
            }

            spawnedItem.Initialize(_EnemyCapsule);

            return spawnedItem;
        }
        catch
        {
            return default;
        }
    }

    public ISpawnable Create(TurretCapsule _turretCapsule, Vector3 spawnPosition, Quaternion rotation)
    {
        try
        {
            if (turretPooler == null)
            {
                turretPooler = new ObjectPooler<TurretController>(_turretCapsule.turretPrefab);
            }
            TurretController spawnedItem = turretPooler.Get();

            //qui è dove setto l'oggetto dall'object pool
            spawnedItem.transform.SetPositionAndRotation(spawnPosition, rotation);

            if (!spawnedItem.gameObject.activeSelf)
            {
                spawnedItem.gameObject.SetActive(true);
            }
            else
            {
                spawnedItem.onDestroyTrigger += () =>
                {
                    turretPooler.Set(spawnedItem);
                };
            }

            if (spawnedItem == null || spawnedItem is not ISpawnable) //  typeof(newFoodBase.GetType()) != IFood
            {
                Debug.LogError($"Errore nella Factory. Nessun prefab creato per il FoodBase {_turretCapsule.Name}");
                return default;
            }

            spawnedItem.Initialize(_turretCapsule);
            //lo setto figlio di chi voglio

            return spawnedItem;
        }
        catch
        {
            return default;
        }
    }
}
