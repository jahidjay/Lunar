using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    
    public GameObject MainMenu;
    public GameObject InstructionMenu;
    public GameObject CreditMenu;

    public void StartTheGame()
    {
        SceneManager.LoadScene(1);
        GameStateManager.Instance.ChangeGameState(GameState.Game);
        GameStateManager.Instance.isWin = false;
    }
    
    // Button click handlers
    public void ShowInstructions()
    {
        MainMenu.SetActive(false);       // Hide Main Menu
        InstructionMenu.SetActive(true); // Show Instruction Menu
    }

    public void ShowCredits()
    {
        MainMenu.SetActive(false);    // Hide Main Menu
        CreditMenu.SetActive(true);  // Show Credit Menu
    }

    public void BackToMainMenu()
    {
        InstructionMenu.SetActive(true); // Hide Instruction Menu
        CreditMenu.SetActive(true);     // Hide Credit Menu
        MainMenu.SetActive(true);        // Show Main Menu
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }
}
