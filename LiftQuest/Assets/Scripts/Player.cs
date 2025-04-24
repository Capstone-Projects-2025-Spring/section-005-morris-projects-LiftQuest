using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Stats")]
    public int maxHearts = 3;
    private int currentHearts;

    [Header("Combat")]
    public float clickCooldown = 0.5f;
    public float attackRadius = 1.5f; // Based on monster scale
    public LayerMask monsterLayer;
    private bool canAttack = true;

    [Header("Game State")]
    public bool isGameOver = false;

    private Collider2D[] hitColliders = new Collider2D[5];

    [SerializeField] private GameManager _gm;

    [SerializeField] private Sprite[] heartSprites;
    [SerializeField] private SpriteRenderer heartImage;
    private int i = 0;

    void Start()
    {
        currentHearts = maxHearts;
        UpdateHealthDisplay();
    }

    void Update()
    {
        if (isGameOver) return;

        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            Attack();
        }
    }

    public void Attack()
    {
        canAttack = false;

        int numHits = Physics2D.OverlapCircleNonAlloc(transform.position, attackRadius, hitColliders, monsterLayer);
        bool hitSomething = false;

        for (int i = 0; i < numHits; i++)
        {
            Monster m = hitColliders[i].GetComponent<Monster>();
            if (m != null)
            {
                m.TakeDamage(1);
                m.ResetAttackTimer();
                hitSomething = true;
            }
        }

        if (hitSomething)
            Debug.Log("Hit a monster!");
        else
            Debug.Log("No monsters in range");

        Invoke(nameof(ResetAttack), clickCooldown);
    }

    void ResetAttack()
    {
        canAttack = true;
    }

    public void TakeDamage(int damage)
    {
        i++;
        heartImage.sprite = heartSprites[i];
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
        _gm.Lose();
        Debug.Log("Game Over!");
    }
}
