using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class EntityBase : MonoBehaviour
{
    //Bools
    #region Booleans
    private bool AIControlled;
    public bool InStartingArea;
    public bool UpdatedMovepoints;
    public bool UpdateMoveOptions;
    public bool CanMove = false;
    public bool AdvancedMover = false;
    #endregion

    //Ints
    #region Ints
    const int MaxHealth=3;
    public int CurrentHealth;
    public int MouseDistance;
    [Space(2)]
    protected int PreferedYChange;

    #endregion

    //Gameobejcts
    #region GameObjects
    public GameObject DebugHitpoints;
    public GameObject ChosenDestination;
    public GameObject FinalDestination;

    #endregion

    //Vectors
    #region Vectors
    public Vector2Int CurrentPosition;
    public List<Vector2> AvailableMoveOptions = new List<Vector2>();
    protected Vector2 LastMove;
    [Space(2)]
    protected Vector2 XChange;
    protected Vector2 YChange;
    public Vector2 ChosenVectorMove;
    public Vector2[] AimingDirections =
    {
        new Vector2(5,2),
        new Vector2(1,2),
        new Vector2(-3,6),
        new Vector2(10,7.5f),
        new Vector2(4.56f,9.86f)
    };
    protected Vector2 ChosenAimDirection;
    #endregion

    //Floats
    public float EnemyPieceDistance;


    //Scripts
    public Teams AssignedTeam;
    public PlayerFunctionality PlayerScript;
    [SerializeField] protected EnemyAI EnemyAIScript;
    [SerializeField] protected EntityBase TargetPiece;
    public WayPoint LastWaypoint;

    public List<AimDirections> MainDirectionsClass = new List<AimDirections>();
    public List<AttackOptions> AttackPointOptions = new List<AttackOptions>();


    //Unity functionality
    #region Unity Functionality
    public LayerMask InteractableLayers;
    [SerializeField] private Color SpriteColor;
    public UnityEngine.UI.Slider HealthBar;
    #endregion

    public List<string> BadTags= new List<string>();

    public Collider2D WorldCollider;


    // Start is called before the first frame update
    void Start()
    {
        //Startup();
        UpdateMoveOptions = true;
    }

    public void EnableCollider(GameObject Entity)
    {
        Collider2D[] Colliders=Entity.GetComponents<Collider2D>();
        foreach (var Collider in Colliders)
        {
            Collider.enabled = true;
        }
        //WorldCollider.enabled = true;
    }

    public void Startup()
    {
        AIControlled = PlayerScript.AIControlled;
        CurrentHealth = MaxHealth;
        HealthBar.minValue = 0;
        HealthBar.maxValue = MaxHealth;
        HealthBar.value = CurrentHealth;

        InStartingArea = true;
        if (AIControlled && FinalDestination != null)
        {
            if (transform.position.y > FinalDestination.transform.position.y)
            {
                PreferedYChange = 1;
            }
            if (transform.position.y <= FinalDestination.transform.position.y)
            {
                PreferedYChange = -1;
            }
        }


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
        CheckMainCardinalDirections();
        XChange = Vector2.zero;
        YChange=Vector2.zero;

        int XAxisDistance = SetAxisInformation("X");
        int YAxisDistance = SetAxisInformation("Y");


        if (XAxisDistance > YAxisDistance || YChange == Vector2.zero)
        {
            ChosenVectorMove = XChange;
        }
        if (YAxisDistance > XAxisDistance || XChange == Vector2.zero)
        {
            ChosenVectorMove = YChange;
        }
        if (YAxisDistance == XAxisDistance)
        {
            int RandomChance = Random.Range(0, 2);
            switch (RandomChance)
            {
                case 0:
                    ChosenVectorMove = XChange;
                    break;

                case 1:
                    ChosenVectorMove = YChange;
                    break;
            }
        }

    }



    public int SetAxisInformation(string DefinedAxis)
    {
        float AxisDistance = 0;
        float RawAxisDistance;



        switch (DefinedAxis)
        {
            case "X":
                RawAxisDistance = transform.position.x - FinalDestination.transform.position.x;
                AxisDistance = Mathf.Abs(RawAxisDistance);
                bool MoveRight = transform.position.x < FinalDestination.transform.position.x;
                if (!AvailableMoveOptions.Contains(Vector2.right) && !AvailableMoveOptions.Contains(Vector2.left))
                {
                    break;
                }

                switch (MoveRight)
                {
                    case true:
                        if (AvailableMoveOptions.Contains(Vector2.right))
                        {
                            XChange = Vector2.right;
                        }
                        break;

                    case false:
                        if (AvailableMoveOptions.Contains(Vector2.left))
                        {
                            XChange = Vector2.left;
                        }

                        break;
                }

                break;

            case "Y":
                RawAxisDistance = transform.position.y - FinalDestination.transform.position.y;
                AxisDistance = Mathf.Abs(RawAxisDistance);
                bool MoveUp = transform.position.y < FinalDestination.transform.position.y;
                if (!AvailableMoveOptions.Contains(Vector2.up) && !AvailableMoveOptions.Contains(Vector2.down))
                {
                    break;
                }
                if (AvailableMoveOptions.Contains(Vector2.up) && AvailableMoveOptions.Contains(Vector2.down)
                    && AvailableMoveOptions.Count == 2) 
                {
                    YChange = new Vector2(0, PreferedYChange);
                    ChosenVectorMove = new Vector2(0, PreferedYChange);
                    break;
                }
                switch (MoveUp)
                {
                    case true:
                        if (AvailableMoveOptions.Contains(Vector2.up))
                        {
                            YChange = Vector2.up;
                        }
                        break;

                    case false:
                        if (AvailableMoveOptions.Contains(Vector2.down))
                        {
                            YChange = Vector2.down;
                        }
                        break;
                }

                break;
        }



        return Mathf.RoundToInt(AxisDistance);
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

    public void AdvanceMove()
    {

        AIMovePiece(ChosenVectorMove);
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

    public void CheckAttackOptions()
    {

        float AimPointDistance = 0.0f;
        foreach (var AimPoint in AimingDirections)
        {
            SimulateBounces(AimPoint);
        }

    }

    public float SimulateBounces(Vector2 AimDirection)
    {
        Vector2 CurrentRayOrigin = transform.position;
        Vector2 CurrentRayDirection = AimDirection;
        Vector2 BestAimDirection = Vector2.zero;

        for (int i = 0; i < 5; i++)
        {
            RaycastHit2D HitInfo = Physics2D.Raycast(CurrentRayOrigin, AimDirection, 100.00f, InteractableLayers);
            
            if (HitInfo.collider != null)
            {
                CurrentRayDirection = Vector2.Reflect(CurrentRayOrigin, HitInfo.normal);
                CurrentRayOrigin = HitInfo.point;

                Debug.DrawLine(CurrentRayOrigin, HitInfo.point, Color.red);
            }
        }

        return 0.0f;
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
        LastMove = Direction * -1;
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
            AvailableMoveOptions.Clear();
            Collision.GetComponent<Collider2D>().isTrigger = true;
            Debug.Log("blade");

            UpdateMoveOptions = true;
            foreach (var Point in Collision.GetComponent<WayPoint>().AddedConnections)
            {
                
                AvailableMoveOptions.Add(Point.MoveDirection);
                if (AvailableMoveOptions.Contains(LastMove))
                {
                    AvailableMoveOptions.Remove(LastMove);
                }
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