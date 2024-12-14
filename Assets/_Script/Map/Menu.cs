using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
    }

    public void Level01()
    {
        SceneManager.LoadScene("Level01");
    }

    public void Level02()
    {
        SceneManager.LoadScene("Level02");
    }

    public void MenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    public void _Quit()
    {
        Application.Quit();
    }
}
