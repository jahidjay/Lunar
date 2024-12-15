using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardBroken : MonoBehaviour
{
    [Header("Block Settings")]
    public List<GameObject> blocks; // Blocks that will fall
    public float fallDelay = 1.0f;  // Delay before the blocks fall

    [Header("Respawn Settings")]
    public float respawnDelay = 5.0f; // Time before blocks respawn
    private List<Vector3> initialPositions = new List<Vector3>(); // Store initial positions

    private Rigidbody2D rb;
    private BoxCollider2D box;

    private void Start()
    {
        // Cache Rigidbody and Collider
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();

        // Store initial positions of all blocks
        foreach (GameObject block in blocks)
        {
            initialPositions.Add(block.transform.position);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FallBlocks());
        }
    }

    private IEnumerator FallBlocks()
    {
        yield return new WaitForSeconds(fallDelay); // Wait for the fall delay

        // Enable gravity and make the blocks dynamic
        foreach (GameObject block in blocks)
        {
            Rigidbody2D blockRb = block.GetComponent<Rigidbody2D>();
            if (blockRb == null)
            {
                blockRb = block.AddComponent<Rigidbody2D>();
            }

            blockRb.isKinematic = false;
            blockRb.gravityScale = Random.Range(0.5f, 3.0f); // Randomized gravity for variation
        }

        // Disable this block's collider to simulate falling
        ChangeBodyType(RigidbodyType2D.Dynamic, false);

        // Wait for respawn delay and reset the blocks
        yield return new WaitForSeconds(respawnDelay);
        Respawn();
    }

    public void Respawn()
    {
        // Reset the position and physics for all blocks
        for (int i = 0; i < blocks.Count; i++)
        {
            GameObject block = blocks[i];

            // Reset block's position to its initial value
            block.transform.position = initialPositions[i];

            // Reset Rigidbody properties
            Rigidbody2D blockRb = block.GetComponent<Rigidbody2D>();
            if (blockRb != null)
            {
                blockRb.isKinematic = true;
                blockRb.gravityScale = 0;
                blockRb.velocity = Vector2.zero; // Clear any residual velocity
                blockRb.angularVelocity = 0f;   // Clear any angular velocity
            }
        }

        // Reset this block's physics and collider
        ChangeBodyType(RigidbodyType2D.Kinematic, true);

        Debug.Log("Blocks have been respawned.");
    }

    private void ChangeBodyType(RigidbodyType2D newBodyType, bool enableCollider)
    {
        if (rb != null)
        {
            rb.bodyType = newBodyType;
        }

        if (box != null)
        {
            box.enabled = enableCollider;
        }
    }
}
