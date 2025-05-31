using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    //public delegate void OnCollision();
    //public OnCollision onCollisionEnter;

    [SerializeField] protected float speed = 1;

    private void Update()
    {
        transform.position += speed * Time.deltaTime * transform.up;
    }
}
