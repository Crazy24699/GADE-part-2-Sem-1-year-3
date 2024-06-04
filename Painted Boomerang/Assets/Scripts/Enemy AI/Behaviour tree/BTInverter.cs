using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTInverter : BTNodeBase
{
    protected BTNodeBase NodeBase;

    public void SetInverterValues(BTNodeBase NodeRef)
    {
        NodeBase = NodeRef;
    }

    public override NodeStateOptions RunNodeLogicAndStatus()
    {
        switch (NodeBase.RunNodeLogicAndStatus())
        {
            case NodeStateOptions.Running:
                CurrentNodeState=NodeStateOptions.Running;
                break;

            case NodeStateOptions.Fail:
                CurrentNodeState=NodeStateOptions.Fail;
                break;

            case NodeStateOptions.Pass:
                CurrentNodeState=NodeStateOptions.Pass;
                break;

            default:
                break;
        }
        return CurrentNodeState;
    }

}
