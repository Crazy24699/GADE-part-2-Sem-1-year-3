using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PiecesDestination : MonoBehaviour
{
    public EntityBase Piece1;
    public GameObject Piece1Dest;
    [Space(5),Header("")]
    public EntityBase Piece2;
    public GameObject Piece2Dest;


    private void Start()
    {
        StartCoroutine(WaypointsInactive());
    }

    IEnumerator WaypointsInactive()
    {
        yield return new WaitForSeconds(0.25f);
        WayPoint[] AllWaypoints = FindObjectsByType<WayPoint>(FindObjectsSortMode.InstanceID);
        foreach (var Waypoint in AllWaypoints)
        {
            Collider2D[] Colliders = Waypoint.GetComponents<Collider2D>();
            foreach (var collider in Colliders)
            {
                collider.isTrigger = true;
            }
            //Waypoint.GetComponent<Collider2D>().isTrigger = true;
        }
    }

}
