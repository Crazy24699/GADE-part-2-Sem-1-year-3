using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFunctionality : MonoBehaviour
{
    [Space(5), Header(" ")]
    public GameObject SelectedEntity;
    public GameObject BoomerangObject;

    [Space(5), Header(" ")]
    public int MovesRemaining = 0;

    [Space(5), Header(" ")]
    public Vector2Int CellCursorLocation;
    public Vector2 ActiveCellLocation;

    [Space(5), Header(" ")]
    public LayerMask ExclusionLayers;
    public LayerMask SelectableLayers;


    [Space(5), Header(" ")]
    public bool TurnActive;
    public bool CanPerformAction;
    public bool InstantBreak;
    public bool AIControlled;
    public bool StartupRan;

    [Space(5), Header(" ")]
    public List<EntityBase> OwnPieces;
    public List<EntityBase> EnemyPieces;
    public Teams ThisTeam;
    public WorldHandler WorldHandlerScript;

    [Space(5), Header(" ")]
    public Image BoomerangIcon;
    public Image MoveIcon;
    public RectTransform TrackedIcon;
    public TextMeshProUGUI TurnsText;

    public void Startup()
    {
        WorldHandlerScript = GameObject.FindObjectOfType<WorldHandler>();
        TurnsText.text = "";
        if (AIControlled)
        {
            EnemyAI EnemyAIScript = GameObject.FindObjectOfType<EnemyAI>();
            EnemyAIScript.AssignedTeam = ThisTeam;
            EnemyAIScript.AIStartup();
        }
        int NameChangeIndex = 0;
        foreach (var Piece in OwnPieces)
        {
            Piece.Startup();
            Piece.name = Piece.name + NameChangeIndex;
            Piece.EnableCollider(Piece.gameObject);
            Piece.AdvancedMover = false;
            if(NameChangeIndex==0 || NameChangeIndex == 1)
            {
                Piece.AdvancedMover = true;
            }
            NameChangeIndex++;
        }

        HashSet<EntityBase> AllPieces = new HashSet<EntityBase>();
        AllPieces = GameObject.FindObjectsByType<EntityBase>(FindObjectsSortMode.None).ToHashSet();
        foreach (var Piece in AllPieces)
        {
            if (Piece.AssignedTeam != ThisTeam)
            {
                EnemyPieces.Add(Piece);
            }
        }

        StartupRan = true;
    }

    public void TurnStart()
    {
        WorldHandlerScript.ActivatedCell.transform.position = ActiveCellLocation;
        //Debug.Log("Went");
        foreach (EntityBase Entity in OwnPieces)
        {
            //Debug.Log("Active");
            Entity.gameObject.GetComponent<Collider2D>().excludeLayers = ExclusionLayers;
        }
    }

    private void Update()
    {
        if (!TurnActive || !StartupRan)
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
            foreach (EntityBase Entity in OwnPieces)
            {
                Entity.gameObject.GetComponent<Collider2D>().excludeLayers = new LayerMask();
                SelectedEntity = null;
            }
            WorldHandlerScript.SetActivePlayer(ThisTeam);

        }
    }

    public void CheckPieces()
    {
        if(OwnPieces.Count == 0)
        {
            WorldHandlerScript.EndGame(ThisTeam);
        }
    }

    public void MouseFunctionalty()
    {

        if (Input.GetMouseButtonDown(1))
        {
            DeselectPiece();
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
                ThrowBoomerang(RayHit.point);
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

    public void DeselectPiece()
    {
        if (SelectedEntity != null)
        {
            SelectedEntity.GetComponent<EntityBase>().RevertColour();
            SelectedEntity.layer = LayerMask.NameToLayer("Pieces");
            SelectedEntity = null;
        }
    }

    public void SelectPiece(GameObject SelectedPiece)
    {
        SelectedEntity = SelectedPiece;
        SelectedEntity.GetComponent<SpriteRenderer>().color = Color.yellow;
        SelectedEntity.layer = LayerMask.NameToLayer("ActivePiece");
    }

    public void MoveSelection(Collider2D HitCollider)
    {
        if (HitCollider.CompareTag("Entity") && HitCollider.GetComponent<EntityBase>().AssignedTeam == ThisTeam)
        {
            SelectPiece(HitCollider.gameObject);
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
                DeselectPiece();
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

    public void ThrowBoomerang(Vector2 AimPoint)
    {

        Vector2 MoveDirection = (AimPoint - new Vector2(SelectedEntity.transform.position.x, SelectedEntity.transform.position.y)).normalized;
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
        Debug.Log("share the truth");
        SelectedEntity.GetComponent<EntityBase>().RevertColour();
        SelectedEntity = null;

        //SelectedEntity = null;
    }

}
