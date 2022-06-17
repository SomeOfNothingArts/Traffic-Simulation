using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NavigationPoint : MonoBehaviour
{
    bool SelectedInEditor = false;

    [Range(0, 9)] // selected id for object in NextNavigationPoints to use AutoConnector on;
    public int AutoConnectorSelectedNaviPoint;
    [Range(0, 50)] // range of searching to new points by AutoConnector;
    public int AutoConnectorRange = 5;
    // variable for saving id of last autoconnected point and sorting them in order;
    int RandomNumber;

    // All conected navigation points for which #0 is the main direction of road
    public GameObject[] NextNavigationPoints;
    // Vehicle which reserved this navigation point for its movement
    public GameObject OccupyingObject;
    // Is this navigation point disabled by StreetLights?
    public bool RedLight;

    // Is this vehicle despawn zone
    public bool IsEndPoint;    
    // Is this vehicle spawn zone
    public bool IsStartPoint;

    // Is this start of section point
    public bool IsCrossingStartPoint;
    // End this start of section point
    public bool IsCrossingEndPoint;

    // subscribsion to event
    private void OnEnable()
    {
        if (IsStartPoint)
        {
            VehicleSpawnerMaster.OnSpawn += SpawnNewCar;
        }
    }   
    // unsubscribsion to event
    private void OnDestroy()
    {
        VehicleSpawnerMaster.OnSpawn -= SpawnNewCar;
    }

    // Inside Editor button which highlights next navigationpoints ways
    public void MarkConnections()
    {
        if (SelectedInEditor)
        {
            SelectedInEditor = false;
        }
        else
        {
            SelectedInEditor = true;
        }
    }

    // Chance to spawn new car
    void SpawnNewCar()
    {
        int rn = Random.Range(0, 4);
        if (OccupyingObject == null && rn >= 3)
        {
            VehicleSpawnerMaster VSMaster = VehicleSpawnerMaster.instance;
            Vector3 SpawnPos = gameObject.transform.position;
            SpawnPos.y = 0;

            if (VSMaster.SpawnableCars != null)
            {
                GameObject newCar = Instantiate(VSMaster.SpawnableCars[Random.Range(0, VSMaster.SpawnableCars.Length)], SpawnPos, Quaternion.identity);
                newCar.GetComponent<VehicleMovement>().NextMovementPoint = this;

                Vector3 relativePos = new Vector3(NextNavigationPoints[0].GetComponent<NavigationPoint>().transform.position.x, 0, NextNavigationPoints[0].GetComponent<NavigationPoint>().transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
                newCar.transform.rotation = Quaternion.LookRotation(relativePos);
            }
        }
    }

    // Inside Edtor button which can save a lot of unnessesery clicking and adding next navigation points in inspector to few clicks and ctrl+z
    public void FindNewNavigationPoint()
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, AutoConnectorRange);
        NavigationPoint[] nearNPs = null;

        for (int x = 0; x < hitColliders.Length; x++)
        {
            if (hitColliders[x].gameObject.transform.childCount > 0)
            {
                if (hitColliders[x].gameObject.transform.GetChild(0).GetComponent<NavigationPoint>() && hitColliders[x].gameObject.transform.GetChild(0).gameObject != this.gameObject)
                {
                    NavigationPoint[] nearNPsCopy = nearNPs;

                    int nextId;

                    if (nearNPs != null)
                    {
                        nextId = nearNPs.Length + 1;
                    }
                    else
                    {
                        nextId = 1;
                    }

                    nearNPs = new NavigationPoint[nextId];

                    if (nearNPsCopy != null)
                    {
                        for (int y = 0; y < nearNPsCopy.Length; y++)
                        {
                            nearNPs[y] = nearNPsCopy[y];
                        }
                    }

                    nearNPs[nearNPs.Length - 1] = hitColliders[x].gameObject.transform.GetChild(0).GetComponent<NavigationPoint>();
                }
            }
        }

        if (NextNavigationPoints.Length >= AutoConnectorSelectedNaviPoint && nearNPs != null)
        {
            if (RandomNumber < nearNPs.Length - 1)
            {
                RandomNumber += 1;
            }
            else
            {
                RandomNumber = 0;
            }
            NextNavigationPoints[AutoConnectorSelectedNaviPoint] = nearNPs[RandomNumber].gameObject;

        }
    }
    void OnDrawGizmos()
    {

        // Drawing debug sphere on navigation point in right color
        DrawSphere();

        if (NextNavigationPoints[0] != null)
        {
            foreach (GameObject marker in NextNavigationPoints)
            {
                if (marker != null)
                {
                    if (SelectedInEditor) // if selected in editor drawing red debug line between all connected navigation points with cyan-yellow arrow in move direction
                    {
                        DrawArrow(transform.position, marker.transform.position, 90, 0.5f, 1, Color.red, Color.yellow, Color.green);
                    }
                    else // Drawing blue debug line between all connected navigation points with black-white arrow in move direction
                    {
                        DrawArrow(transform.position, marker.transform.position, 90, 0.5f, 1, Color.blue, Color.black, Color.white);
                    }
                }
            }

        }
    }

    void DrawSphere()
    {
        // Chenging navigation point debug color as movementlock state is

        if (IsEndPoint) // if marked as vehicle despawn zone
        {
            Gizmos.color = Color.black;
        }
        else if (NextNavigationPoints[0] == null) //if there is no next navigationpoint what will couse error
        {
            Gizmos.color = Color.magenta;
        }
        else if (IsStartPoint) // if marked as vehicle spawn zone
        {
            Gizmos.color = Color.white;
        }
        else if (RedLight) // if stoped by street lights
        {
            Gizmos.color = Color.red;
        }
        else if (IsCrossingStartPoint) // if marked as start of section
        {
            Gizmos.color = Color.cyan;
        }
        else if (IsCrossingEndPoint) // if marked as end of section
        {
            Gizmos.color = Color.blue;
        }
        else if (OccupyingObject != null) // if reserved to move by any vehicle
        {
            Gizmos.color = Color.yellow;
        }
        else // if it is free to move on
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawSphere(transform.position, 1);
    }

    /// Based on  https://forum.unity3d.com/threads/debug-drawarrow.85980/
    void DrawArrow(Vector3 a, Vector3 b, float arrowheadAngle, float arrowheadDistance, float arrowheadLength, Color c1, Color c2, Color c3)
    {

        // Get the Direction of the Vector
        Vector3 dir = b - a;

        // Get the Position of the Arrowhead along the length of the line.
        Vector3 arrowPos = a + (dir * arrowheadDistance);

        // Get the Arrowhead Lines using the direction from earlier multiplied by a vector representing half of the full angle of the arrowhead (y)
        // and -1 for going backwards instead of forwards (z), which is then multiplied by the desired length of the arrowhead lines coming from the point.

        Vector3 up = Quaternion.LookRotation(dir) * new Vector3(0f, Mathf.Sin(arrowheadAngle / 72), -1f) * arrowheadLength;
        Vector3 down = Quaternion.LookRotation(dir) * new Vector3(0f, -Mathf.Sin(arrowheadAngle / 72), -1f) * arrowheadLength;
        Vector3 left = Quaternion.LookRotation(dir) * new Vector3(Mathf.Sin(arrowheadAngle / 72), 0f, -1f) * arrowheadLength;
        Vector3 right = Quaternion.LookRotation(dir) * new Vector3(-Mathf.Sin(arrowheadAngle / 72), 0f, -1f) * arrowheadLength;

        // Get the End Locations of all points for connecting arrowhead lines.
        Vector3 upPos = arrowPos + up;
        Vector3 downPos = arrowPos + down;
        Vector3 leftPos = arrowPos + left;
        Vector3 rightPos = arrowPos + right;

        // Draw the line from A to B
        Gizmos.color = c1;
        Gizmos.DrawLine(a, b);

        // Draw the rays representing the arrowhead.
        Gizmos.color = c2;
        Gizmos.DrawRay(arrowPos, up);
        Gizmos.DrawRay(arrowPos, down);
        Gizmos.DrawRay(arrowPos, left);
        Gizmos.DrawRay(arrowPos, right);

        // Draw Connections between rays representing the arrowhead
        Gizmos.color = c3;
        Gizmos.DrawLine(upPos, leftPos);
        Gizmos.DrawLine(leftPos, downPos);
        Gizmos.DrawLine(downPos, rightPos);
        Gizmos.DrawLine(rightPos, upPos);

    }
}