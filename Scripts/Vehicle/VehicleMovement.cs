using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    // gameobject which is works as bumper and detect collision with next object
    public VehicleBumperTrigger bumperTrigger;

    // Next navigation point to move on
    public NavigationPoint planedNextMovementPoint;
    // Previus navigation points stored in memmory
    public NavigationPoint nextMovementPoint;
    private NavigationPoint prevMovementPoint;
    private NavigationPoint secPrevMovementPoint;

    [Range(1, 3)]    // Number of navigation points taken by this vehicle
    public int vehicleLength;

    [SerializeField] // Actual speed of this vehicle
    private float speed = 0;

    // Acceleration of this vehicle;
    [SerializeField]
    private float speedAcceleration = 0.05f;
    // Breaking power of this vehicle
    [SerializeField]
    private float speedBreak = 0.2f;
    // Max speed of this vehicle;
    [SerializeField]
    private float speedMax = 0.25f;

    // VehicleVisuals for randomization and animation of this vehicle
    private VehicleVisuals vV;

    private void Start()
    {
        vV = GetComponent<VehicleVisuals>();
    }
    void Update()
    {
        // emergency navigationpoint finder
        if (nextMovementPoint == null)
        {
            nextMovementPoint = GameObject.Find("NavigationPoint").GetComponent<NavigationPoint>();

            if (nextMovementPoint.occupyingObject == null)
            {
                nextMovementPoint.occupyingObject = gameObject;
            }

        }
        // emergency navigationpoint finder 2
        if (planedNextMovementPoint == nextMovementPoint || planedNextMovementPoint == null)
        {
            if (Random.Range(0, 3) >= 1)
            {
                planedNextMovementPoint = nextMovementPoint.nextNavigationPoints[Random.Range(0, nextMovementPoint.nextNavigationPoints.Length)].GetComponent<NavigationPoint>();
            }
            else
            {
                planedNextMovementPoint = nextMovementPoint.nextNavigationPoints[0].GetComponent<NavigationPoint>();
            }
        }

        // chenging navigation point to next in case of being close enough
        if ((transform.position - nextMovementPoint.transform.position).magnitude < 10)
        {

            if (!planedNextMovementPoint.redLight && (planedNextMovementPoint.occupyingObject == null || nextMovementPoint.isEndPoint))
            {
                if (nextMovementPoint.isEndPoint) // despawning vehicle in case ending in endpoint
                {
                    Destroy(gameObject);
                }
                else
                {
                    ChangeMovementPoint(planedNextMovementPoint);
                }
            }
            else
            {
                SlowDown();
            }
        }
        else if (bumperTrigger != null)
        {
            if (!bumperTrigger.bumperIsActivated)
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
        if (speed > 0)
        {
            speed -= speedBreak * Time.deltaTime;

            if (speed < 0)
            {
                speed = 0;
            }
        }
    }

    // speeding up of this vehicle
    void SpeedUp()
    {
        if (speed < speedMax)
        {
            speed += speedAcceleration * Time.deltaTime;
        }
        else if (speed > speedMax)
        {
            speed = speedMax;
        }
    }

    void Movement()
    {
        SpeedUp();

        // moving vehicle by acctual speed
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, speed);

        // rotation of vehicle towards next navigation point
        Vector3 targetDirection = nextMovementPoint.transform.position - transform.position;
        targetDirection.y = 0;
        float singleStep = (speed * 5) * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);

        // animation of all wheels accurate to current speed
        if (vV)
        {
            foreach (GameObject wheel in vV.driveWheels)
            {
                if (wheel != null)
                {
                    wheel.transform.Rotate(0, 0, -10f * (speed * 4));

                }
            }
        }
    }

    // Choosing next navigation point
    void ChangeMovementPoint(NavigationPoint choosenMovementPoint)
    {
        if (nextMovementPoint.isCrossingStartPoint)
        {
            bumperTrigger.bumperIsEnabled = false;
            //BumperTrigger.GetComponent<Collider>().enabled = false;
        }
        else if (nextMovementPoint.isCrossingEndPoint)
        {
            bumperTrigger.bumperIsEnabled = true;
            //BumperTrigger.GetComponent<Collider>().enabled = true;
        }

        if (vehicleLength == 2)
        {
            if (prevMovementPoint == null)
            {

            }
            else if (prevMovementPoint.occupyingObject == gameObject)
            {
                prevMovementPoint.occupyingObject = null;
            }
        }
        else if (vehicleLength == 3)
        {
            if (secPrevMovementPoint == null)
            {

            }
            else if (secPrevMovementPoint.occupyingObject == gameObject)
            {
                secPrevMovementPoint.occupyingObject = null;
            }
        }
        else
        {
            if (nextMovementPoint.occupyingObject == gameObject)
            {
                nextMovementPoint.occupyingObject = null;
            }
        }

        secPrevMovementPoint = prevMovementPoint;
        prevMovementPoint = nextMovementPoint;
        nextMovementPoint = choosenMovementPoint;
        nextMovementPoint.occupyingObject = gameObject;
    }
    
    // Clearing navigation points taken by this vehicle from thier memory
    private void OnDestroy()
    {
        if (nextMovementPoint.occupyingObject == gameObject)
        {
            nextMovementPoint.occupyingObject = null;
        }
        if (prevMovementPoint.occupyingObject == gameObject)
        {
            prevMovementPoint.occupyingObject = null;
        }
        if (secPrevMovementPoint.occupyingObject == gameObject)
        {
            secPrevMovementPoint.occupyingObject = null;
        }
    }
}
