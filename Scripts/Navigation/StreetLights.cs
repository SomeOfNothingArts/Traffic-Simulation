using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

[Serializable]
struct LightPhases
{
    public NavigationPoint[] greenLightInPhase;
}

public class StreetLights : MonoBehaviour
{
    private int currentPhase;
    [SerializeField]
    private LightPhases[] lightPhases;

    [SerializeField]
    private float phaseLength = 7.5f;

    private void Start()
    {
        InvokeRepeating("TurnAllLightsRed", 0, phaseLength);
        InvokeRepeating("ChangePhase", phaseLength / 5, phaseLength);
    }

    // changing light phase to next one
    void ChangePhase()
    {

        currentPhase += 1;
        if (currentPhase >= lightPhases.Length)
        {
            currentPhase = 0;
        }

        foreach (NavigationPoint light in lightPhases[currentPhase].greenLightInPhase)
        {
            light.redLight = false;
        }
    }

    // turning all lights red for preparation for next phase
    void TurnAllLightsRed()
    {
        for (int a = 0; a < lightPhases.Length; a++)
        {
            foreach (NavigationPoint light in lightPhases[a].greenLightInPhase)
            {
                light.redLight = true;
            }
        }
    }


}
