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
    protected bool InSight = false;
    protected bool HasSelectedPiece = false;

    //Scripts
    private EntityBase PieceSelected;

    //Floats


    public BTChoosePiece(GameObject EnemyAIRef)
    {
        WorldHandlerScript = GameObject.FindObjectOfType<WorldHandler>();
        EnemyAIScript = EnemyAIRef.GetComponent<EnemyAI>();

        RunStartup();
    }

    public override NodeStateOptions RunNodeLogicAndStatus()
    {
        PieceSelected = EnemyAIScript.ChosenPiece;
        if (PieceSelected == null)
        {
            //Debug.Log("what");
        }
            
        if(PieceSelected == null && EnemyAIScript.ThisPlayerScript)
        {
            switch (EnemyAIScript.GameStarted)
            {
                case true:
                    SelectPiece();
                    

                    break;

                case false:
                    int RandomPiece = Random.Range(0, EnemyAIScript.PiecesInPlay.Count);
                    PieceSelected = EnemyAIScript.PiecesInPlay[RandomPiece].GetComponent<EntityBase>();
                    EnemyAIScript.ChosenPiece = PieceSelected;
                    EnemyAIScript.ThisPlayerScript.SelectPiece(PieceSelected.gameObject);
                    EnemyAIScript.PieceSelected = true;
                    EnemyAIScript.GameStarted = true;

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
            //Debug.Log(EnemyAIScript.PiecesInPlay.Count);
            //Debug.Log("woop");
            //Debug.Log("Noop");

            EntityBase PieceScript = Piece.GetComponent<EntityBase>();

            if (PieceScript.CurrentHealth < PieceScript.CurrentHealth / 2)
            {
                Debug.Log("Noop");
                if (PieceSelected == null || PieceScript.CurrentHealth < PieceSelected.CurrentHealth)
                {
                    UpdatePieceData(PieceScript);
                }

            }

            if (PieceScript.CheckLOS() && ProgramManager.ProgramManagerInstance.HardmodeActive)
            {
                Debug.Log("woop");
            }

            if (PieceScript.InStartingArea && PieceSelected == null)
            {
                UpdatePieceData(PieceScript);
                //Debug.Log("Noop     " + PieceScript.gameObject.name);
                return;
            }
            else if (!PieceScript.InStartingArea && PieceSelected == null) 
            {
                int RandomPieceNumber = Random.Range(0, EnemyAIScript.PiecesInPlay.Count);
                PieceSelected = EnemyAIScript.PiecesInPlay[RandomPieceNumber].GetComponent<EntityBase>();
                Debug.Log(RandomPieceNumber);
                //PieceSelected = CheckPieceDistances();
                Debug.Log("making a sounds");
                UpdatePieceData(PieceSelected);
            }


        }
    }

    

    protected EntityBase CheckPieceDistances()
    {
        float MinPieceDistance = 0.0f;
        EntityBase BestOption = new EntityBase();
        foreach (var Piece in EnemyAIScript.PiecesInPlay)
        {
            Piece.GetComponent<EntityBase>().SelectAttackPiece();
            if (Piece.GetComponent<EntityBase>().EnemyPieceDistance > MinPieceDistance)
            {
                BestOption = Piece.GetComponent<EntityBase>();
                MinPieceDistance = Vector3.Distance(SelfRef.transform.position, Piece.transform.position);
            }
        }

        return BestOption;
    }


    public void UpdatePieceData(EntityBase PieceScript)
    {
        PieceSelected = PieceScript;
        EnemyAIScript.ChosenPiece = PieceSelected;
        EnemyAIScript.AIChangePieceState(true);
    }

}


