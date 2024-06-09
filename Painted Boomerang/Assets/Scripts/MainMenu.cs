using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject MainScreenPanel;
    public GameObject GameSelectionPanel;

    public void StartGame()
    {
        MainScreenPanel.SetActive(false);
        GameSelectionPanel.SetActive(true);
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
