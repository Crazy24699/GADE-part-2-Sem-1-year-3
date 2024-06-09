using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class EntityBase : MonoBehaviour
{
    //Bools
    private bool AIControlled;
    public bool InStartingArea;
    public bool UpdatedMovepoints;

    //Ints
    const int MaxHealth=3;
    public int CurrentHealth;
    public int MouseDistance;
    public int MovesDoneSinceRefresh;

    //Gameobejcts
    public GameObject DebugHitpoints;
    public GameObject ChosenDestination;

    //Vectors
    public Vector2Int CurrentPosition;
    public List<Vector2> Options = new List<Vector2>();


    [SerializeField] private Color SpriteColor;

    //Scripts
    public UnityEngine.UI.Slider HealthBar;
    public Teams AssignedTeam;
    public PlayerFunctionality PlayerScript;
    [SerializeField] protected EnemyAI EnemyAIScript;
    [SerializeField] protected EntityBase TargetPiece;
    public WayPoint LastWaypoint;

    public List<AimDirections> MainDirectionsClass = new List<AimDirections>();
    public List<AttackOptions> AttackPointOptions = new List<AttackOptions>();
    
    public float EnemyPieceDistance;

    public List<string> BadTags= new List<string>();


    // Start is called before the first frame update
    void Start()
    {
        Startup();
    }

    public void Startup()
    {
        AIControlled = PlayerScript.AIControlled;
        CurrentHealth = MaxHealth;
        HealthBar.minValue = 0;
        HealthBar.maxValue = MaxHealth;
        HealthBar.value = CurrentHealth;

        InStartingArea = true;

        this.gameObject.name = this.gameObject.name + PlayerScript.OwnPieces.Count;

        if (AIControlled)
        {
            EnemyAIScript = GameObject.FindObjectOfType<EnemyAI>();
        }
    }

    public void ChooseDestination()
    {
        if (ChosenDestination == null || InStartingArea)
        {
            WayPoint[] WaypointPositions = GameObject.FindObjectsOfType<WayPoint>();
            float BestWaypointDistance = 0.0f;
            GameObject WaypointDestination = new GameObject();

            for (int i = 0; i < 5; i++)
            {
                float CurrentPointDistance = Vector2.Distance(WaypointPositions[i].gameObject.transform.position, transform.position);
                if (CurrentPointDistance < BestWaypointDistance)
                {
                    WaypointDestination = WaypointPositions[i].gameObject;
                }
            }

            ChosenDestination = WaypointDestination;
        }
        else if(ChosenDestination != null)
        {
            WayPoint CollidedPointScript = ChosenDestination.GetComponent<WayPoint>();
            
            ChosenDestination = CollidedPointScript.AddedConnections[0].ConnectionPoint;
        }

    }

    public void CheckRoutes()
    {

    }

    protected void ProcessPossibleMoves()
    {
        if (ChosenDestination == null)
        {
            return;
        }


        float XDifference = Mathf.Abs(transform.position.x - ChosenDestination.transform.position.x);
        float YDifference = Mathf.Abs(transform.position.y - ChosenDestination.transform.position.y);

        bool ChangeYLocation = YDifference > XDifference;
        bool YAxisInLine = transform.position.y == ChosenDestination.transform.position.y;

        if(ChangeYLocation && !YAxisInLine)
        {
            CheckMainCardinalDirections();
            //AimDirections[] AvaliableMoves=
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

    //Checks if the current piece in is LOS of the enemy
    public bool CheckLOS()
    {
        
        return true;
    }

    

    public void SelectAttackPiece()
    {
        
        foreach (var Piece in EnemyAIScript.ThisPlayerScript.EnemyPieces)
        {
            float Distance = Vector3.Distance(this.transform.position, Piece.transform.position);
            if (Distance > EnemyPieceDistance)
            {
                EnemyPieceDistance = Distance;
                TargetPiece = Piece;
            }
        }
    }



    public void SetRandomAttackPositions()
    {
        int PositionChangeValue = 0;
        for (int i = 0; i < 3; )
        {
            int RandomSide = Random.Range(0, 2);
            switch (RandomSide)
            {
                case 0:
                    PositionChangeValue = -1;
                    break;

                default:
                case 1:
                    PositionChangeValue = 1;
                    break;
            }

            if (AttackPointOptions.Count == 0 || AttackPointOptions.Count < 3) 
            {
                AttackPointOptions.Add(new AttackOptions());

                
            }

            if (AttackPointOptions.Count <= 3) 
            {
                AttackPointOptions[i].XPosition = Random.Range(1 * PositionChangeValue, 7 * PositionChangeValue);
                AttackPointOptions[i].YPosition = Random.Range(1 * PositionChangeValue, 7 * PositionChangeValue);
            }
            Debug.Log(i + "     Love bites   " + AttackPointOptions.Count);
            //Debug.Log(AttackPointOptions.Count + "        " + i);
            if (AttackPointOptions.Count<i)
            {
                
                
            }

            i++;


        }
    }

    public void SetEntityInfo(PlayerFunctionality PlayerScriptRef,Color TeamColour, Teams SetTeam)
    {
        AssignedTeam = SetTeam;
        PlayerScript = PlayerScriptRef;
        SpriteColor = TeamColour;
        RevertColour();
    }

    public int HandleHealth(int HealthChange)
    {
        CurrentHealth += HealthChange;
        if(CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            PlayerScript.OwnPieces.Remove(this);
            PlayerScript.EnemyPieces.Remove(this);
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
            RaycastHit2D RaycastData;
            RaycastData = Physics2D.Raycast(transform.position, AimDirection.Direction, 100.00f, EnemyAIScript.InteractableLayers);
            AimDirection.AimDistance = RaycastData.distance;
            AimDirection.AimPoint = RaycastData.point;
            AimDirection.ObejctTag = RaycastData.collider.gameObject.tag;
            //Debug.Log(RaycastData.collider.gameObject.name + "     Devil trigger    " + AimDirection.AimPoint);
            //Instantiate(DebugHitpoints, AimDirection.AimPoint, Quaternion.identity);
        }
    }

    public void AIMovePiece(Vector2 Direction)
    {
        Debug.DrawLine(transform.position, this.transform.position + new Vector3(Direction.x * 4, Direction.y * 4, 0), Color.black);
        this.transform.position += new Vector3(Direction.x * 4, Direction.y * 4, 0);
        PlayerScript.MovesRemaining--;
        UpdatedMovepoints = false;
        //Options.Clear();
        //Debug.Log("Moved");
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
        if (Collision.CompareTag("StartingSquare"))
        {
            InStartingArea = true;
        }

        if(AIControlled && Collision.CompareTag("Waypoint"))
        {
            Options.Clear();
            Collision.GetComponent<Collider2D>().isTrigger = true;
            Debug.Log("blade");
            foreach (var Point in Collision.GetComponent<WayPoint>().AddedConnections)
            {
                
                Options.Add(Point.MoveDirection);
            }
            if (Collision.gameObject.Equals(ChosenDestination))
            {
                //ChooseDestination();
                CheckRoutes();
                return;
            }



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
        if (Collision.CompareTag("StartingSquare"))
        {
            InStartingArea = false;
        }

        if (AIControlled && Collision.CompareTag("Waypoint"))
        {
            Collision.GetComponent<Collider2D>().isTrigger = false;

        }
    }



}


[System.Serializable]
public class AimDirections
{
    public Vector2 Direction;
    public Vector2 AimPoint;

    public float AimDistance;

    public string ObejctTag;
}

[System.Serializable]
public class AttackOptions
{
    public float XPosition = 0;
    public float YPosition = 0;


}