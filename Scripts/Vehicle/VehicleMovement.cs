using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    // gameobject which is works as bumper and detect collision with next object
    public VehicleBumperTrigger BumperTrigger;

    // Next navigation point to move on
    public NavigationPoint PlanedNextMovementPoint;
    // Previus navigation points stored in memmory
    public NavigationPoint NextMovementPoint;
    private NavigationPoint _PrevMovementPoint;
    private NavigationPoint _SecPrevMovementPoint;

    // ...
    public bool IsaBus;
    [Range(1, 3)]    // Number of navigation points taken by this vehicle
    public int VehicleLength;

    [SerializeField] // Actual speed of this vehicle
    private float _Speed = 0;

    // Acceleration of this vehicle;
    public float SpeedAcceleration = 0.05f;
    // Breaking power of this vehicle
    public float SpeedBreak = 0.2f;
    // Max speed of this vehicle;
    public float SpeedMax = 0.25f;

    // VehicleVisuals for randomization and animation of this vehicle
    VehicleVisuals VS;

    private void Start()
    {
        VS = GetComponent<VehicleVisuals>();
    }
    void Update()
    {
        // emergency navigationpoint finder
        if (NextMovementPoint == null)
        {
            NextMovementPoint = GameObject.Find("NavigationPoint").GetComponent<NavigationPoint>();

            if (NextMovementPoint.OccupyingObject == null)
            {
                NextMovementPoint.OccupyingObject = gameObject;
            }

        }
        // emergency navigationpoint finder 2
        if (PlanedNextMovementPoint == NextMovementPoint || PlanedNextMovementPoint == null)
        {
            if (Random.Range(0, 3) >= 1)
            {
                PlanedNextMovementPoint = NextMovementPoint.NextNavigationPoints[Random.Range(0, NextMovementPoint.NextNavigationPoints.Length)].GetComponent<NavigationPoint>();
            }
            else
            {
                PlanedNextMovementPoint = NextMovementPoint.NextNavigationPoints[0].GetComponent<NavigationPoint>();
            }
        }

        // chenging navigation point to next in case of being close enough
        if ((transform.position - NextMovementPoint.transform.position).magnitude < 10)
        {

            if (!PlanedNextMovementPoint.RedLight && (PlanedNextMovementPoint.OccupyingObject == null || NextMovementPoint.IsEndPoint))
            {
                if (NextMovementPoint.IsEndPoint) // despawning vehicle in case ending in endpoint
                {
                    Destroy(gameObject);
                }
                else
                {
                    ChangeMovementPoint(PlanedNextMovementPoint);
                }
            }
            else
            {
                SlowDown();
            }
        }
        else if (BumperTrigger != null)
        {
            if (!BumperTrigger.BumperIsActivated)
            {
                Movement();
            }
            else
            {
                SlowDown();
            }
        }
        else
        {
            Movement();
        }

    }

    // stoping this vehicle
    void SlowDown()
    {
        if (_Speed > 0)
        {
            _Speed -= SpeedBreak * Time.deltaTime;

            if (_Speed < 0)
            {
                _Speed = 0;
            }
        }
    }

    // speeding up of this vehicle
    void SpeedUp()
    {
        if (_Speed < SpeedMax)
        {
            _Speed += SpeedAcceleration * Time.deltaTime;
        }
        else if (_Speed > SpeedMax)
        {
            _Speed = SpeedMax;
        }
    }

    void Movement()
    {
        SpeedUp();

        // moving vehicle by acctual speed
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, _Speed);

        // rotation of vehicle towards next navigation point
        Vector3 targetDirection = NextMovementPoint.transform.position - transform.position;
        targetDirection.y = 0;
        float singleStep = (_Speed * 5) * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);

        // animation of all wheels accurate to current speed
        if (VS)
        {
            foreach (GameObject wheel in VS.DriveWheels)
            {
                if (wheel != null)
                {
                    wheel.transform.Rotate(0, 0, -10f * (_Speed * 4));

                }
            }
        }
    }

    // Choosing next navigation point
    void ChangeMovementPoint(NavigationPoint choosenMovementPoint)
    {
        if (NextMovementPoint.IsCrossingStartPoint)
        {
            BumperTrigger.BumperIsEnabled = false;
            //BumperTrigger.GetComponent<Collider>().enabled = false;
        }
        else if (NextMovementPoint.IsCrossingEndPoint)
        {
            BumperTrigger.BumperIsEnabled = true;
            //BumperTrigger.GetComponent<Collider>().enabled = true;
        }

        if (VehicleLength == 2)
        {
            if (_PrevMovementPoint == null)
            {

            }
            else if (_PrevMovementPoint.OccupyingObject == gameObject)
            {
                _PrevMovementPoint.OccupyingObject = null;
            }
        }
        else if (VehicleLength == 3)
        {
            if (_SecPrevMovementPoint == null)
            {

            }
            else if (_SecPrevMovementPoint.OccupyingObject == gameObject)
            {
                _SecPrevMovementPoint.OccupyingObject = null;
            }
        }
        else
        {
            if (NextMovementPoint.OccupyingObject == gameObject)
            {
                NextMovementPoint.OccupyingObject = null;
            }
        }

        _SecPrevMovementPoint = _PrevMovementPoint;
        _PrevMovementPoint = NextMovementPoint;
        NextMovementPoint = choosenMovementPoint;
        NextMovementPoint.OccupyingObject = gameObject;
    }
    
    // Clearing navigation points taken by this vehicle from thier memory
    private void OnDestroy()
    {
        if (NextMovementPoint.OccupyingObject == gameObject)
        {
            NextMovementPoint.OccupyingObject = null;
        }
        if (_PrevMovementPoint.OccupyingObject == gameObject)
        {
            _PrevMovementPoint.OccupyingObject = null;
        }
        if (_SecPrevMovementPoint.OccupyingObject == gameObject)
        {
            _SecPrevMovementPoint.OccupyingObject = null;
        }
    }
}
