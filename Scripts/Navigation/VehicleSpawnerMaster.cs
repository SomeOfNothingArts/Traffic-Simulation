using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VehicleSpawnerMaster : MonoBehaviour
{
    public static VehicleSpawnerMaster instance { get; private set; }
    public delegate void SpawnTime();
    public static event SpawnTime OnSpawn;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public bool GameIsPlaying = true;

    public GameObject[] SpawnableCars;

    public int TimeBetweenSpawns = 5;
    //UnityEvent Event_SpawnTime;

    void Start()
    {
        //if (Event_SpawnTime == null)
        //{
        //    Event_SpawnTime = new UnityEvent();
        //}

        InvokeRepeating("TryToSpawn", 0, TimeBetweenSpawns);
    }

    void TryToSpawn()
    {
        if (OnSpawn != null && GameIsPlaying)
        {
            OnSpawn();
        }
    }
}
