using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BTNodeSequence : BTNodeBase
{

    public NodeStateOptions CurrentNodeStatus;

    protected List<BTNodeBase> AllNodes = new List<BTNodeBase>();


    //Yes i know that this is essentually a constructor by a different name. 
    //But I dont want to use any real constructors and with this i can actually
    //make it do more things in the future and not just a constructor
    public void SetSequenceValues(List<BTNodeBase> NodeList)
    {
        AllNodes = NodeList;
    }

    //I specifically used a virtual modifier because the 
    //abstract modifier is a waste of time and characters typed. 
    public override NodeStateOptions RunNodeLogicAndStatus()
    {
        bool AnyActiveNodes = false;
        foreach (var Node in AllNodes)
        {
            switch (Node.RunNodeLogicAndStatus())
            {
                case NodeStateOptions.Running:
                    AnyActiveNodes = true; 
                    break;

                case NodeStateOptions.Pass:
                    break;

                case NodeStateOptions.Fail:
                    CurrentNodeState = NodeStateOptions.Fail;
                    break;
            }
        }

        CurrentNodeState = AnyActiveNodes ? NodeStateOptions.Running : NodeStateOptions.Pass;
        return CurrentNodeState;
    }

}
