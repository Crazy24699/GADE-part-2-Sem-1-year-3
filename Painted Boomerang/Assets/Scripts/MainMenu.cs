using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene("PVP");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadPlayerLevel(string LoadLevel)
    {
        SceneManager.LoadScene(LoadLevel);
    }

}
