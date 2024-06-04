using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTDeadNode : BTNodeBase
{
    protected EnemyAI EnemyAIScript;
    public BTDeadNode(GameObject EnemyAIRef)
    {
        EnemyAIScript = EnemyAIRef.GetComponent<EnemyAI>();
    }

    public override NodeStateOptions RunNodeLogicAndStatus()
    {
        //if (EnemyAIScript.IsDead)
        //{
        //    Debug.Log("bulletproog");
        //    return NodeStateOptions.Running;
        //}
        Debug.Log("spirefire");
        return NodeStateOptions.Fail;
    }
}
