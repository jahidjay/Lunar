using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameFinishUI : MonoBehaviour
{
    public Text endingStatus;

    private void Start()
    {
        
        if (GameStateManager.Instance.isWin)
        {
            endingStatus.text = "YOU WIN..!";
        }
        else endingStatus.text = "YOU DIED..! :(";
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("LunarMenu");
        GameStateManager.Instance.ChangeGameState(GameState.Game);
        GameStateManager.Instance.isWin = false;

    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(1);
        GameStateManager.Instance.ChangeGameState(GameState.Game);
        GameStateManager.Instance.isWin = false;
    }
}
