using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BTChoice : BTNodeBase
{

    public EnemyAI EnemyAIScript;
    public WorldHandler WorldHandlerScript;
    public NavMeshAgent AgentRef;

    protected EntityBase ChosenPieceRef;

    private int RandomAction;

    //Vectors
    protected Vector2 CoverDestination;
    [SerializeField]protected List<Vector2> MoveOptions=new List<Vector2>();

    //Floats
    protected float CoverDistance;

    private float ChoiceDelayTime = 0.85f;
    public float CurrentDelayTime;

    private float NextDecisionDelay = 1.25f;
    private float NextDecisionDelayTime;
    
    //Bools
    protected bool Attacking;
    protected bool Retreating;
    protected bool CheckedDirections = false;
    protected bool InSight;     //If the piece has direct line of sight to the players pieces
    private bool DirectionChosen = false;
    private bool MovingPiece = false;
    private bool ActionChosen = false;

    public BTChoice(GameObject EnemyAIRef)
    {
        WorldHandlerScript = GameObject.FindObjectOfType<WorldHandler>();
        EnemyAIScript = EnemyAIRef.GetComponent<EnemyAI>();

        CurrentDelayTime = ChoiceDelayTime; 

        RunStartup();
    }

    public override NodeStateOptions RunNodeLogicAndStatus()
    {

        if(!EnemyAIScript.ThisPlayerScript.TurnActive)
        {
            return NodeStateOptions.Fail;
        }
        
        #region Decision Delay
        if (NextDecisionDelayTime > 0 )
        {
            NextDecisionDelayTime -= Time.deltaTime;
            Debug.Log("self fufullied");
            return NodeStateOptions.Running;
        }
        #endregion

        if (EnemyAIScript.PieceSelected && NextDecisionDelayTime <= 0) 
        {
            
            Debug.Log("Rip tear and maim    ");
            ChosenPieceRef = EnemyAIScript.ChosenPiece;
            NextDecisionDelay = 0;

            //CheckSightlines();
            #region Action Delay
            ChooseAction();
            if (CurrentDelayTime > 0)
            {
                CurrentDelayTime -= Time.deltaTime;
                Debug.Log("The weight of the world      "+CurrentDelayTime);
                return NodeStateOptions.Running;
            }
            #endregion

            if (!ActionChosen)
            {
                RandomAction = Random.Range(0, 1);
                ActionChosen = true;
            }


            #region Moves the current selected Piece
            if (MovingPiece && CurrentDelayTime <= 0 && RandomAction == 0)  
            {
                MoveAction();
                return NodeStateOptions.Pass;
            }
            #endregion



            return NodeStateOptions.Pass;
        }

        return NodeStateOptions.Fail;
    }

    protected void CheckSightlines()
    {
        
        foreach (var PieceLocation in EnemyAIScript.ThisPlayerScript.EnemyPieces)
        {
            int HitobjectNumbers = 0;
            Vector2 RayDirection = ChosenPieceRef.transform.position - PieceLocation.transform.position;
            float Distance = Vector2.Distance(ChosenPieceRef.transform.position, PieceLocation.transform.position);

            RaycastHit2D[] HitData = Physics2D.RaycastAll(ChosenPieceRef.transform.position, RayDirection,Distance);
            foreach (var HitObject in HitData)
            {
                if (HitObject.collider.contactCaptureLayers.Equals("Break Wall"))
                {
                    HitobjectNumbers++;
                }
            }
        }
    }

    protected void UpdateMoveOptions()
    {
        MoveOptions.Clear();
        foreach (var Direction in EnemyAIScript.ChosenPiece.MainDirectionsClass)
        {
            if (Direction.AimDistance > 4)
            {
                MoveOptions.Add(Direction.Direction);
            }
        }
        ChosenPieceRef.Options = MoveOptions;
    }

    protected void ChooseAction()
    {
        Debug.Log("shink shink boom boom");
        if ((EnemyAIScript.LowHealthPiece && InSight) || Retreating)
        {
            
            TakeCover();
        }
        if(!Retreating)
        {
            Debug.Log("Ive unlocked it ");
            if(!CheckedDirections)
            {
                EnemyAIScript.ChosenPiece.CheckMainCardinalDirections();
                CheckedDirections = true;
            }
        }

        if (!EnemyAIScript.ChosenPiece.InStartingArea && !DirectionChosen)
        {
           
            switch (RandomAction)
            {
                case 0:

                    break;

                case 1:
                    break;
            }

            //MovingPiece = true;
        }
        

        if(!DirectionChosen)
        {

            MovingPiece = true;
        }
        
    }

    public void AttackAction()
    {

    }

    private void MoveAction()
    {
        CurrentDelayTime = 0.0f;
        UpdateMoveOptions();

        int MoveDirection = Random.Range(0, MoveOptions.Count);
        ChosenPieceRef.AIMovePiece(MoveOptions[MoveDirection]);
        EnemyAIScript.AIChangePieceState(false);

        DirectionChosen = true;
        MovingPiece = false;

        CurrentDelayTime = ChoiceDelayTime;
        NextDecisionDelayTime = NextDecisionDelay;
        ResetChoiceState();
    }

    private void ResetChoiceState()
    {
        CheckedDirections = false;
        Attacking = false;
        Retreating = false;
        InSight = false;
        DirectionChosen = false;
    }

    protected void CheckCollidingObjects()
    {
        
    }

    protected void TakeCover()
    {
        RaycastHit2D[] CirclesData = Physics2D.CircleCastAll(ChosenPieceRef.transform.position, 1.25f, Vector2.down, EnemyAIScript.InteractableLayers);
        float NewCoverDistance;
        Debug.Log("Negative");
        foreach (var IntersectPoint in CirclesData)
        {

            NewCoverDistance = Vector2.Distance(IntersectPoint.transform.position, ChosenPieceRef.transform.position);
            if (NewCoverDistance < CoverDistance) 
            {
                
                switch (NewCoverDistance)
                {
                    case < 0:
                        CoverDestination.x -= 1;
                        Debug.Log("Negative");
                    break;

                    case > 0:
                        CoverDestination.x += 1;
                        Debug.Log("Positive");
                    break;
                }
                CoverDistance = Mathf.Abs(NewCoverDistance);
                Debug.Log(CoverDestination);
            }
        }



    }
    public void OnDrawGizmos()
    {
        Debug.Log("Negative");
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector2.up, 1.25f);
    }
}

public class MoveProbabilities
{
    public string DirectionName;

    public Vector2 MoveDirection;

    int MaxChance = 10;
    int MinChance = 0;

}