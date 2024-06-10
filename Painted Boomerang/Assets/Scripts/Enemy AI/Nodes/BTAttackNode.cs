using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class BTAttackNode : BTNodeBase
{
    public EnemyAI EnemyAIScript;
    public WorldHandler WorldHandlerScript;

    public float Distance;
    protected float MissChance = 3.5f;

    protected bool CanAttack=false;
    protected bool AimPointsUpdated = false;

    public Vector3 Direciton;

    public List<GameObject> AllTargets = new List<GameObject>();

    public GameObject Target;
    public GameObject SelfRef;

    protected Transform ThrowDirection;
    protected Vector2 AimingPoint;        //Where the AI will aim to throw the boomerange

    public BTAttackNode(GameObject EnemyAIRef)
    {
        WorldHandlerScript = GameObject.FindObjectOfType<WorldHandler>();
        EnemyAIScript = EnemyAIRef.GetComponent<EnemyAI>();

        RunStartup();
    }

    public override NodeStateOptions RunNodeLogicAndStatus()
    {

        if (!EnemyAIScript.AttackingActive)
        {
            return NodeStateOptions.Fail;
        }

        if(!AimPointsUpdated)
        {
            GetAimPoints();
        }

        if (EnemyAIScript.ThisPlayerScript.TurnActive && EnemyAIScript.ThisPlayerScript.MovesRemaining <= 0) 
        {
            AimPointsUpdated = false;
            return NodeStateOptions.Fail;
        }

        if(AimPointsUpdated && CanAttack && EnemyAIScript.AttackingActive)
        {
            ThrowingLogic();
        }
        if (Distance >= 2 && CanAttack) 
        {
            Debug.Log("evil roams");
            ThrowingLogic();
            return NodeStateOptions.Running;
        }

        else if(EnemyAIScript.EnemyInRange && CanAttack) 
        {
            Debug.Log("Oh");
            switch (MissChance)         //Change this, its a placeholder
            {
                case <=5:
                    ThrowingLogic();
                    Debug.Log("red");
                    break;

                case > 5:
                    //Debug.Log("water dream");
                    ThrowingLogic();
                    break;
            }

            return NodeStateOptions.Running;
        }
        else
        {
            //Debug.Log("water dream");
            return NodeStateOptions.Fail;
        }
    }

    protected void GetAimPoints()
    {
        EnemyAIScript.ChosenPiece.SetRandomAttackPositions();
        int RandomAttackOption = Random.Range(0, EnemyAIScript.ChosenPiece.AttackPointOptions.Count);
        AttackOptions AttackOptionsClass = EnemyAIScript.ChosenPiece.AttackPointOptions[RandomAttackOption];
        Vector2 AimPosition = new Vector2(AttackOptionsClass.XPosition, AttackOptionsClass.YPosition);
        AimingPoint = AimPosition;

        AimPointsUpdated = true;
        CanAttack = true;
    }

    protected void ThrowingLogic()
    {
        if (EnemyAIScript.CanThrow && EnemyAIScript.EnemyInRange)
        {
            float MissOffset = Random.Range(MissChance * -1, MissChance);

            int MissOffsetAxis = Random.Range(0, 3);
            Vector2 ChangeVector = Vector2.zero;
            switch (MissOffsetAxis)
            {
                case 0:
                    ChangeVector.x = MissOffset;
                    break;

                case 1:
                    ChangeVector.y = MissOffset;
                    break;

                case 2:
                    ChangeVector *= MissOffset;
                    break;
            }

            AimingPoint += ChangeVector;
        }
        EnemyAIScript.ThisPlayerScript.ThrowBoomerang(AimingPoint);
        EnemyAIScript.AttackingActive = false;
        EnemyAIScript.ThisPlayerScript.CanPerformAction = false;
        CanAttack = false;
    }


}
