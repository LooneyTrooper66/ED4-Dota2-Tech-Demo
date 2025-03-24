using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaneCreepSpawn : MonoBehaviour
{
    public float creepMoveSpeed;
    public float siegeCreepMoveSpeed;

    public GameObject meleeCreepPrefab;
    public GameObject rangedCreepPrefab;
    public GameObject siegeCreepPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 30f; // should be 30 seconds
    public int meleeCreepsPerWave = 3;
    public int rangedCreepsPerWave = 1;
    public int wavesUntilSiegeCreep = 6;
    private int waveCount = 0;
    private Timer timer;
    private bool wavesStart;


    private void Start()
    {
        GameObject timerHolder = GameObject.Find("--Timer--");
        timer = timerHolder.GetComponent<Timer>();
        wavesStart = false;
    }

    private void Update()
    {
        if (timer.timeElapsed >= 0 && wavesStart == false)
        {
            StartCoroutine(SpawnCreeps());
            wavesStart = true;
        }
    }

    IEnumerator SpawnCreeps()
    {
        while (true)
        {
            waveCount++;

            if (waveCount % wavesUntilSiegeCreep == 0)
            {
                SpawnRegularCreeps();
                SpawnSiegeCreep();
                yield return new WaitForSecondsRealtime(spawnInterval);
            }
            else
            {
                SpawnRegularCreeps();
                yield return new WaitForSecondsRealtime(spawnInterval);
            }
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
        GameObject creep = Instantiate(siegeCreepPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
