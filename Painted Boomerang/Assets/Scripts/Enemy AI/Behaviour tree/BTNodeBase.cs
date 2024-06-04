
using UnityEngine;

[System.Serializable]
public class BTNodeBase
{


    public NodeStateOptions CurrentNodeState;

    public virtual NodeStateOptions RunNodeLogicAndStatus()
    {
        return CurrentNodeState;
    }

    

    public BTNodeBase RunStartup()
    {
        BehaviourTree BehaviourTreeScript;
        BehaviourTreeScript = GameObject.FindFirstObjectByType<BehaviourTree>();
        BehaviourTreeScript.AllChoiceOptions.Add(this);
        //Debug.Log("Smoking tail  ");
        return this;
    }


}
public enum NodeStateOptions
{
    Running,
    Fail,
    Pass
}