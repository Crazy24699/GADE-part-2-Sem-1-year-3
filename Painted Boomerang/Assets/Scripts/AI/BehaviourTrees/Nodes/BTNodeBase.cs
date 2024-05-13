using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BTNodeBase
{

    public NodeStateOptions CurrentNodeState;

    public BTNodeBase RunStartup()
    {
        return this;
    }

    public virtual NodeStateOptions NodeLogicAndState()
    {
        return CurrentNodeState;
    }

}

public enum NodeStateOptions
{
    Running,
    Fail,
    Pass
}