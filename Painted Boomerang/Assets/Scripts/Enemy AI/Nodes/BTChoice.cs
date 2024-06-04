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

    //Vectors
    protected Vector2 CoverDestination;

    //Floats
    protected float CoverDistance;

    //Bools
    protected bool Attacking;
    protected bool Retreating;

    public BTChoice(GameObject EnemyAIRef)
    {
        WorldHandlerScript = GameObject.FindObjectOfType<WorldHandler>();
        EnemyAIScript = EnemyAIRef.GetComponent<EnemyAI>();

        RunStartup();
    }

    public override NodeStateOptions RunNodeLogicAndStatus()
    {

        if(Input.GetKeyDown(KeyCode.T))
        {
            EnemyAIScript.PieceSelected = true;
            Retreating = true;
        }

        if(EnemyAIScript.PieceSelected)
        {
            ChosenPieceRef = EnemyAIScript.ChosenPiece;
            Debug.Log("Negative");
            ChooseAction();

            return NodeStateOptions.Pass;
        }

        return NodeStateOptions.Fail;
    }

    protected void ChooseAction()
    {
        if((EnemyAIScript.LowHealthPiece && Retreating) || Retreating)
        {
            TakeCover();
            return;
        }
        
        
    }

    protected void TakeCover()
    {
        RaycastHit2D[] CirclesData = Physics2D.CircleCastAll(ChosenPieceRef.transform.position, 1.25f, Vector2.down, EnemyAIScript.InteractableLayers);
        float NewCoverDistance;
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
    void OnDrawGizmos()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(ChosenPieceRef.transform.position, 1.25f);
    }
}
