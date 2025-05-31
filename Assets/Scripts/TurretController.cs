using System.Linq;
using UnityEngine;

public class TurretController : MonoBehaviour, ISpawnable
{
    public delegate void OnDestroy();
    public OnDestroy onDestroyTrigger;
    protected TurretCapsule turretData;

    [Header("Vars")]
    [SerializeField] int cost = 50;

    [Header("Targeting")]
    [SerializeField] float range = 5f;
    [SerializeField] float fireRate = 1f;
    [SerializeField] float projectileSpeed = 1000f;
    private float fireCooldown = 0f;

    [Header("References")]
    [SerializeField] Transform cannonRotateTransform;
    [SerializeField] Transform cannonGraphics;
    [SerializeField] Transform cannonRangeGraphics;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject projectilePrefab;

    [SerializeField] BaseProjectile bulletPrefab;

    ObjectPooler<BaseProjectile> projectilesPooler;

    private void Awake()
    {
        projectilesPooler = new ObjectPooler<BaseProjectile>(bulletPrefab);
    }
    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range);
        var enemies = hits
            .Where(h => h.GetComponent<EnemyController>() != null)
            .Select(h => h.transform)
            .ToList();

        if (enemies.Count == 0)
            return;

        Transform target = enemies
            .OrderBy(t => Vector2.Distance(transform.position, t.position))
            .First();

        Vector2 dir = (target.position - cannonRotateTransform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        cannonRotateTransform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (fireCooldown <= 0f)
        {
            Shoot(dir);
            fireCooldown = 1f / fireRate;
        }
    }

    private void Shoot(Vector2 direction)
    {
        if (projectilePrefab == null || firePoint == null)
            return;

        //GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // TODO: Bisogna gestire la distruzione del proiettile in modo intelligente ed estendibile
        // si potrebbe usare una callback onDestroyProjectile ??

        BaseProjectile baseProjectile = projectilesPooler.Get();
        if (!baseProjectile.gameObject.activeSelf)
        {
            baseProjectile.gameObject.SetActive(true);
        }
        else
        {
            baseProjectile.damager.onTriggerEnter += () =>
            {
                projectilesPooler.Set(baseProjectile);
            };
        }
        baseProjectile.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
    }

    public void TurretSelected()
    {
        cannonGraphics.GetComponent<SpriteRenderer>().color = Color.red;
        cannonRangeGraphics.transform.localScale = new Vector2(range, range) * 2;
        cannonRangeGraphics.gameObject.SetActive(true);

        Publisher.Publish(new TurretInfoMessage(fireRate, cost, bulletPrefab.damager.damageAmount));
    }
    public void TurretDeselected()
    {
        cannonGraphics.GetComponent<SpriteRenderer>().color = Color.white;
        cannonRangeGraphics.gameObject.SetActive(false);
        UIManager.Instance.UnshowTurretStats();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    public void DestroySellTurret()
    {
        onDestroyTrigger?.Invoke();
        gameObject.SetActive(false);
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public virtual void Initialize(TurretCapsule _turretData)
    {
        this.turretData = _turretData;
        //lo inizializzo a 0
    }
    public void SellTurret()
    {
        GameManager.Instance.AddCoins(cost);
        //e faccio il set dell'object pooler e lo setto false
        onDestroyTrigger?.Invoke();
        gameObject.SetActive(false);
    }
}
