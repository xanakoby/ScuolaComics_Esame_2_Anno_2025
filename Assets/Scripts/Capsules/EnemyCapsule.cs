using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyPrefab", menuName = "ScriptableObjects/Enemy")]
public class EnemyCapsule : ScriptableObject, ISpawnable
{
    public string Name;
    public delegate void OnDestroy();
    public OnDestroy onDestroyTrigger;
    public EnemyController enemyPrefab;
    public GameObject GetGameObject()
    {
        return null;
    }
}
