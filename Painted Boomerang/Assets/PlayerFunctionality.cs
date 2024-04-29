using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFunctionality : MonoBehaviour
{
    public GameObject SelectedEntity;

    public int MovesRemaining = 0;
    public Vector2Int CellCursorLocation;

    public LayerMask SelectableLayers;

    public List<EntityBase> Entities;

    public bool TurnActive;

    public WorldHandler.Teams ThisTeam;
    public WorldHandler WorldHandlerScript;
    public TextMeshProUGUI TurnsText;

    public Image BoomerangIcon;
    public Image MoveIcon;
    public RectTransform TrackedIcon;

    // Start is called before the first frame update
    void Start()
    {
        WorldHandlerScript = GameObject.FindObjectOfType<WorldHandler>();

    }

    private void Update()
    {
        if (!TurnActive)
        {
            return;
        }
        MouseFunctionalty();
        if (MovesRemaining <= 0)
        {
            EndTurn();
        }
    }

    public void EndTurn()
    {
        if (MovesRemaining <= 0) 
        {
            MovesRemaining = 0;
            WorldHandlerScript.SetActivePlayer(ThisTeam);

        }
    }

    public void MouseFunctionalty()
    {

        if (Input.GetMouseButtonDown(1))
        {
            if (SelectedEntity != null)
            {
                SelectedEntity.GetComponent<SpriteRenderer>().color = Color.white;
                SelectedEntity = null;
            }

        }

        RaycastHit2D RayHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, SelectableLayers);
        Collider2D HitCollider = RayHit.collider;

        if (HitCollider == null)
        {
            Debug.Log("Hit");
            return;
        }
        
        if (HitCollider.CompareTag("Cell"))
        {
            CellCursorLocation = HitCollider.gameObject.GetComponent<CellFunctionality>().Location;
        }

        if (Input.GetMouseButtonDown(0)) 
        {

            MoveSelection(HitCollider);
            
            if(SelectedEntity != null && SelectedEntity.GetComponent<EntityBase>().MouseDistance>=8)
            {
                ThrowBoomerang();
            }
        }
        if(SelectedEntity != null)
        {
            EntityBase SelectedEntityBase = SelectedEntity.GetComponent<EntityBase>();
            switch (SelectedEntityBase.MouseDistance)
            {
                case < 8:
                    TrackedIcon.GetComponent<Image>().color = MoveIcon.color;
                    break;

                case >= 8:
                    TrackedIcon.GetComponent<Image>().color = BoomerangIcon.color;
                    break;
            }

        }
        TrackedIcon.transform.position = Input.mousePosition;
    }

    public void MoveSelection(Collider2D HitCollider)
    {
        if (HitCollider.CompareTag("Entity") && HitCollider.GetComponent<EntityBase>().AssignedTeam == ThisTeam)
        {
            SelectedEntity = HitCollider.gameObject;
            SelectedEntity.GetComponent<SpriteRenderer>().color = Color.yellow;
            Debug.Log("entity");
        }
        else if (HitCollider.gameObject.CompareTag("Cell") && SelectedEntity != null)
        {
            HitCollider.gameObject.GetComponent<CellFunctionality>().enabled = true;
            CellFunctionality CellScript = HitCollider.GetComponent<CellFunctionality>();
            EntityBase EntityScript = SelectedEntity.GetComponent<EntityBase>();

            if (CellScript.ContainsEntity)
            {

                return;
            }

            if (CellScript.NeighbourCellsLocation.Contains(SelectedEntity.GetComponent<EntityBase>().CurrentPosition))
            {
                EntityScript.MoveEntity(new Vector2Int(CellScript.WorldLocation.x, CellScript.WorldLocation.y));
                MovesRemaining--;
                SelectedEntity.GetComponent<SpriteRenderer>().color = Color.white;
                SelectedEntity = null;
            }
            else
            {
                foreach (var Cell in CellScript.NeighbourCellsLocation)
                {
                    Debug.Log($"Neighbour cells {Cell}");
                }
            }

        }
    }

    public void ThrowBoomerang()
    {
        Debug.Log("share the truth");


        //SelectedEntity = null;
    }

}
