using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgramManager : MonoBehaviour
{
    public static ProgramManager ProgramManagerInstance;
    public bool GameStarted = false;
    public bool HardmodeActive;
    
    public enum DifficultyOptions
    {
        Easy,
        Hard,
    };
    public DifficultyOptions ChosenDifficulty;

    public int CellSize;



    void Start()
    {

        if (ProgramManagerInstance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            ProgramManagerInstance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDifficulty(int Difficulty)
    {
        switch (Difficulty)
        {
            case 0:
                ChosenDifficulty = DifficultyOptions.Easy;
                break;

            case 1:
                ChosenDifficulty = DifficultyOptions.Hard;
                break;
        }
    }
}
