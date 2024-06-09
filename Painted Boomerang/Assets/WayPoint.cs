using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WayPoint : MonoBehaviour
{

    public ConnectedPoints[] DirectionConnections;
    public List<ConnectedPoints> AddedConnections = new List<ConnectedPoints>();
    public List<Vector2> RelatedDirections = new List<Vector2>();

    public LayerMask InteractableLayers;

    public GameObject HitVisualiser;
    public bool MoveInvalid;

    public Vector2 NextMove;

    private Vector2[] DirectionOptions =
    {
        Vector2.down,
        Vector2.up,
        Vector2.left,
        Vector2.right
    };



    public void Start()
    {
        if (InteractableLayers.Equals("Nothing"))
        {
            InteractableLayers=LayerMask.NameToLayer("Waypoint");
            InteractableLayers=LayerMask.NameToLayer("Pieces");
        }

        for (int i = 0; i < AddedConnections.Count; i++)
        {
            switch (AddedConnections[i].Connection)
            {
                default:
                case "Up":
                    AddedConnections[i].MoveDirection = Vector2.up;
                    //SurroundingData(Vector2.up);
                    break;

                case "Down":
                    AddedConnections[i].MoveDirection = Vector2.down;
                    //SurroundingData(Vector2.down);
                    break;

                case "Left":
                    AddedConnections[i].MoveDirection = Vector2.left;
                    //SurroundingData(Vector2.left);
                    break;

                case "Right":
                    AddedConnections[i].MoveDirection = Vector2.right;
                    //SurroundingData(Vector2.right);
                    break;
            }
        }

        foreach (var Direction in DirectionOptions)
        {
            string FiredDirection = "";
            Vector2 AlteredDirection = transform.position;
            if (Direction == Vector2.up)
            {
                AlteredDirection += Vector2.up;
                FiredDirection = "Up";
            }
            if (Direction == Vector2.down)
            {
                AlteredDirection += Vector2.down;
                FiredDirection = "Down";
            }
            if (Direction == Vector2.left)
            {
                AlteredDirection += Vector2.left;
                FiredDirection = "Left";
            }
            if (Direction == Vector2.right)
            {
                AlteredDirection += Vector2.right;
                FiredDirection = "Right";
            }

            SurroundingData(Direction, AlteredDirection,FiredDirection);
        }

    }

    protected void SurroundingData(Vector2 ShootDirection, Vector2 ShootPoint, string FireDirection)
    {
        RaycastHit2D RaycastData;
        RaycastData = Physics2D.Raycast(ShootPoint, ShootDirection, 100.00f, InteractableLayers);
        if(RaycastData.collider == null)
        {
            return;
        }
        Debug.Log("blood    " + RaycastData.collider.name);


        if (RaycastData.collider.CompareTag("Waypoint") && RaycastData.collider.name != this.name)
        {
            if (RaycastData.collider.GetComponent<WayPoint>().MoveInvalid)
            {
                return;
            }
            RelatedDirections.Add(ShootDirection);
            AddedConnections.Add(new ConnectedPoints
            {
                Connection = FireDirection,
                ConnectionPoint = RaycastData.collider.gameObject,
                MoveDirection = ShootDirection
            });
            //Instantiate(HitVisualiser, RaycastData.transform.position, Quaternion.identity);
            //RaycastData.collider.GetComponent<Collider2D>().isTrigger = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D Collision)
    {
        if (Collision.CompareTag("StartingSquare"))
        {

        }
    }

}

[System.Serializable]
public class ConnectedPoints
{
    public string Connection;
    public GameObject ConnectionPoint;
    public Vector2 MoveDirection;
}
