using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTChoosePiece : BTNodeBase
{
    public EnemyAI EnemyAIScript;
    public WorldHandler WorldHandlerScript;
    public NavMeshAgent AgentRef;

    //GameObj
    public GameObject Target;
    public GameObject SelfRef;

    //Bools
    protected bool HasSelectedPiece = false;

    //Gameobejcts
    public EntityBase PieceSelected;


    public BTChoosePiece(GameObject EnemyAIRef)
    {
        WorldHandlerScript = GameObject.FindObjectOfType<WorldHandler>();
        EnemyAIScript = EnemyAIRef.GetComponent<EnemyAI>();

        RunStartup();
    }

    public override NodeStateOptions RunNodeLogicAndStatus()
    {
        if(PieceSelected == null)
        {
            switch (EnemyAIScript.GameStarted)
            {
                case true:
                    SelectPiece();
                    Debug.Log("woop");
                    break;

                case false:
                    int RandomPiece = Random.Range(0, EnemyAIScript.PiecesInPlay.Count);
                    PieceSelected = EnemyAIScript.PiecesInPlay[RandomPiece].GetComponent<EntityBase>();
                    PieceSelected.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
                    EnemyAIScript.ChosenPiece = PieceSelected;
                    EnemyAIScript.PieceSelected = true;
                    Debug.Log("Noop");
                    break;


            }
            return NodeStateOptions.Pass;
        }
        
        
        return NodeStateOptions.Fail;
    }

    

    public void SelectPiece()
    {
        foreach (var Piece in EnemyAIScript.PiecesInPlay)
        {
            EntityBase PieceScript = Piece.GetComponent<EntityBase>();
            if (PieceScript.CurrentHealth < PieceScript.CurrentHealth / 2)
            {

                if (PieceSelected == null || PieceScript.CurrentHealth < PieceSelected.CurrentHealth)
                {
                    PieceSelected = PieceScript;
                    EnemyAIScript.ChosenPiece = PieceSelected;
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
