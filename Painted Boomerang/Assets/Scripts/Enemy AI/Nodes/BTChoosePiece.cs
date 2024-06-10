using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BTChoosePiece : BTNodeBase
{
    public EnemyAI EnemyAIScript;
    public WorldHandler WorldHandlerScript;
    public NavMeshAgent AgentRef;

    //GameObjects
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

        if (PieceSelected == null && EnemyAIScript.ThisPlayerScript && EnemyAIScript.GameStarted)
        {
            switch (ProgramManager.ProgramManagerInstance.ChosenDifficulty)
            {
               
                default:
                case ProgramManager.DifficultyOptions.Easy:
                    SelectPiece();
                    break;
                
                
                case ProgramManager.DifficultyOptions.Hard:
                    AdvancedPieceSelection();
                    break;
                    
            }
            return NodeStateOptions.Running;
        }
        else if (!EnemyAIScript.GameStarted)
        {
            SelectRandomPiece();
            return NodeStateOptions.Pass;
        }


        //if(PieceSelected == null && EnemyAIScript.ThisPlayerScript)
        //{
        //    switch (EnemyAIScript.GameStarted)
        //    {
        //        case true:
        //            SelectPiece();

        //            break;

        //        case false:
        //            SelectRandomPiece();

        //            break;


        //    }
        //    return NodeStateOptions.Pass;
        //}
        
        
        return NodeStateOptions.Fail;
    }



    public void SelectRandomPiece()
    {
        int RandomPiece = Random.Range(0, EnemyAIScript.PiecesInPlay.Count);
        PieceSelected = EnemyAIScript.PiecesInPlay[RandomPiece].GetComponent<EntityBase>();

        EnemyAIScript.ChosenPiece = PieceSelected;
        EnemyAIScript.ThisPlayerScript.SelectPiece(PieceSelected.gameObject);

        EnemyAIScript.PieceSelected = true;
        EnemyAIScript.GameStarted = true;
    }

    protected void AdvancedPieceSelection()
    {
        EntityBase ChosenPiece;
        ChosenPiece = CheckPieceDistances();


    }

    public void SelectPiece()
    {
        
        foreach (var Piece in EnemyAIScript.PiecesInPlay)
        {

            EntityBase PieceScript = Piece.GetComponent<EntityBase>();

            if (PieceScript.CurrentHealth < PieceScript.CurrentHealth / 2)
            {
                //Debug.Log("Noop");
                if (PieceSelected == null || PieceScript.CurrentHealth < PieceSelected.CurrentHealth)
                {
                    UpdatePieceData(PieceScript);
                }

            }

            if (PieceScript.CheckLOS() && ProgramManager.ProgramManagerInstance.HardmodeActive)
            {
                //Debug.Log("woop");
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

    protected void CheckPieceDanger()
    {

    }

    public void UpdatePieceData(EntityBase PieceScript)
    {
        PieceSelected = PieceScript;
        EnemyAIScript.ChosenPiece = PieceSelected;
        EnemyAIScript.AIChangePieceState(true);
    }

}


