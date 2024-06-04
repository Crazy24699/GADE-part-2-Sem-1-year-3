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
    public NavMeshAgent AgentRef;

    public float Distance;
    protected float MissChange = 3.5f;

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

        if (Distance <= 2) 
        {
            Debug.Log("evil roams");
            ThrowingLogic();
            return NodeStateOptions.Running;
        }

        else if(EnemyAIScript.EnemyInRange) 
        {
            Debug.Log("Oh");
            switch (MissChange)         //Change this, its a placeholder
            {
                case <=5:
                    AgentRef.isStopped = true;
                    ThrowingLogic();
                    Debug.Log("red");
                    break;

                case > 5:
                    AgentRef.isStopped = false;
                    AgentRef.SetDestination(Target.transform.position);
                    Debug.Log("water dream");
                    ThrowingLogic();
                    break;
            }

            return NodeStateOptions.Running;
        }
        else
        {
            return NodeStateOptions.Fail;
        }
    }


    protected void ThrowingLogic()
    {
        if (EnemyAIScript.CanThrow && EnemyAIScript.EnemyInRange)
        {
            float MissOffset = Random.Range(MissChange * -1, MissChange);

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
    }


}
