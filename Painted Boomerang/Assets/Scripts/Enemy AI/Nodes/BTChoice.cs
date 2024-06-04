using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTChoice : BTNodeBase
{

    public EnemyAI EnemyAIScript;
    public WorldHandler WorldHandlerScript;
    public NavMeshAgent AgentRef;

    protected EntityBase ChosenPieceRef;

    //Bools
    protected bool Attacking;
    protected bool TakeCover;

    public BTChoice(GameObject EnemyAIRef)
    {
        WorldHandlerScript = GameObject.FindObjectOfType<WorldHandler>();
        EnemyAIScript = EnemyAIRef.GetComponent<EnemyAI>();

        RunStartup();
    }

    public override NodeStateOptions RunNodeLogicAndStatus()
    {
        if(EnemyAIScript.PieceSelected)
        {
            ChosenPieceRef = EnemyAIScript.ChosenPiece;
            return NodeStateOptions.Pass;
        }

        return NodeStateOptions.Fail;
    }

    protected void ChooseAction()
    {
        if(EnemyAIScript.LowHealthPiece)
        {

            return;
        }
        
        
    }

}
