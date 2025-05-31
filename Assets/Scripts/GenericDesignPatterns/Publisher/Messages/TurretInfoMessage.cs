using UnityEngine;

public class TurretInfoMessage : IPublisherMessage
{
    public TurretInfoMessage(float _fireRate, int _cost, float _bulletsDamage)
    {
        fireRate = _fireRate;
        cost = _cost;
        bulletsDamage = _bulletsDamage;
    }
    public float fireRate { get; }
    public int cost { get; }
    public float bulletsDamage { get; }
}
