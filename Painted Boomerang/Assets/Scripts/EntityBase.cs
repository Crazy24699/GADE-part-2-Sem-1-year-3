using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    public bool CanInteract;
    public bool Selected;
    public bool IsMoving = false;

    public bool CanPerformAction = true;

    const int MaxHealth=3;
    public int CurrentHealth;

    public Vector2Int CurrentPosition;


    public int MouseDistance;

    public WorldHandler.Teams AssignedTeam;
    public PlayerFunctionality PlayerScript;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //MouseDistance = new Vector2Int(Mathf.RoundToInt(Mathf.Abs(MousePosition.x - transform.position.x)), Mathf.RoundToInt(Mathf.Abs(MousePosition.y - transform.position.y)));
        MouseDistance = Mathf.RoundToInt(Mathf.Abs(Vector2.Distance(this.transform.position, MousePosition)));
    }

    public void MoveEntity(Vector2Int CellCord)
    {
        this.transform.position = new Vector3(CellCord.x, CellCord.y);
    }

    public int HandleHealth(int HealthChange)
    {
        CurrentHealth += HealthChange;
        if(CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            PlayerScript.Entities.Remove(this);
            PlayerScript.CheckPieces();
            Destroy(this.gameObject);
        }
        return CurrentHealth;
    }

    private void OnTriggerEnter2D(Collider2D Collision)
    {
        if (Collision.CompareTag("Cell"))
        {
            CellFunctionality CellScript;
            Collision.TryGetComponent<CellFunctionality>(out CellScript);
            CellScript.enabled = true;
            CurrentPosition = CellScript.Location;
        }
    }
    private void OnTriggerExit2D(Collider2D Collision)
    {
        if (Collision.CompareTag("Cell"))
        {
            CellFunctionality CellScript;
            Collision.TryGetComponent<CellFunctionality>(out CellScript);
            CellScript.enabled = false;
        }
    }
}
