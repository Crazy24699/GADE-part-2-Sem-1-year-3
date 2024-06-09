using System.Collections.Generic;
using UnityEngine;

public class TestAI : MonoBehaviour
{

    public GameObject PieceRef;
    public GameObject DestinationRef;

    public string ChosenMove;
    public List<Vector2> MoveOptions= new List<Vector2>();
    public Vector2 SelectedOption;

    public WayPoint CurrentWaypoint;
    public WayPoint LastWaypoint;
    public EntityBase MovePiece;

    [HideInInspector]public int XDirection;
    [HideInInspector] public int YDirection;

    public string XMoveDirectionString;
    public string YMoveDirectionString;

    public string ChosenMoveDirection;

    public bool HasMove;

    public Vector2 XChange;
    public Vector2 YChange;

    public Vector2 ChosenVectorMove;

    // Start is called before the first frame update
    void Start()
    {
        HasMove = false;
        MovePiece = PieceRef.GetComponent<EntityBase>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            //CheckRoutesDepricated();
            MovePiece.AIMovePiece(ChosenVectorMove);
            
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            CheckRoutes();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            EntityBase MovePiece = PieceRef.GetComponent<EntityBase>();
            MovePiece.CheckMainCardinalDirections();


            //UpdatePieceOptions();
        }
    }

    public void CheckRoutes()
    {
        
        MoveOptions = MovePiece.Options;
        MovePiece.CheckMainCardinalDirections();

        ChosenMoveDirection = "";
        ChosenVectorMove = Vector2.zero;
        XChange = Vector2.zero;
        YChange = Vector2.zero;

        //Vector2 ChosenDirection = Vector2.zero; 
        //UpdatePieceOptions();


        int XAxisDistance = SetAxisInformation("X");
        int YAxisDistance = SetAxisInformation("Y");

        Debug.Log(YAxisDistance + "       " + XAxisDistance);

        if(XAxisDistance > YAxisDistance || YChange==Vector2.zero)
        {
            ChosenMoveDirection = XMoveDirectionString;
            ChosenVectorMove = XChange;
        }
        if(YAxisDistance > XAxisDistance || XChange == Vector2.zero)
        {
            ChosenMoveDirection = YMoveDirectionString;
            ChosenVectorMove = YChange;
        }
        if(YAxisDistance==XAxisDistance)
        {
            int RandomChance = Random.Range(0, 2);
            switch (RandomChance)
            {
                case 0:
                    ChosenMoveDirection = XMoveDirectionString;
                    ChosenVectorMove = XChange;
                    break;

                case 1:
                    ChosenMoveDirection = YMoveDirectionString;
                    ChosenVectorMove = YChange;
                    break;
            }
        }


        HasMove = true;

        //MovePiece.AIMovePiece(ChosenDirection);
    }

    

    public void CheckRoutesDepricated()
    {
        EntityBase MovePiece = PieceRef.GetComponent<EntityBase>();
        MovePiece.CheckMainCardinalDirections();


        //UpdatePieceOptions();


        int XAxisDistance = SetAxisInformation("X");
        int YAxisDistance = SetAxisInformation("Y");

        if (XAxisDistance == YAxisDistance)
        {
            int RandomDirection = Random.Range(0, MoveOptions.Count);

            ChosenMove = MoveOptions[RandomDirection].ToString();
            MovePiece.AIMovePiece(MoveOptions[RandomDirection]);

            Debug.Log("Chosen Random");

            

        }
        else if (YAxisDistance > XAxisDistance)
        {
            YDirection = +1;
            if (PieceRef.transform.position.y > DestinationRef.transform.position.y)
            {
                YDirection = -1;
            }
            int Index = MovePiece.Options.FindIndex(Val => Val.y == +YDirection);
            Debug.Log(Index);
            ChosenMove = MoveOptions[Index].ToString();
            MovePiece.AIMovePiece(MoveOptions[Index]);
        }
        else if (XAxisDistance > YAxisDistance)
        {
            int Index = MovePiece.Options.FindIndex(Val => Val.x == +1.0f);
            Debug.Log(Index);
            ChosenMove = MoveOptions[Index].ToString();
            MovePiece.AIMovePiece(MoveOptions[Index]);
        }
    }

    public void UpdatePieceOptions()
    {
        Debug.Log("welcome to paradise");
        //PieceRef.GetComponent<EntityBase>().Options.Clear();
        foreach (var Direction in PieceRef.GetComponent<EntityBase>().MainDirectionsClass)
        {
            //Debug.Log(Direction.AimDistance);
            if (Direction.AimDistance > 4)
            {
                //MoveOptions.Add(Direction.Direction);
            }
        }

        //PieceRef.GetComponent<EntityBase>().Options = MoveOptions;
        //EnemyAIScript.ChosenPiece.UpdatedMovepoints = true; 
    }

    public int SetAxisInformation(string DefinedAxis)
    {
        float AxisDistance = 0;
        float RawAxisDistance;

        switch (DefinedAxis)
        {
            case "X":
                RawAxisDistance = PieceRef.transform.position.x - DestinationRef.transform.position.x;
                AxisDistance = Mathf.Abs(RawAxisDistance);
                bool MoveRight = PieceRef.transform.position.x < DestinationRef.transform.position.x;
                if (!MoveOptions.Contains(Vector2.right) && !MoveOptions.Contains(Vector2.left))
                {
                    break;
                }
                switch (MoveRight)
                {
                    case true:
                        if (MoveOptions.Contains(Vector2.right))
                        {
                            XMoveDirectionString = "Right";
                            XChange = Vector2.right;
                        }
                        break;

                    case false:
                        if (MoveOptions.Contains(Vector2.left))
                        {
                            XMoveDirectionString = "Left";
                            XChange = Vector2.left;
                        }

                        break;
                }

                break;

            case "Y":
                RawAxisDistance = PieceRef.transform.position.y - DestinationRef.transform.position.y;
                AxisDistance = Mathf.Abs(RawAxisDistance);
                bool MoveUp = PieceRef.transform.position.y < DestinationRef.transform.position.y;
                if (!MoveOptions.Contains(Vector2.up) && !MoveOptions.Contains(Vector2.down))
                {
                    break;
                }
                switch (MoveUp)
                {
                    case true:
                        if (MoveOptions.Contains(Vector2.up))
                        {
                            YMoveDirectionString = "Up";
                            YChange = Vector2.up;
                        }
                        break;

                    case false:
                        if (MoveOptions.Contains(Vector2.down))
                        {
                            YMoveDirectionString = "Down";
                            YChange = Vector2.down;
                        }
                        break;
                }

                break;
        }



        return Mathf.RoundToInt(AxisDistance);
    }

    private void OnTriggerEnter2D(Collider2D Collision)
    {
        
    }

}
