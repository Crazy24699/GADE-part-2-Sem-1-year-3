using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class WorldHandler : MonoBehaviour
{
    const int AreaLength = 64;
    const int AreaHeight = 32;
    private int AreaCellHeight;
    private int AreaCellLength;
    protected int WorldCellSize;

    public GameObject TilePrefab;
    public GameObject LoadingScreenPanel;
    public GameObject SelectedEntity;

    public string CurrentTag;

    public Vector2Int CellCursorLocation;

    public GameManager GameManagerScript;

    public LayerMask SelectableLayers;

    public enum Teams
    {
        Team1,
        Team2
    }
    public Teams CurrentTeam;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManagerScript != null)
        {
            return;
        }

        GameManagerScript = GameManager.GameManagerInstance;

        WorldCellSize = 4;

        AreaCellHeight = AreaHeight / WorldCellSize;
        AreaCellLength = AreaLength / WorldCellSize;

        StartCoroutine(GenerateGrid());
        CurrentTeam = Teams.Team1;
    }

    public IEnumerator GenerateGrid()
    {

        for (int xCord = 0; xCord < AreaLength/ WorldCellSize; xCord++)
        {

            for (int yCord = 0; yCord < AreaHeight/WorldCellSize; yCord++)
            {
                yield return new WaitForSeconds(0.02f);
                GameObject SpawnedObject = Instantiate(TilePrefab, new Vector3(xCord*4, yCord+10), Quaternion.identity);
                SpawnedObject.name = $"Tile: x:{xCord} y:{yCord}";
                SpawnedObject.transform.SetParent(GameObject.Find("PlayableGrid").transform);
                SpawnedObject.transform.position = new Vector3((xCord * 4) - (AreaLength / 2) + 2f, (yCord * 4) - (AreaHeight / 2) + 2f);
                if(yCord == 0 || yCord == AreaHeight / WorldCellSize-1 || xCord==0 || xCord== AreaLength / WorldCellSize-1)
                {
                    SpawnedObject.name = $"Tile: x:{xCord} y:{yCord} First last";
                }
                SpawnedObject.GetComponent<CellFunctionality>().Location = new Vector2Int(xCord, yCord);
                InitialStartArea(SpawnedObject, xCord, yCord);
            }
            
        }
    }

    protected void InitialStartArea(GameObject SpawnedObjectRef, int xCordRef, int yCordRef)
    {

        if ((yCordRef == (AreaCellHeight / 2) - 1 || yCordRef == AreaCellHeight/2))
        {

            switch (xCordRef < AreaCellLength / 2)
            {

                case true:
                    if (xCordRef > 0 && xCordRef < 3)
                    {
                        SpawnedObjectRef.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                    break;

                case false:
                    if (xCordRef > AreaCellLength - 4 && xCordRef < AreaCellLength - 1)
                    {
                        SpawnedObjectRef.GetComponent<SpriteRenderer>().color = Color.green;
                    }
                    break;
            }
            //Debug.Log($"X cord: {xCordRef} {AreaCellLength}");
        }
    }

    protected void PlaceObstructions(int xCordRef, int yCordRef)
    {
        
    }

    private void Update()
    {
        MouseFunctionalty();
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
        CurrentTag = HitCollider.tag;
        if (HitCollider.CompareTag("Cell"))
        {
            CellCursorLocation = RayHit.collider.gameObject.GetComponent<CellFunctionality>().Location;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (HitCollider.CompareTag("Entity") && HitCollider.GetComponent<EntityBase>().TeamName == CurrentTeam.ToString())
            {
                SelectedEntity = RayHit.collider.gameObject;
                SelectedEntity.GetComponent<SpriteRenderer>().color = Color.yellow;
                Debug.Log("entity");
            }
            else if (HitCollider.gameObject.CompareTag("Cell") && SelectedEntity != null)
            {
                HitCollider.gameObject.GetComponent<CellFunctionality>().enabled = true;
                HitCollider.GetComponent<CellFunctionality>().PopulateNearCellList(AreaCellHeight, AreaCellLength);
                CellFunctionality CellScript = HitCollider.GetComponent<CellFunctionality>();
                
                if (CellScript.NeighbourCells == null)
                {
                    Debug.Log("nia");
                    CellScript.PopulateNearCellList(AreaCellHeight, AreaCellLength);
                }

                if (CellScript.NeighbourCells.Contains(SelectedEntity.GetComponent<EntityBase>().CurrentPosition))
                {
                    Debug.Log("nia");
                    SelectedEntity.transform.position = new Vector3(CellScript.WorldLocation.x, CellScript.WorldLocation.y);
                }
                else
                {
                    foreach (var Cell in CellScript.NeighbourCells)
                    {
                        Debug.Log($"Neighbour cells {Cell}");
                    }
                }

            }

        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Debug.Log(HitCollider.gameObject.tag);
        //    switch (HitCollider.gameObject.tag)
        //    {
        //        case "Entity":
        //            Debug.Log("tag");
        //            if (HitCollider.GetComponent<EntityBase>().TeamName == CurrentTeam.ToString())
        //            {
        //                SelectedEntity = RayHit.collider.gameObject;
        //                SelectedEntity.GetComponent<SpriteRenderer>().color = Color.yellow;
        //                Debug.Log("entity");
        //            }

        //            break;

        //        case "Cell":

        //            if (SelectedEntity != null)
        //            {

        //                CellFunctionality CellScript = HitCollider.GetComponent<CellFunctionality>();
        //                Debug.Log("Finger");
        //                if(CellScript.NeighbourCells == null)
        //                {
        //                    Debug.Log("nia");
        //                    CellScript.PopulateNearCellList(AreaCellHeight, AreaCellLength);
        //                }
        //                Debug.Log(CellScript.NeighbourCells.Count);
        //                foreach (var Cell in CellScript.NeighbourCells)
        //                {
        //                    Debug.Log($"Neighbour cells {Cell}");
        //                }
        //                if (CellScript.NeighbourCells.Contains(SelectedEntity.GetComponent<EntityBase>().CurrentPosition))
        //                {
        //                    Debug.Log("nia");
        //                    SelectedEntity.transform.position = new Vector3(CellScript.Location.x, CellScript.Location.y);
        //                }
        //                else
        //                {
        //                    foreach (var Cell in CellScript.NeighbourCells)
        //                    {
        //                        Debug.Log($"Neighbour cells {Cell}");
        //                    }
        //                }
        //                foreach (var Cell in CellScript.NeighbourCells)
        //                {
        //                    Debug.Log($"Neighbour cells {Cell}");
        //                }
        //            }

        //            break;
        //    }

        //}
    }
    
}
