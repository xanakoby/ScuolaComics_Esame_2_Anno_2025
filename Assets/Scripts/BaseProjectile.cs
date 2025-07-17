using UnityEngine;

public class BaseProjectile : MonoBehaviour
{

    [SerializeField] protected float speed = 1;
    public Damager damager;
    private void Start()
    {
        Invoke(nameof(DestroyAfterTime), 4f); 
    }
    private void Update()
    {
        transform.position += speed * Time.deltaTime * transform.up;
    }
    private void DestroyAfterTime()
    {
        gameObject.SetActive(false);
    }
}
