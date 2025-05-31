using UnityEngine;

public class TurretInfoMessage : IPublisherMessage
{
    public TurretInfoMessage(float _fireRate, int _cost)
    {
        fireRate = _fireRate;
        cost = _cost;
    }
    public float fireRate { get; }
    public int cost;
}
