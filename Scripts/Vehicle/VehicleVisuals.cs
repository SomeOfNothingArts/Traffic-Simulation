using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleVisuals : MonoBehaviour
{
    // Available material to use on specific vehicle
    public Material[] AvailableMaterials;
    // All body parts which will be set to random material
    public GameObject[] BodyParts;
    // ...
    public GameObject[] LStearingWheels;
    // ...
    public GameObject[] RStearingWheels;
    // all wheels which are going to be rotated
    public GameObject[] DriveWheels;


    private void Start()
    {
        // Vehicle material randomizator
        if (AvailableMaterials != null)
        {
            int randomMaterialId = Random.Range(0, AvailableMaterials.Length);

            if (BodyParts != null)
            {
                foreach (GameObject part in BodyParts)
                {
                    part.GetComponent<Renderer>().material = AvailableMaterials[randomMaterialId];
                }
            }
        }
    }
}
