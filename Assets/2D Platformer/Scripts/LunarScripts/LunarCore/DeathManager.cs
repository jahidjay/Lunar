using UnityEngine;

public class DeathManager : MonoBehaviour
{
    public Transform RespawnPoint; // The current active respawn point

    private Vector3 initialPosition; // Player's initial position in the scene

    void Start()
    {
        // Save the player's starting position as the initial respawn point
        initialPosition = transform.position;

        // Set the initial respawn point to the player's starting position
        if (RespawnPoint == null)
        {
            RespawnPoint = new GameObject("InitialRespawnPoint").transform;
            RespawnPoint.position = initialPosition;
        }
    }

    public void Die()
    {
        Debug.Log("Player died!");

        // Respawn the player at the last active respawn point
        RespawnAtCheckpoint();
    }

    public void RespawnAtCheckpoint()
    {
        // Move the player to the current respawn point
        transform.position = RespawnPoint.position;
        Animator anim = gameObject.GetComponentInChildren<Animator>();
        anim.Rebind();
        anim.Update(0f);
        GetComponent<LunarHealth>().FullHeal(); 
        Debug.Log("Respawning player at respawn point: " + RespawnPoint.position);
    }
}