using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFunctionality : MonoBehaviour
{
    public GameObject SelectedEntity;
    public GameObject BoomerangObject;


    public int MovesRemaining = 0;
    public Vector2Int CellCursorLocation;
    public Vector2 ActiveCellLocation;

    public LayerMask ExclusionLayers;
    public LayerMask SelectableLayers;

    public List<EntityBase> Entities;

    public bool TurnActive;
    public bool CanPerformAction;
    public bool InstantBreak;
    public bool AIControlled;

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
        TurnsText.text = "";
    }

    public void TurnStart()
    {
        WorldHandlerScript.ActivatedCell.transform.position = ActiveCellLocation;
        Debug.Log("Went");
        foreach (EntityBase Entity in Entities)
        {
            Debug.Log("Active");
            Entity.gameObject.GetComponent<Collider2D>().excludeLayers = ExclusionLayers;
        }
    }

    private void Update()
    {
        if (!TurnActive)
        {
            return;
        }
        TurnsText.text = $"It is {ThisTeam}'s turn and have {MovesRemaining} moves remaining";

        if (!AIControlled)
        {
            MouseFunctionalty();

        }

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
            foreach (EntityBase Entity in Entities)
            {
                Entity.gameObject.GetComponent<Collider2D>().excludeLayers = new LayerMask();
                SelectedEntity = null;
            }
            WorldHandlerScript.SetActivePlayer(ThisTeam);

        }
    }

    public void CheckPieces()
    {
        if(Entities.Count == 0)
        {
            WorldHandlerScript.EndGame(ThisTeam);
        }
    }

    public void MouseFunctionalty()
    {

        if (Input.GetMouseButtonDown(1))
        {
            if (SelectedEntity != null)
            {
                SelectedEntity.GetComponent<EntityBase>().RevertColour();
                SelectedEntity = null;
            }

        }

        RaycastHit2D RayHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, SelectableLayers);
        Collider2D HitCollider = RayHit.collider;

        if (HitCollider == null)
        {
            //Debug.Log("Hit");
            return;
        }
        
        if (HitCollider.CompareTag("Cell"))
        {
            CellCursorLocation = HitCollider.gameObject.GetComponent<CellFunctionality>().Location;
        }

        if (Input.GetMouseButtonDown(0) && CanPerformAction) 
        {
            MoveSelection(HitCollider);

            if(SelectedEntity != null && SelectedEntity.GetComponent<EntityBase>().MouseDistance>=8)
            {
                ThrowBoomerang(RayHit);
                CanPerformAction = false;
            }
        }
        if(SelectedEntity != null)
        {
            EntityBase SelectedEntityBase = SelectedEntity.GetComponent<EntityBase>();
            switch (SelectedEntityBase.MouseDistance)
            {
                case < 8:
                    TrackedIcon.GetComponent<Image>().sprite= MoveIcon.sprite;
                    break;

                case >= 8:
                    TrackedIcon.GetComponent<Image>().sprite = BoomerangIcon.sprite;
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
            //Debug.Log("entity");
        }
        else if (HitCollider.gameObject.CompareTag("Cell") && SelectedEntity != null)
        {
            HitCollider.gameObject.GetComponent<CellFunctionality>().enabled = true;
            CellFunctionality CellScript = HitCollider.GetComponent<CellFunctionality>();
            EntityBase EntityScript = SelectedEntity.GetComponent<EntityBase>();

            if (CellScript.Populated)
            {

                return;
            }

            if (CellScript.NeighbourCellsLocation.Contains(SelectedEntity.GetComponent<EntityBase>().CurrentPosition))
            {
                EntityScript.MoveEntity(new Vector2Int(CellScript.WorldLocation.x, CellScript.WorldLocation.y));
                MovesRemaining--;
                SelectedEntity.GetComponent<EntityBase>().RevertColour();
                SelectedEntity = null;
            }
            else
            {
                foreach (var Cell in CellScript.NeighbourCellsLocation)
                {
                    //Debug.Log($"Neighbour cells {Cell}");
                }
            }

        }
    }

    public void ThrowBoomerang(RaycastHit2D RayHitRef)
    {
        Debug.Log("share the truth");

        Vector2 MoveDirection = (RayHitRef.point - new Vector2(SelectedEntity.transform.position.x, SelectedEntity.transform.position.y)).normalized;
        GameObject SpawnedBoomerang= Instantiate(BoomerangObject, SelectedEntity.transform.position, SelectedEntity.transform.rotation);
        SpawnedBoomerang.GetComponent<Rigidbody2D>().velocity = MoveDirection*25;
        BoomerangFunctionality BoomerangeScriptRef=SpawnedBoomerang.GetComponent<BoomerangFunctionality>();
        BoomerangeScriptRef.ParentEntity = SelectedEntity;
        BoomerangeScriptRef.ThisTeam = ThisTeam;
        BoomerangeScriptRef.PlayerParent = this;
        if (InstantBreak)
        {
            BoomerangeScriptRef.Damage = 100;
            BoomerangeScriptRef.InstantBreak = true;
        }
        SelectedEntity.GetComponent<EntityBase>().RevertColour();
        SelectedEntity = null;

        //SelectedEntity = null;
    }

}
