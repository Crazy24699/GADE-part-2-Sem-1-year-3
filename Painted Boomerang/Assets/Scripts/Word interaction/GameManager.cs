using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager GameManagerInstance;
    public bool GameStarted = false;

    public int CellSize;

    

    void Start()
    {

        if(GameManagerInstance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            GameManagerInstance = this;
        }



    }

    public void GeneratMapLayout(int ChosenSize)
    {
        switch (ChosenSize)
        {
            //Small

            case 1:
                //Height = 8;
                //Length = 4;
                CellSize = 8;
                break;

            //Large
            default:
            case 2:
                //Height = 32;
                //Length = 64;
                CellSize = 4;
                break;
        }
    }


    
}
