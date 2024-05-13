using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTChoosePiece : BTNodeBase
{
    protected AIFunctionality AIScriptRef;
    protected WorldHandler WorldHandlerScript;
    protected List<EntityBase> OwnPieces;
    protected List<EntityBase> EnemyPieces;

    public BTChoosePiece(GameObject AIRef)
    {
        AIScriptRef = AIRef.GetComponent<AIFunctionality>();

    }


    public void ComparePieces()
    {
        //compare every piece of the players to the enemies pieces
    }

}
