using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : MonoBehaviour
{

    public BTNodeBase RootNode;

    public List<BTNodeBase> AllChoiceOptions;
    public GameObject EnemySelfRef;

    public bool StartupRan;

    public void BehaviourTreeStartup()
    {
        EnemySelfRef = this.gameObject;

        CreateBehaviourTree();
        StartupRan = true;
        Debug.Log("HAHAHAHAHA       ");
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(!StartupRan)
        {
            return;
        }
        RootNode.RunNodeLogicAndStatus();
    }

    public void CreateBehaviourTree()
    {
        BTDeadNode DieNode = new BTDeadNode(EnemySelfRef);
        BTAttackNode AttackEnemyNode = new BTAttackNode(EnemySelfRef);
        BTChoosePiece ChoosePieceNode = new BTChoosePiece(EnemySelfRef);
        BTChoice ChoiceNode = new BTChoice(EnemySelfRef);
        //add a defence node and maybe a script for it. It needs to activate when the AI scores a point or when the flags are inactive. 

        BTNodeSequence DeathSequence = new BTNodeSequence();
        BTNodeSequence ChooseMoveSequence = new BTNodeSequence();
        BTNodeSequence AttackEnemySequence = new BTNodeSequence();

        DeathSequence.SetSequenceValues(new List<BTNodeBase> { DieNode });
        AttackEnemySequence.SetSequenceValues(new List<BTNodeBase> { AttackEnemyNode });
        ChooseMoveSequence.SetSequenceValues(new List<BTNodeBase> { ChoosePieceNode, ChoiceNode });

        RootNode = new BTNodeSelector(new List<BTNodeBase> { DeathSequence, ChooseMoveSequence, AttackEnemySequence });


    }

   
}
