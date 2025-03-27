using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaneCreepSpawn : MonoBehaviour
{
    public GameObject meleeCreepPrefab;
    public GameObject rangedCreepPrefab;
    public GameObject siegeCreepPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 30f; // should be 30 seconds
    public int meleeCreepsPerWave = 3;
    public int rangedCreepsPerWave = 1;
    public int siegeCreepsPerWave = 1;
    public int wavesUntilSiegeCreep = 5;  // should be on the 6th, 11th, 16th, 21st, etc..
    private int waveCount = 0;
    private Timer timer;
    private float timeSinceLastWave = 0;


    private void Start()
    {
        GameObject timerHolder = GameObject.Find("--Timer--");
        timer = timerHolder.GetComponent<Timer>();
        timeSinceLastWave = spawnInterval;
    }

    private void Update()
    {
        if (timer.gamePlaying == true && timer.timeElapsed >= 0)
        {
            timeSinceLastWave += Time.deltaTime;
            if (timeSinceLastWave >= spawnInterval)
            {
                SpawnCreeps();
                timeSinceLastWave = 0;
            }
        }
    }

    void SpawnCreeps()
    {
        waveCount++;
        switch (waveCount) // this is genuinely the most efficient way to do it coz the difference between the wave changes isnt consistent
        {
            case 16:
                meleeCreepsPerWave = 4;
                break;
            case 31:
                meleeCreepsPerWave = 5;
                break;
            case 36:
                siegeCreepsPerWave = 2;
                break;
            case 41:
                rangedCreepsPerWave = 2;
                break;
            case 46:
                meleeCreepsPerWave = 6;
                break;
        }
        if (waveCount % wavesUntilSiegeCreep == 0)
        {
            SpawnRegularCreeps();
            SpawnSiegeCreep();
        }
        else
        {
            SpawnRegularCreeps();
        }
    }

    void SpawnRegularCreeps()
    {
        for (int i = 0; i < meleeCreepsPerWave; ++i)
        {
            GameObject creep = Instantiate(meleeCreepPrefab, spawnPoint.position, spawnPoint.rotation);
        }

        for (int i = 0;i < rangedCreepsPerWave; i++)
        {
            GameObject creep = Instantiate(rangedCreepPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }

    void SpawnSiegeCreep()
    {
        for (int i = 0; i < rangedCreepsPerWave; i++)
        {
            GameObject creep = Instantiate(siegeCreepPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
