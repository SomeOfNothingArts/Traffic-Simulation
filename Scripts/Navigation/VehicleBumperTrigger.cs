using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleBumperTrigger : MonoBehaviour
{

    public GameObject[] ObjectsInTrigger;
    private GameObject[] _CopyOfObjectsInTrigger;
    private GameObject[] _NewListOfObjectsInTrigger;

    public  bool BumperIsEnabled = true;
    public bool BumperIsActivated;

    // trying to squeeze vehicle if they are facing each other and force them to move
    private void Update()
    {
        if (BumperIsActivated && transform.localScale.x > 1)
        {
            transform.localScale += new Vector3(-0.1f, 0, 0);
        }
        else if (transform.localScale.x != 3)
        {
            transform.localScale = new Vector3(3, 1, 1);
        }
    }

    // activating bumper
    private void OnTriggerEnter(Collider other)
    {
        if (BumperIsEnabled)
        {
            if (other.tag != "Bumper")
            {
                ObjectEnteringTrigger(other.gameObject);

                if (ObjectsInTrigger != null)
                {
                    BumperIsActivated = true;
                }
            }
            else
            {
                Invoke("ForceToDrive", 2f);
                //Debug.Log("Collision in: " + transform.position + " by " + gameObject.name);
            }
        }
    }

    // drive despite collision to remove blockage
    void ForceToDrive()
    {
        BumperIsActivated = false;
    }

    // allowing to move in case of road clearence
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Bumper")
        {
            ObjectLeavingTrigger(other.gameObject);

            if (ObjectsInTrigger == null || ObjectsInTrigger.Length == 0)
            {
                BumperIsActivated = false;
            }
        }
        else
        {
            CancelInvoke("ForceToDrive");
            //Debug.Log("Collision in: " + transform.position + " with " + gameObject.name + " ended");
        }
    }

    // radding object from colliding object list
    public void ObjectEnteringTrigger(GameObject EnteringObject)
    {
        bool objectInsideTriggerAlready = false;


        for (int i = 0; i < ObjectsInTrigger.Length; i++)
        {
            if (ObjectsInTrigger[i] == EnteringObject)
            {
                objectInsideTriggerAlready = true;
            }
        }


        if (!objectInsideTriggerAlready)
        {
            int idNumber = ObjectsInTrigger.Length + 1;


            _CopyOfObjectsInTrigger = new GameObject[idNumber];

            for (int i = 0; i < ObjectsInTrigger.Length; i++)
            {
                _CopyOfObjectsInTrigger[i] = ObjectsInTrigger[i];
            }
            _CopyOfObjectsInTrigger[idNumber - 1] = EnteringObject;

            ObjectsInTrigger = _CopyOfObjectsInTrigger;

            ClearUnusedLists();
        }
    }

    // removing object from colliding object list
    public void ObjectLeavingTrigger(GameObject leavingGuardUnit)
    {
        bool objectInsideTriggerAlready = false;
        int leavingObjectId = -1;

        for (int i = 0; i < ObjectsInTrigger.Length; i++)
        {
            if (ObjectsInTrigger[i] == leavingGuardUnit)
            {
                leavingObjectId = i;
                objectInsideTriggerAlready = true;
            }
        }


        if (objectInsideTriggerAlready)
        {
            for (int i = 0; i < ObjectsInTrigger.Length; i++)
            {
                if (i != leavingObjectId && ObjectsInTrigger[i] != leavingGuardUnit)
                {
                    int idNumber;
                    if (_CopyOfObjectsInTrigger == null)
                    {
                        idNumber = 1;
                    }
                    else
                    {
                        idNumber = _CopyOfObjectsInTrigger.Length + 1;
                    }

                    _CopyOfObjectsInTrigger = new GameObject[idNumber];
                    _CopyOfObjectsInTrigger[idNumber - 1] = ObjectsInTrigger[i];

                    if (_NewListOfObjectsInTrigger != null)
                    {
                        for (int x = 0; x < _NewListOfObjectsInTrigger.Length; x++)
                        {
                            _CopyOfObjectsInTrigger[x] = _NewListOfObjectsInTrigger[x];
                        }
                    }

                    _NewListOfObjectsInTrigger = _CopyOfObjectsInTrigger;
                }
            }

            ObjectsInTrigger = _NewListOfObjectsInTrigger;
            ClearUnusedLists();
        }
    }

    // clearing supporting lists of colliding objects
    void ClearUnusedLists()
    {
        _NewListOfObjectsInTrigger = new GameObject[0];
        _CopyOfObjectsInTrigger = new GameObject[0];
    }
}
