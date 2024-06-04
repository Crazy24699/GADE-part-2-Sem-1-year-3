using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNodeSelector : BTNodeBase
{

    protected List<BTNodeBase> AllNodes = new List<BTNodeBase>();

    public BTNodeSelector(List<BTNodeBase> NodeList)
    {
        AllNodes = NodeList;
        
    }

    public override NodeStateOptions RunNodeLogicAndStatus()
    {
        foreach (var Node in AllNodes)
        {
            switch (Node.RunNodeLogicAndStatus())
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
        }

        CurrentNodeState = NodeStateOptions.Fail;
        return CurrentNodeState;
    }


}
