using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldHandler : MonoBehaviour
{
    const int AreaLength = 64;
    const int AreaHeight = 32;

    public GameObject TilePrefab;
    public GameObject LoadingScreenPanel;

    protected int WorldCellSize;

    public GameManager GameManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        if (GameManagerScript == null)
        {
            WorldCellSize = 4;

            StartCoroutine(GenerateGrid());

            return;
        }
        GameManagerScript = GameManager.GameManagerInstance;

    }

    public IEnumerator GenerateGrid()
    {

        for (int i = 0; i < AreaLength/ WorldCellSize; i++)
        {

            for (int j = 0; j < AreaHeight/WorldCellSize; j++)
            {
                yield return new WaitForSeconds(0.1f);
                GameObject SpawnedObject = Instantiate(TilePrefab, new Vector3(i*4, j+10), Quaternion.identity);
                SpawnedObject.name = $"Tile: x:{i} y:{j}";
                SpawnedObject.transform.SetParent(GameObject.Find("PlayableGrid").transform);
                SpawnedObject.transform.position = new Vector3((i * 4) - (AreaLength / 2) + 2f, (j * 4) - (AreaHeight / 2) + 2f);
                if(j == 0 || j == AreaHeight / WorldCellSize-1 || i==0 || i== AreaLength / WorldCellSize-1)
                {
                    SpawnedObject.name = $"Tile: x:{i} y:{j} First last";
                }
            }
            
        }
    }

    
}
