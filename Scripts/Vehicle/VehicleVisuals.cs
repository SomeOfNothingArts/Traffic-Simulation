using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleVisuals : MonoBehaviour
{
    // Available material to use on specific vehicle
    public Material[] availableMaterials;
    // All body parts which will be set to random material
    public GameObject[] bodyParts;
    // all wheels which are going to be rotated
    public GameObject[] driveWheels;


    private void Start()
    {
        // Vehicle material randomizator
        if (availableMaterials != null)
        {
            int randomMaterialId = Random.Range(0, availableMaterials.Length);

            if (bodyParts != null)
            {
                foreach (GameObject part in bodyParts)
                {
                    part.GetComponent<Renderer>().material = availableMaterials[randomMaterialId];
                }
            }
        }
    }
}
