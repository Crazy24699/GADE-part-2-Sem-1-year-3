using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    //Gameobjects

    protected GameObject EnemyBase;
    [HideInInspector]public List<GameObject> PiecesInPlay = new List<GameObject>();
    [SerializeField] protected GameObject BoomerangRef;

    //Scripts
    public BehaviourTree BehaviourTreeScript;
    public WorldHandler.Teams AssignedTeam;
    public LayerMask InteractableLayers;
    public EntityBase ChosenPiece;


    //Bools
    [HideInInspector] public bool EnemyInRange;
    [HideInInspector]public bool CanThrow = true;
    private bool GameStarted = false;
    public bool PieceSelected;
    public bool LowHealthPiece;

    //Floats
    protected float TimerLength=2.5f;
    protected float CurrentTime;

    // Start is called before the first frame update
    void Start()
    {

        PiecesInPlay = GetAllPieces();

        if (GameStarted)
        {

        }

    }

    // Update is called once per frame
    void Update()
    {




    }

    private void FixedUpdate()
    {
        
    }

    public void MakeDecision()
    {

    }


    protected void SelectPiece()
    {

    }



    private void PerformPieceAction()
    {


        GameStarted = true;
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
            }
        }

        return Pieces;
    }

}


