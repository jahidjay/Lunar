using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player enters the respawn point's trigger zone
        if (other.CompareTag("Player"))
        {
            DeathManager deathManager = other.GetComponent<DeathManager>();

            if (deathManager != null)
            {
                // Update the player's respawn point to this respawn point
                deathManager.RespawnPoint = transform;
                Debug.Log("Respawn point activated at: " + transform.position);
            }
        }
    }
}