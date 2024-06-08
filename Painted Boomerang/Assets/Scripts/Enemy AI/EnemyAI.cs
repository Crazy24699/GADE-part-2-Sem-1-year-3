using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public Vector2 Direcitonal;
    //Gameobjects

    protected GameObject EnemyBase;
    public List<GameObject> PiecesInPlay = new List<GameObject>();
    [SerializeField] protected GameObject BoomerangRef;

    //Scripts
    public BehaviourTree BehaviourTreeScript;
    public Teams AssignedTeam;
    protected WorldHandler WorldHandlerScript;
    public LayerMask InteractableLayers;
    public EntityBase ChosenPiece;
    public PlayerFunctionality ThisPlayerScript;

    //Bools
    [HideInInspector] public bool EnemyInRange;
    [HideInInspector]public bool CanThrow = true;
    public bool GameStarted = false;
    public bool PieceSelected;
    public bool LowHealthPiece;
    public bool AttackingActive;
    public bool CanPerformMove;

    //Floats
    protected float TimerLength = 2.5f;
    protected float CurrentTime;

    public void AIStartup()
    {
        WorldHandlerScript = GameObject.FindObjectOfType<WorldHandler>();

        PlayerFunctionality[] PlayerScripts = FindObjectsByType<PlayerFunctionality>(FindObjectsSortMode.None);
        foreach (var Script in PlayerScripts)
        {
            if (Script.ThisTeam == AssignedTeam)
            {
                ThisPlayerScript = Script;
                break;
            }
        }
        Debug.Log("Gutter");
        PiecesInPlay = GetAllPieces();
        BehaviourTreeScript = GameObject.FindObjectOfType<BehaviourTree>();
        BehaviourTreeScript.BehaviourTreeStartup();
    }

    void Update()
    {
        if(ChosenPiece == null)
        {
            return;
        }
        CanPerformMove = ThisPlayerScript.CanPerformAction;
        
        //DrawBouncingRay(ChosenPiece.transform.position, Direcitonal, 3);
    }


    void DrawBouncingRay(Vector2 origin, Vector2 direction, int remainingBounces)
    {
        Vector2 currentOrigin = origin;
        Vector2 currentDirection = direction;
        float remainingDistance = 10000;

        for (int i = 0; i < 3; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentOrigin, currentDirection, Mathf.Infinity, InteractableLayers);

            if (hit.collider != null)
            {
                // Draw the current segment of the ray
                Debug.DrawLine(currentOrigin, hit.point, Color.red);

                // Calculate the reflection direction
                currentDirection = Vector2.Reflect(currentDirection, hit.normal);

                // Update the origin to the hit point
                currentOrigin = hit.point;
                Debug.Log(currentOrigin);
                // Reduce the remaining distance
                remainingDistance -= Vector2.Distance(currentOrigin, hit.point);

                // Check if there are remaining bounces
                if (remainingBounces > 0)
                {
                    remainingBounces--;
                }
                else
                {
                    break;
                }
            }
            else
            {
                // Draw the remaining segment of the ray
                Debug.DrawLine(currentOrigin, currentOrigin + currentDirection * remainingDistance, Color.red);
                Debug.Log("Ruin");
                break;
            }
        }
    }

    public void AIChangePieceState(bool PieceStateChange)
    {
        switch (PieceStateChange)
        {
            case true:
                ThisPlayerScript.SelectPiece(ChosenPiece.gameObject);
                PieceSelected = true;
                break;

            case false:
                ThisPlayerScript.DeselectPiece();
                ChosenPiece = null;
                PieceSelected = false;
                break;
        }
    }



    public List<GameObject> GetAllPieces()
    {
        HashSet<EntityBase> AvailablePieces = new HashSet<EntityBase>();

        AvailablePieces = GameObject.FindObjectsByType<EntityBase>(FindObjectsSortMode.None).ToHashSet();
        List<GameObject> Pieces = new List<GameObject>();

        foreach (EntityBase FoundPiece in AvailablePieces)
        {
            if (FoundPiece.AssignedTeam == AssignedTeam) 
            {
                Pieces.Add(FoundPiece.gameObject);
                FoundPiece.Startup();
            }
        }

        return Pieces;
    }

}


