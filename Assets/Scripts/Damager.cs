using Unity.VisualScripting;
using UnityEngine;

public class Damager : MonoBehaviour
{
    public delegate void OnTriggerEnter();
    public OnTriggerEnter onTriggerEnter;

    public float damageAmount = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyController enemyController))
        {
            enemyController.TakeDamage(damageAmount);
        }
        onTriggerEnter?.Invoke();

        gameObject.SetActive(false);
    }
    private void OnBecameInvisible()
    {
        onTriggerEnter?.Invoke();

        gameObject.SetActive(false);
    }
}
