using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLights : MonoBehaviour
{
    [Range(2, 4)]
    public int NumberOfPhases;
    public int CurrentPhase;

    public NavigationPoint[] GreenLightPointsPhase1;
    public NavigationPoint[] GreenLightPointsPhase2;
    public NavigationPoint[] GreenLightPointsPhase3;
    public NavigationPoint[] GreenLightPointsPhase4;


    public float PhaseLength = 7.5f;

    private void Start()
    {
        InvokeRepeating("TurnAllLightsRed", 0, PhaseLength);
        InvokeRepeating("ChangePhase", PhaseLength / 5, PhaseLength);
    }

    // changing light phase to next one
    void ChangePhase()
    {

        CurrentPhase += 1;
        if (CurrentPhase > NumberOfPhases)
        {
            CurrentPhase = 1;
        }

        if (CurrentPhase == 1 && GreenLightPointsPhase1 != null)
        {
            foreach (NavigationPoint light in GreenLightPointsPhase1)
            {
                light.RedLight = false;
            }
        }
        else if (CurrentPhase == 2 && GreenLightPointsPhase2 != null)
        {
            foreach (NavigationPoint light in GreenLightPointsPhase2)
            {
                light.RedLight = false;
            }
        }
        else if (CurrentPhase == 3 && GreenLightPointsPhase3 != null)
        {
            foreach (NavigationPoint light in GreenLightPointsPhase3)
            {
                light.RedLight = false;
            }
        }
        else if (CurrentPhase == 4 && GreenLightPointsPhase4 != null)
        {
            foreach (NavigationPoint light in GreenLightPointsPhase4)
            {
                light.RedLight = false;
            }
        }

    }

    // turning all lights red for preparation for next phase
    void TurnAllLightsRed()
    {
        if (GreenLightPointsPhase1 != null)
            foreach (NavigationPoint light in GreenLightPointsPhase1)
            {
                light.RedLight = true;
            }
        if (GreenLightPointsPhase2 != null)
            foreach (NavigationPoint light in GreenLightPointsPhase2)
            {
                light.RedLight = true;
            }
        if (GreenLightPointsPhase3 != null)
            foreach (NavigationPoint light in GreenLightPointsPhase3)
            {
                light.RedLight = true;
            }
        if (GreenLightPointsPhase4 != null)
            foreach (NavigationPoint light in GreenLightPointsPhase4)
            {
                light.RedLight = true;
            }
    }


}
