using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleBumperTrigger : MonoBehaviour
{

    public GameObject[] objectsInTrigger;


    public  bool bumperIsEnabled = true;
    public bool bumperIsActivated;

    // trying to squeeze vehicle if they are facing each other and force them to move
    private void Update()
    {
        if (bumperIsActivated && transform.localScale.x > 1)
        {
            transform.localScale += new Vector3(-0.1f, 0, 0);

            if (objectsInTrigger == null)
            {
                bumperIsActivated = false;
            }
        }
        else if (transform.localScale.x != 3)
        {
            transform.localScale = new Vector3(3, 1, 1);
        }
    }

    // activating bumper
    private void OnTriggerEnter(Collider other)
    {
        if (bumperIsEnabled)
        {
            if (other.tag != "Bumper")
            {
                ObjectEnteringTrigger(other.gameObject);
            }
            else
            {
                Invoke("ForceToDrive", 2f);
            }
        }
    }

    // drive despite collision to remove blockage
    void ForceToDrive()
    {
        bumperIsActivated = false;
    }

    // allowing to move in case of road clearence
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Bumper")
        {
            ObjectLeavingTrigger(other.gameObject);

            if (objectsInTrigger == null || objectsInTrigger.Length == 0)
            {
                bumperIsActivated = false;
            }
        }
        else
        {
            CancelInvoke("ForceToDrive");
        }
    }

    // radding object from colliding object list
    public void ObjectEnteringTrigger(GameObject EnteringObject)
    {
        bool objectInsideTriggerAlready = false;

        GameObject[] copyOfObjectsInTrigger;
        int objectsLength;

        if (objectsInTrigger != null)
        {
            objectsLength = objectsInTrigger.Length;
        }
        else
        {
            objectsLength = 0;
        }
            for (int i = 0; i < objectsLength; i++)
            {
                if (objectsInTrigger[i] == EnteringObject)
                {
                    objectInsideTriggerAlready = true;
                }
            }
        

        if (!objectInsideTriggerAlready)
        {
            int idNumber = objectsLength + 1;


            copyOfObjectsInTrigger = new GameObject[idNumber];

            for (int i = 0; i < objectsLength; i++)
            {
                copyOfObjectsInTrigger[i] = objectsInTrigger[i];
            }
            copyOfObjectsInTrigger[idNumber - 1] = EnteringObject;

            objectsInTrigger = copyOfObjectsInTrigger;
        }
    }

    // removing object from colliding object list
    public void ObjectLeavingTrigger(GameObject leavingObject)
    {
        bool objectInsideTriggerAlready = false;
        int leavingObjectId = -1;

        GameObject[] copyOfObjectsInTrigger = null;
        GameObject[] newListOfObjectsInTrigger = null;

        if (objectsInTrigger != null)
        {
            for (int i = 0; i < objectsInTrigger.Length; i++)
            {
                if (objectsInTrigger[i] == leavingObject)
                {
                    leavingObjectId = i;
                    objectInsideTriggerAlready = true;
                }
            }
        }


        if (objectInsideTriggerAlready)
        {
            for (int i = 0; i < objectsInTrigger.Length; i++)
            {
                if (i != leavingObjectId && objectsInTrigger[i] != leavingObject)
                {
                    int idNumber;
                    if (copyOfObjectsInTrigger == null)
                    {
                        idNumber = 1;
                    }
                    else
                    {
                        idNumber = copyOfObjectsInTrigger.Length + 1;
                    }

                    copyOfObjectsInTrigger = new GameObject[idNumber];
                    copyOfObjectsInTrigger[idNumber - 1] = objectsInTrigger[i];

                    if (newListOfObjectsInTrigger != null)
                    {
                        for (int x = 0; x < newListOfObjectsInTrigger.Length; x++)
                        {
                            copyOfObjectsInTrigger[x] = newListOfObjectsInTrigger[x];
                        }
                    }

                    newListOfObjectsInTrigger = copyOfObjectsInTrigger;
                }
            }
                objectsInTrigger = newListOfObjectsInTrigger;
            
        }
    }
}
