using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
    [Header("Monster Settings")]
    public int maxHealth = 7;
    private int currentHealth;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float attackRange = 2f;

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

        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance > attackRange)
        {
            MoveTowardsPlayer();
        }
        else if (attackTimer <= 0)
        {
            AttackPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        transform.LookAt(player.transform);
    }

    void AttackPlayer()
    {
        player.TakeDamage(damage);
        ResetAttackTimer();
    }

    public void ResetAttackTimer()
    {
        attackTimer = attackCooldown;
    }

    public void TakeDamage(int damageAmount)
    {
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
            spawner.update();
        }
        Destroy(gameObject);
    }
}