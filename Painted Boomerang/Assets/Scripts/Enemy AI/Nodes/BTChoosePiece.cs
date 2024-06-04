using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTChoosePiece : BTNodeBase
{
    public EnemyAI EnemyAIScript;
    public WorldHandler WorldHandlerScript;
    public NavMeshAgent AgentRef;

    public GameObject Target;
    public GameObject SelfRef;

    //Bools
    protected bool PieceSelected = false;

    //Gameobejcts
    public EntityBase SelectedPiece;


    public BTChoosePiece(GameObject EnemyAIRef)
    {
        WorldHandlerScript = GameObject.FindObjectOfType<WorldHandler>();
        EnemyAIScript = EnemyAIRef.GetComponent<EnemyAI>();

        RunStartup();
    }

    public override NodeStateOptions RunNodeLogicAndStatus()
    {

        return NodeStateOptions.Running;
    }

    public void SelectPiece()
    {
        foreach (var Piece in EnemyAIScript.PiecesInPlay)
        {
            EntityBase PieceScript = Piece.GetComponent<EntityBase>();
            if (PieceScript.CurrentHealth < PieceScript.CurrentHealth / 2)
            {

                if (SelectedPiece == null || PieceScript.CurrentHealth < SelectedPiece.CurrentHealth)
                {
                    SelectedPiece = PieceScript;
                    EnemyAIScript.ChosenPiece = SelectedPiece;
                    EnemyAIScript.PieceSelected = true;
                    return;
                }

            }
            else
            {

            }

            
        }
    }


    public void UpdatePieceData()
    {
        foreach (var Piece in EnemyAIScript.PiecesInPlay)
        {
            Piece.GetComponent<EntityBase>().CheckMainCardinalDirections();
        }
    }

}
