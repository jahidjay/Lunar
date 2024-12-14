using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MainMenu,
    Game,
    Ending
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public bool isWin = false; 
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
            Destroy(gameObject);
    }

    public GameState state;

    private void Start()
    {
        state = GameState.MainMenu; 
    }

    public void ChangeGameState(GameState gameState)
    {
        state  = gameState;
    }

}
