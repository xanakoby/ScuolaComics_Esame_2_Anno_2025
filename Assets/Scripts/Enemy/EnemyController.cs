using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour, ISpawnable
{
    public delegate void OnDestroy();
    public OnDestroy onDestroyTrigger;
    protected EnemyCapsule enemyData;

    [Header("Path")]
    [SerializeField] List<Transform> pathPoints;
    private int currentPointIndex = 0;

    [Header("Movement")]
    [SerializeField] float speed = 2f;

    [Header("Health")]
    [SerializeField] float maxHealth = 10f;
    [SerializeField] float currentHealth;
    [SerializeField] GameObject canvasLife;
    [SerializeField] Image lifeBar;

    [Header("Damage")]
    [SerializeField] int damageToPlayer = 1;

    [Header("Graphics")]
    [SerializeField] SpriteRenderer graphicsObject;

    [Header("Value")]
    [SerializeField] int valueCoin;

    // TODO: Modificare lo script in modo che si usi il RigidBody2D per il movimento invece che transform.position

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        FollowPath();
    }

    private void FollowPath()
    {
        if (pathPoints == null || pathPoints.Count == 0) return;

        Vector3 targetPoint = pathPoints[currentPointIndex].position;
        Vector3 direction = (targetPoint - transform.position).normalized;

        transform.position += direction * speed * Time.deltaTime;

        UpdateGraphicsRotation(direction);

        if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
        {
            currentPointIndex++;
            if (currentPointIndex >= pathPoints.Count)
                ReachExit();
        }
    }

    private void UpdateGraphicsRotation(Vector3 direction)
    {
        if (graphicsObject == null) return;

        float angle;
        bool horizontal = Mathf.Abs(direction.x) > Mathf.Abs(direction.y);

        if (horizontal)
        {
            if (direction.x > 0f)
            {
                angle = 90f;
                graphicsObject.flipY = true;
            }
            else
            {
                angle = -90f;
                graphicsObject.flipY = true;
            }
        }
        else
        {
            if (direction.y > 0f)
            {
                angle = 0f;
            }
            else
            {
                angle = 180f;
            }
            graphicsObject.flipY = false;
        }

        graphicsObject.transform.localEulerAngles = new Vector3(0f, 0f, angle);
    }



    private void ReachExit()
    {
        // TODO: In che modo possiamo togliere la vita alla Base del giocatore senza avere un riferimento diretto?

        Debug.Log("usito");
        Die();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (!canvasLife.activeSelf)
            canvasLife.SetActive(true);

        lifeBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0f) 
        {
            GameManager.Instance.AddCoins(valueCoin);
            Die();
        }
    }

    private void Die()
    {
        // TODO: Si potrebbe fare di meglio? Come possiamo non eliminare l'oggetto e usarlo in un altro modo?
        //Destroy(gameObject);
        onDestroyTrigger?.Invoke();
        gameObject.SetActive(false);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public virtual void Initialize(EnemyCapsule _EnemyData)
    {
        this.enemyData = _EnemyData;
        pathPoints = GameManager.Instance.GetPathList();
        currentPointIndex = 0;
    }
}
