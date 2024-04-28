using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PopulateGrid : MonoBehaviour
{
    public List<CellFunctionality> GridCells = new List<CellFunctionality>();
    public List<CellFunctionality> WallCells;

    public void PickWallSpots()
    {

        foreach (var Cell in GridCells)
        {
            int ChanceNumber = Random.Range(0, 10);
            
            if (ChanceNumber <= 3 && !Cell.IsBorderCell && !Cell.StartingCell)
            {
                Debug.Log("Picked");
                HandleWallObject(Cell);
            }
            if (!Cell.ObstacleNear)
            {

            }
        }
    }

    public void HandleWallObject(CellFunctionality BaseCell)
    {
        int WallLength = Random.Range(1, 4);

        //if the wall will be length or height long. 
        int WallType = Random.Range(1, 3);
        HashSet<CellFunctionality> PickedCells = new HashSet<CellFunctionality>();

        //if its a vertical wall, we dont really want it against the border of the map
        if (WallType == 2 && BaseCell.GetComponent<CellFunctionality>().IsBorderCell)
        {
            WallType = 1;
        }

        Debug.Log("Take a chance    " + WallType);
        Vector2Int InitialStartPoint;
        switch (WallType)
        {
            default:
            case 1:

                PickedCells.AddRange(BaseCell.NearCells.Where(NearCellLocation => NearCellLocation.Location.x == BaseCell.Location.x));
                PickedCells.Add(BaseCell);
                List<CellFunctionality> CellOptions = PickedCells.ToList();
                Debug.Log(PickedCells.Count +"  Picking "+BaseCell.NeighbourCells.Count);

                (InitialStartPoint) = WallLength > 1 ? new Vector2Int(BaseCell.Location.x - 1, BaseCell.Location.y) : new Vector2Int(BaseCell.Location.x, BaseCell.Location.y);

                for (int i = 0; i < WallLength ; i++)
                {
                    if (CellOptions[i].ObstacleNear)
                    {
                        return;
                    }

                }
                foreach (var CellScript in CellOptions)
                {
                    CellScript.ChangeCell(true);
                    CellScript.ObstacleNear = true;
                    foreach (var Cell in CellScript.NearCells)
                    {
                        Cell.ObstacleNear = true;
                    }
                }
                
                break;

            case 2:
                break;
        }


    }

}
