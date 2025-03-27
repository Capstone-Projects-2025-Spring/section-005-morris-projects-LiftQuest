using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Stats")]
    public int maxHearts = 3;
    private int currentHearts;

    [Header("Combat Settings")]
    public float attackRange = 3f;
    public LayerMask monsterLayer;
    public float clickCooldown = 0.5f;
    private bool canAttack = true;
    
    [Header("Game State")]
    public bool isGameOver = false;

    private Collider[] hitColliders = new Collider[5];

    void Start()
    {
        currentHearts = maxHearts;
        UpdateHealthDisplay();
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()
    {
        if (isGameOver) return;

        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            Debug.Log("button clicked");
            Attack();
        }
    }

    void Attack()
    {
        canAttack = false;
        
        int numColliders = Physics.OverlapSphereNonAlloc(
            transform.position, 
            attackRange, 
            hitColliders, 
            monsterLayer
        );

        bool hitMonster = false;
        
        for (int i = 0; i < numColliders; i++)
        {
            Monster monster = hitColliders[i].GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(1);
                monster.ResetAttackTimer();
                hitMonster = true;
            }

        }

        if (hitMonster)
        {
            Debug.Log("Hit a monster!");
        }
        else
        {
            Debug.Log("No monsters in range");
        }

        Invoke(nameof(ResetAttack), clickCooldown);
    }

    void ResetAttack()
    {
        canAttack = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void TakeDamage(int damage)
    {
        currentHearts -= damage;
        UpdateHealthDisplay();

        if (currentHearts <= 0)
        {
            GameOver();
        }
    }

    void UpdateHealthDisplay()
    {
        Debug.Log($"Player Health: {currentHearts}/{maxHearts}");
    }

    void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("Game Over!");
    }
}