using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFunctionality : MonoBehaviour
{

    public PlayerFunctionality PlayerFunctionality;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //The general functionality and thought process of the AI. What it handles and does
    protected void AIHandler()
    {
        ///
        ///picks a random piece
        ///chooses to move it or to use the boomerang
        ///if it chooses to throw the boomerang, it will choose a random angle in a 360 range if the player pieces are not in direct line of sight
        ///it will check constantly if there is a direct line of sight to the players pieces against every piece the ai has alive
        ///it will throw out 3 rays into the world with 5 bounces each, the end location of each ray will be compared to the location of each of the remaining pieces the player has
        ///and the nearest one will be chosen. If it is in a range that is ideal, it will choose to use the boomerange instead of moving
        ///if the range if too large, it will choose a direction to move in, it will favour being at least 1 cell away from any wall
        ///
    }

    public void CreateBehaviourTree()
    {

    }



}

