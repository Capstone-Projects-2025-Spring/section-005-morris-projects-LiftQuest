using UnityEngine;
public class Monster : MonoBehaviour
{
    [Header("Monster Stats")]
    public int maxHealth = 7;
    private int currentHealth;

    [Header("Boss Settings")]
    public bool isBoss = false;

    [Header("Growth")]
    public float maxScale = 1.5f;
    public float scaleSpeed = 0.3f;
    public float attackThreshold = 1.2f;

    [Header("Attack")]
    public int damage = 1;
    public float attackCooldown = 5f;
    private float attackTimer;

    [Header("References")]
    public Player player;
    public GameManager spawner;

    void Start()
    {
        if (player == null)
            player = FindObjectOfType<Player>();

        currentHealth = maxHealth;
        ResetAttackTimer();
    }

    void Update()
    {
        if (player == null || player.isGameOver) return;

        attackTimer -= Time.deltaTime;
        GrowTowardPlayer();

        if (transform.position.y <= attackThreshold && attackTimer <= 0)
        {
            AttackPlayer();
        }
    }

    void GrowTowardPlayer()
    {
        float scaleStep = scaleSpeed * Time.deltaTime;
        transform.position += Vector3.down * (scaleStep * 0.3f);
    }

    void AttackPlayer()
    {
        if (isBoss)
        {
            // Boss ends the game on attack
            Debug.Log("Boss attacked! Game over.");
            spawner.Win(); // Ensure death
        }
        else
        {
            player.TakeDamage(damage);
        }

        ResetAttackTimer();
    }

    public void ResetAttackTimer()
    {
        attackTimer = attackCooldown;
    }

    public void TakeDamage(int damageAmount)
    {
        if (isBoss)
        {
            // Boss cannot be killed, just reward points
            spawner.score++;
            spawner.UpdateScoreUI();
            Debug.Log("Boss hit! Score: " + spawner.score);
            return;
        }

        currentHealth -= damageAmount;
        Debug.Log("Monster hit! Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (spawner != null)
        {
            spawner.score++;
            spawner.UpdateScoreUI();
            spawner.OnMonsterDefeated();
        }
        Destroy(gameObject);
    }
}
