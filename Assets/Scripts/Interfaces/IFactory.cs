using NUnit.Framework.Interfaces;
using UnityEngine;

public interface IFactory
{
    //GameObject Create(ScriptableObject objectData);

    ISpawnable Create(EnemyCapsule _enemyCapsule, Vector3 _spawnPosition, Quaternion _rotation);
    ISpawnable Create(TurretCapsule _turretCapsule, Vector3 _spawnPosition, Quaternion _rotation);
}
