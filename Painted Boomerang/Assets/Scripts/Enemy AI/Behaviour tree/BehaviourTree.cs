using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : MonoBehaviour
{

    public BTNodeBase RootNode;

    public List<BTNodeBase> AllChoiceOptions;
    public GameObject EnemySelfRef;

    void Start()
    {
        EnemySelfRef = this.gameObject;
        
        CreateBehaviourTree();

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        RootNode.RunNodeLogicAndStatus();
    }

    public void CreateBehaviourTree()
    {
        BTDeadNode DieNode = new BTDeadNode(EnemySelfRef);
        BTAttackNode AttackEnemyNode = new BTAttackNode(EnemySelfRef);
        //add a defence node and maybe a script for it. It needs to activate when the AI scores a point or when the flags are inactive. 

        BTNodeSequence DeathSequence = new BTNodeSequence();
        BTNodeSequence GoForFlagSequence = new BTNodeSequence();
        BTNodeSequence DefendFlagSequence = new BTNodeSequence();
        BTNodeSequence AttackEnemySequence = new BTNodeSequence();

        DeathSequence.SetSequenceValues(new List<BTNodeBase> { DieNode });
        AttackEnemySequence.SetSequenceValues(new List<BTNodeBase> { AttackEnemyNode });

        RootNode = new BTNodeSelector(new List<BTNodeBase> {DeathSequence, GoForFlagSequence, DefendFlagSequence, AttackEnemySequence });


    }

   
}
