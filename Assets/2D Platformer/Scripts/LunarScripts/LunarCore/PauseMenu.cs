using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Reference to the Pause Menu UI
    public GameObject PauseMenuUI;

    // Track if the game is paused
    private bool isPaused = false;

    void Update()
    {
        // Check for the pause button (Escape key or custom input)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
                Debug.Log("Paused");
            }
        }
    }

    // Pause the game
    public void Pause()
    {
        if (PauseMenuUI != null)
        {
            PauseMenuUI.SetActive(true); // Show the Pause Menu
            Time.timeScale = 0f;         // Freeze game time
            isPaused = true;             // Set paused state
            Debug.Log("Pause menu enabled");
        }
        else
        {
            Debug.LogError("PauseMenuUI is not assigned in the Inspector!");
        }
    }

    // Resume the game
    public void Resume()
    {
        if (PauseMenuUI != null)
        {
            PauseMenuUI.SetActive(false); // Hide the Pause Menu
            Time.timeScale = 1f;          // Resume game time
            isPaused = false;             // Reset paused state
            Debug.Log("Game resumed. Time Scale: " + Time.timeScale);
        }
    }

    // Return to the Main Menu (or LunarMenu Scene)
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Ensure game time is running before switching scenes
        SceneManager.LoadScene("LunarMenu"); // Replace with your Main Menu scene name
    }
}