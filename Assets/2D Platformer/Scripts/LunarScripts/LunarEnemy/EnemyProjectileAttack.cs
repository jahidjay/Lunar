using UnityEngine;

public class EnemyProjectileAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public GameObject projectilePrefab; // Prefab of the projectile
    public Transform firePoint; // Position where the projectile will spawn
    public float attackCooldown = 2f; // Time between each attack
    public float projectileSpeed = 10f; // Speed of the projectile

    [Header("Player Detection")]
    public Transform player; // Reference to the player's transform
    public float detectionRange = 15f; // Distance at which the AI detects the player

    private float cooldownTimer = 0f; // Timer to track attack cooldown

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        // Check if player is within range
        if (PlayerInRange())
        {
            // Attack if cooldown is complete
            if (cooldownTimer >= attackCooldown)
            {
                Attack();
                cooldownTimer = 0f; // Reset cooldown
            }
        }
    }

    private bool PlayerInRange()
    {
        if (player == null) return false;

        // Calculate the distance between AI and player
        float distance = Vector2.Distance(transform.position, player.position);
        return distance <= detectionRange;
    }

    private void Attack()
    {
        // Spawn a projectile and shoot it towards the player
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            // Calculate direction towards the player
            Vector2 direction = (player.position - firePoint.position).normalized;

            // Apply velocity to the projectile
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * projectileSpeed;
            }

            Debug.Log("Projectile fired at the player!");
        }
        else
        {
            Debug.LogError("Projectile Prefab or Fire Point is not assigned!");
        }
    }
}
