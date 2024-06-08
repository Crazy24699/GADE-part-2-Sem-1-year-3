using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFunctionality : MonoBehaviour
{
    public TextMeshProUGUI Team1Score;
    public TextMeshProUGUI Team2Score;
    public TextMeshProUGUI ScreenMessage;
    [Space(10)]

    public GameObject GameUI;
    public GameObject PauseUI;

    public bool GamePaused;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if(Time.timeScale <= 0f)
        {
            GamePaused = true;
        }
        switch (GamePaused)
        {
            default:
            case true:
                GamePaused = false;
                Time.timeScale = 1;
                break;

            case false:
                GamePaused = true;
                Time.timeScale = 0;
                break;
        }

        PauseUI.SetActive(GamePaused);


    }

    public void Resume()
    {
        PauseGame();
        PauseUI.SetActive(false);
    }

    public void RestartGame()
    {
        PauseGame();
        SceneManager.LoadScene("Main Level");
    }

    public void MoveToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

}