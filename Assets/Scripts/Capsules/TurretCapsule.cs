using UnityEngine;

[CreateAssetMenu(fileName = "NewTurretPrefab", menuName = "ScriptableObjects/Turret")]
public class TurretCapsule : ScriptableObject, ISpawnable
{
    public string Name;
    public delegate void OnDestroy();
    public OnDestroy onDestroyTrigger;
    public TurretController turretPrefab;
    public GameObject GetGameObject()
    {
        return null;
    }
}
