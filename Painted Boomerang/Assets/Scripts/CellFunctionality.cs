using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CellFunctionality : MonoBehaviour
{
    [SerializeField]private SpriteRenderer Sprite;

    public Vector2Int Location;
    public Vector2Int WorldLocation;

    public HashSet<Vector2Int> NeighbourCells = new HashSet<Vector2Int>();
    public List<Vector2Int> NearCells = new List<Vector2Int>();

    

    public LayerMask HitLayers;
    // Start is called before the first frame update
    void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();

    }

    private void FixedUpdate()
    {
        HandleMouse();
        NearCells = NeighbourCells.ToListPooled();
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
            ChangeCell(true);
        }
        else
        {
            ChangeCell(false);
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
                NeighbourCells.Add(CellCord);
                //Debug.Log(CellCord);
            }

        }
    }

    public void ChangeCell(bool HighlightCell)
    {
        switch (HighlightCell)
        {
            case true:
                Sprite.color = Color.blue;
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
