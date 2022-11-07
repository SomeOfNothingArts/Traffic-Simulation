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
    
    public bool gameIsPlaying = true;

    public GameObject[] spawnableCars;

    public int timeBetweenSpawns = 5;

    void Start()
    {

        InvokeRepeating("TryToSpawn", 0, timeBetweenSpawns);
    }

    void TryToSpawn()
    {
        if (OnSpawn != null && gameIsPlaying)
        {
            OnSpawn();
        }
    }
}
