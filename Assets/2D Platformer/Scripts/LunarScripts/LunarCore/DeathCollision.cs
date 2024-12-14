using UnityEngine;

public class DeathCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player enters the death zone
        if (other.CompareTag("Player"))
        {
            // Get the DeathManager component from the player
            DeathManager deathManager = other.GetComponent<DeathManager>();

            if (deathManager != null)
            {
                // Trigger the player's death
                deathManager.Die();
            }
        }
    }
}