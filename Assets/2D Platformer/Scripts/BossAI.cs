using System.Collections;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("Player Detection")]
    public Transform player; // Reference to the player's transform
    public float detectionRange = 10f; // Detection radius
    public float attackRange = 2f; // Attack range

    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Attack")]
    public float attackCooldown = 2f; // Cooldown between attacks
    private float cooldownTimer = Mathf.Infinity;

    [Header("Health")]
    public int health = 100;

    private Animator animator; // Animator reference
    private Rigidbody2D rb; // Rigidbody for movement
    private bool isFacingRight = true; // Track facing direction
    private LunarEnemyMeleeAttack meleeAttack; // Reference to the melee attack script

    private enum State { Idle, Run, Attack, Death }
    private State currentState = State.Idle;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Reference the melee attack script for damaging the player
        meleeAttack = GetComponent<LunarEnemyMeleeAttack>();
    }

    private void Update()
    {
        if (health <= 0)
        {
            ChangeState(State.Death);
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && cooldownTimer >= attackCooldown)
        {
            ChangeState(State.Attack);
        }
        else if (distanceToPlayer <= detectionRange)
        {
            ChangeState(State.Run);
        }
        else
        {
            ChangeState(State.Idle);
        }

        HandleStates();
        cooldownTimer += Time.deltaTime;
    }

    private void ChangeState(State newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        // Update animator parameters
        animator.SetBool("IsRunning", currentState == State.Run);
        animator.SetBool("IsAttacking", currentState == State.Attack);
        animator.SetBool("IsDead", currentState == State.Death);
    }

    private void HandleStates()
    {
        switch (currentState)
        {
            case State.Run:
                MoveTowardsPlayer();
                break;

            case State.Attack:
                PerformAttack();
                break;

            case State.Death:
                rb.velocity = Vector2.zero;
                break;

            case State.Idle:
                rb.velocity = Vector2.zero;
                break;
        }
    }

    private void MoveTowardsPlayer()
    {
        // Move the boss towards the player
        Vector2 direction = (player.position - transform.position).normalized;

        // Flip the boss to face the player
        if (direction.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (direction.x < 0 && isFacingRight)
        {
            Flip();
        }

        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
    }

    private void PerformAttack()
    {
        cooldownTimer = 0f;

        // Trigger the melee attack animation
        animator.SetTrigger("Attack");

        // Damage player using the LunarEnemyMeleeAttack script
        if (meleeAttack != null)
        {
            meleeAttack.DamagePlayer();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Flip the x-axis
        transform.localScale = scale;
    }

    public void TakeDamage(int damage)
    {
        if (health <= 0) return;

        health -= damage;
        Debug.Log("Boss Health: " + health);

        if (health <= 0)
        {
            ChangeState(State.Death);
            Debug.Log("Boss has died!");
        }
    }
}
