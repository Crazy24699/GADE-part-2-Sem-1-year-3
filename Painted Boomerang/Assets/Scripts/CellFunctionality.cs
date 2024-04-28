using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CellFunctionality : MonoBehaviour
{
    [SerializeField]private SpriteRenderer Sprite;

    public Vector2Int Location;
    public Vector2Int WorldLocation;

    public HashSet<Vector2Int> NeighbourCellsLocation = new HashSet<Vector2Int>();
    public HashSet<CellFunctionality> NeighbourCells = new HashSet<CellFunctionality>();
    public List<CellFunctionality> NearCells = new List<CellFunctionality>();

    public bool ObstacleNear = false;
    public bool IsBorderCell = false;
    public bool StartingCell = false;

    private WorldHandler WorldScript;

    public LayerMask HitLayers;
    // Start is called before the first frame update
    void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
        WorldScript = FindObjectOfType<WorldHandler>();
    }

    private void FixedUpdate()
    {
        HandleMouse();
        NearCells = NeighbourCells.ToList();
        //NearCells = WorldScript.AllCells.Where(Cell => Cell.Location == Location).ToList();
        //Debug.Log(WorldScript.AllCells.Where(Cell => Cell.Location == Location));
        WorldLocation = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
    }

    public void HandleMouse()
    {
        if (!GameManager.GameManagerInstance.GameStarted || !this.isActiveAndEnabled)
        {
            return;
        }


        RaycastHit2D RayHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, HitLayers);
        if (RayHit.collider != null && RayHit.collider.CompareTag("Cell") && RayHit.collider.name == this.name) 
        {
            //ChangeCell(true);
        }
        else
        {
            //ChangeCell(false);
        }
    }

    public void PopulateNearCellList(int AreaHeight, int AreaLength)
    {

        HashSet<Vector2Int> Cells = new HashSet<Vector2Int>
        {
            Location + Vector2Int.left,
            Location + Vector2Int.right,
            Location + Vector2Int.up,
            Location + Vector2Int.down
        };

        foreach (var CellCord in Cells)
        {

            
            if (CellCord.x >= 0 && CellCord.y >= 0 && CellCord.y <= AreaHeight && CellCord.x <= AreaLength)
            {

                NeighbourCells.AddRange(WorldScript.AllCells.Where(Cell => Cell.Location == CellCord));
                //Debug.Log(WorldScript.AllCells.Where(Cell => Cell.Location == Location).ToString());
                //Debug.Log("Sharp and clean  ");
                NeighbourCellsLocation.Add(CellCord);
                Debug.Log(CellCord);
            }
            
        }
    }

    
    public void ChangeColour()
    {
        Sprite.color = Color.black ;
    }

    public void ChangeCell(bool HighlightCell)
    {
        switch (HighlightCell)
        {
            case true:
                Sprite.color = Color.blue;
                //Sprite.enabled = false;
                break;

            case false:
                Sprite.color = Color.white;
                break;
        }
    }

    private void OnMouseEnter()
    {

        
    }

    private void OnMouseExit()
    {
        if (!GameManager.GameManagerInstance.GameStarted || !this.isActiveAndEnabled)
        {
            return;
        }
        
    }

}
