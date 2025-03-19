using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaneCreepSpawn : MonoBehaviour
{
    public float creepMoveSpeed;
    public float siegeCreepMoveSpeed;

    public GameObject creepPrefab;
    public GameObject siegeCreepPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 30f; // should be 30 seconds
    public int meleeCreepsPerWave = 3;
    public int rangedCreepsPerWave = 1;
    public int wavesUntilSiegeCreep = 6;
    private int waveCount = 0;


    private void Start()
    {
        StartCoroutine(SpawnCreeps());
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
            GameObject creep = Instantiate(creepPrefab, spawnPoint.position, spawnPoint.rotation);
        }

        for (int i = 0;i < rangedCreepsPerWave; i++)
        {

        }
    }

    void SpawnSiegeCreep()
    {

    }
}
