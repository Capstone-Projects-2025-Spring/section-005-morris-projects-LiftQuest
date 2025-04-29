using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
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
    [SerializeField] private Animator _anim;
    [SerializeField] private AudioSource audioSource;
    public AudioClip attackSound;
    public AudioClip damageSound;
    public AudioClip deathSound;

    public String enemyAnim;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
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
            Debug.Log("Boss attacked! Game over.");
            spawner.Win();
        }
        else
        {
            _anim.SetBool("isAttacking", true);
            player.StartEnemyAnim(enemyAnim);
            StartCoroutine(ResetAttackAnimation());
        }

        ResetAttackTimer();
    }

    IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(1f); // Wait 0.5 seconds
        audioSource.PlayOneShot(attackSound);
        player.TakeDamage(damage);
        _anim.SetBool("isAttacking", false);
    }

    public void TakeDamage(int damageAmount)
    {
        if (isBoss)
        {
            _anim.SetBool("isDamaged", true);
            StartCoroutine(ResetDamageAnimation());
            spawner.score++;
            spawner.UpdateScoreUI();
            Debug.Log("Boss hit! Score: " + spawner.score);
            return;
        }

        _anim.SetBool("isDamaged", true);
        StartCoroutine(ResetDamageAnimation());

        currentHealth -= damageAmount;
        Debug.Log("Monster hit! Health: " + currentHealth);

        if (currentHealth <= 0)
        {
           audioSource.PlayOneShot(deathSound);
           StartCoroutine(Die());
        }
    }

    IEnumerator ResetDamageAnimation()
    {
        audioSource.PlayOneShot(damageSound);
        yield return new WaitForSeconds(0.5f); // Wait 0.5 seconds
        _anim.SetBool("isDamaged", false);
    }


    public void ResetAttackTimer()
    {
        attackTimer = attackCooldown;
    }


    IEnumerator Die()
    {
        yield return new WaitForSeconds(0.5f);
        if (spawner != null)
        {
            spawner.score++;
            spawner.UpdateScoreUI();
            spawner.OnMonsterDefeated();
        }
        Destroy(gameObject);
    }

    

}
