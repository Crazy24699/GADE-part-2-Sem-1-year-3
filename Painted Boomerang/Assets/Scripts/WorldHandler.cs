using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class WorldHandler : MonoBehaviour
{
    const int AreaLength = 64;
    const int AreaHeight = 32;
    const int MaxMoves = 2;
    private int AreaCellHeight;
    private int AreaCellLength;
    protected int WorldCellSize;
    [Space(5)]
    

    [Space(10)]
    public bool CanMakeMove;

    [Space(10)]
    public GameObject TilePrefab;
    public GameObject LoadingScreenPanel;

    [Space(5)]
    public GameObject PlayPieceRef;

    
    [Space(10)]
    public HashSet<CellFunctionality> AllCells = new HashSet<CellFunctionality>();

    public GameManager GameManagerScript;
    public PopulateGrid PopulateScript;
    public PlayerFunctionality Team1Script;
    public PlayerFunctionality Team2Script;

    public enum Teams
    {
        Team1,
        Team2
    }
    public Teams CurrentTeam;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManagerScript != null)
        {
            return;
        }

        GameManagerScript = GameManager.GameManagerInstance;

        WorldCellSize = 4;

        AreaCellHeight = AreaHeight / WorldCellSize;
        AreaCellLength = AreaLength / WorldCellSize;

        PopulateScript = GameObject.FindObjectOfType<PopulateGrid>();
        StartCoroutine(GenerateGrid());
        CurrentTeam = Teams.Team1;

    }

    #region Grid Generation
    public IEnumerator GenerateGrid()
    {

        bool CanPlaceObstructions = true;

        //Change to possibly be random
        int ObstructionWait = 5;
        int CurrentObstructionWait = 0;

        HashSet<CellFunctionality> Cells = new HashSet<CellFunctionality>();

        for (int xCord = 0; xCord < AreaLength/ WorldCellSize; xCord++)
        {

            for (int yCord = 0; yCord < AreaHeight/WorldCellSize; yCord++)
            {
                yield return new WaitForSeconds(0.005f);

                GameObject SpawnedObject = Instantiate(TilePrefab, new Vector3(xCord*4, yCord+10), Quaternion.identity);
                SpawnedObject.name = $"Tile: x:{xCord} y:{yCord}";
                SpawnedObject.transform.SetParent(GameObject.Find("PlayableGrid").transform);
                SpawnedObject.transform.position = new Vector3((xCord * 4) - (AreaLength / 2) + 2f, (yCord * 4) - (AreaHeight / 2) + 2f);

                if(yCord == 0 || yCord == AreaHeight / WorldCellSize-1 || xCord==0 || xCord== AreaLength / WorldCellSize-1)
                {
                    SpawnedObject.name = $"Tile: x:{xCord} y:{yCord} First last";
                    SpawnedObject.GetComponent<CellFunctionality>().IsBorderCell = true;
                }
                SpawnedObject.GetComponent<CellFunctionality>().Location = new Vector2Int(xCord, yCord);
                InitialStartArea(SpawnedObject, xCord, yCord);
                
                Cells.Add(SpawnedObject.GetComponent<CellFunctionality>());
                #region RemoveRandomizer

                if (CanPlaceObstructions && CurrentObstructionWait == 0)
                {
                    Debug.Log("Spin");
                    CurrentObstructionWait = ObstructionWait;
                    int Chance = Random.Range(0, 24);
                    if (Chance <= 5)
                    {
                        int ObstructionCount = Random.Range(1, 3);
                        
                    }
                    //CanPlaceObstructions = false;
                }
                else if (!CanPlaceObstructions || CurrentObstructionWait > 0)
                {
                    CurrentObstructionWait -= 1;
                }
                else if (CurrentObstructionWait <= 0) 
                {
                    CurrentObstructionWait = 0;
                    CanPlaceObstructions= true;
                }

                #endregion
                
            }
            PopulateScript.GridCells = Cells.ToList();
            AllCells = Cells;
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(PopulateNearCells());
        Debug.Log(AllCells.Count);
    }

    private IEnumerator PopulateNearCells()
    {
        foreach (CellFunctionality CellScript in AllCells)
        {
            yield return new WaitForSeconds(0.025f);
            CellScript.PopulateNearCellList(AreaHeight, AreaLength);
        }
        //PopulateScript.PickWallSpots();
    }

    /*Make the individual entities spawn into their respective team numbers, make the team numbers as parents within the world
     that is then parented to a major parent that houses it all, this is just so it looks clean and better. */

    protected void InitialStartArea(GameObject SpawnedObjectRef, int xCordRef, int yCordRef)
    {

        if ((yCordRef == (AreaCellHeight / 2) - 1 || yCordRef == AreaCellHeight/2))
        {

            switch (xCordRef < AreaCellLength / 2)
            {

                case true:
                    if (xCordRef > 0 && xCordRef < 3)
                    {
                        SpawnPieces(SpawnedObjectRef,Teams.Team1);
                    }
                    break;

                case false:
                    if (xCordRef > AreaCellLength - 4 && xCordRef < AreaCellLength - 1)
                    {
                        SpawnPieces(SpawnedObjectRef,Teams.Team2);
                    }
                    break;
            }
            //Debug.Log($"X cord: {xCordRef} {AreaCellLength}");
        }
    }

    public void SpawnPieces(GameObject ObjectRef, Teams ThisTeam)
    {
        GameObject SpawnedPiece;
        SpawnedPiece = Instantiate(PlayPieceRef, new Vector3(ObjectRef.transform.position.x, ObjectRef.transform.position.y, -1), Quaternion.identity);


        ObjectRef.GetComponent<CellFunctionality>().StartingCell = true;
        SpawnedPiece.GetComponent<EntityBase>().AssignedTeam = ThisTeam;
        switch (ThisTeam)
        {
            case Teams.Team1:
                ObjectRef.GetComponent<SpriteRenderer>().color = Color.green;
                SpawnedPiece.transform.SetParent(Team1Script.transform);
                Team1Script.Entities.Add(SpawnedPiece.GetComponent<EntityBase>());
                break;

            case Teams.Team2:
                ObjectRef.GetComponent<SpriteRenderer>().color = Color.red;
                SpawnedPiece.transform.SetParent(Team2Script.transform);
                Team2Script.Entities.Add(SpawnedPiece.GetComponent<EntityBase>());
                break;

        }
    }

    #endregion

    public void SetActivePlayer(Teams ActiveTeam)
    {
        switch (ActiveTeam)
        {
            case Teams.Team2:
                Team1Script.TurnActive = true;
                Team2Script.TurnActive = false;
                Team1Script.MovesRemaining = MaxMoves;
                break;

            case Teams.Team1:
                Team2Script.TurnActive = true;
                Team1Script.TurnActive = false;
                Team2Script.MovesRemaining = MaxMoves;
                break;


        }
    }
    
    
}
