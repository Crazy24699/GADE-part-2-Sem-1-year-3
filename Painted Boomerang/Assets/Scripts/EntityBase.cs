using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EntityBase : MonoBehaviour
{
    //Bools
    public bool CanInteract;
    public bool Selected;
    public bool IsMoving = false;
    public bool CanPerformAction = true;
    private bool AIControlled;

    //Ints
    const int MaxHealth=3;
    public int CurrentHealth;
    public int MouseDistance;


    public Vector2Int CurrentPosition;

    public Color SpriteColor;

    //Scripts
    public Slider HealthBar;
    public WorldHandler.Teams AssignedTeam;
    public PlayerFunctionality PlayerScript;

    public List<AimDirections> MainDirectionsClass;
    public List<AimDirections> OffsetDirectionsClas;
    protected EnemyAI EnemyAIScript;


    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        HealthBar.minValue = 0;
        HealthBar.maxValue = MaxHealth;
        HealthBar.value = CurrentHealth;

        if(AIControlled)
        {
            EnemyAIScript = GameObject.FindObjectOfType<EnemyAI>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //MouseDistance = new Vector2Int(Mathf.RoundToInt(Mathf.Abs(MousePosition.x - transform.position.x)), Mathf.RoundToInt(Mathf.Abs(MousePosition.y - transform.position.y)));
        MouseDistance = Mathf.RoundToInt(Mathf.Abs(Vector2.Distance(this.transform.position, MousePosition)));
        HealthBar.value = CurrentHealth ;
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

    public void RevertColour()
    {
        this.GetComponent<SpriteRenderer>().color = SpriteColor;
    }

    public void CheckMainCardinalDirections()
    {
        foreach (var AimDirection in MainDirectionsClass)
        {
            RaycastHit RaycastData;
            Physics.Raycast(transform.position, AimDirection.Direction, out RaycastData, 100, EnemyAIScript.InteractableLayers);

            AimDirection.AimDistance = RaycastData.distance;
            AimDirection.AimPoint = RaycastData.point;
        }
    }

    public void CheckOffsetCardinalDirections()
    {
        foreach (var AimDirection in OffsetDirectionsClas)
        {
            RaycastHit2D RaycastData;
            RaycastData = Physics2D.Raycast(transform.position, AimDirection.Direction, 100, EnemyAIScript.InteractableLayers);

            AimDirection.AimDistance = RaycastData.distance;
            AimDirection.AimPoint = RaycastData.point;
        }
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


[System.Serializable]
public class AimDirections
{
    public Vector2 Direction;
    public Vector2 AimPoint;

    public float AimDistance;
}