using UnityEngine;

public class NeutralCreepSpawn : MonoBehaviour
{
    private bool campClear = true;
    private Timer timer;

    public ObjectPooler poolScript;
    public string poolTag;
    private Transform spawnPoint;
    private Vector3 boxSize = new Vector3(10, 5, 10);
    public LayerMask checkBoxesForThings;
    public float spawnInterval = 59f; // should be at the turn of each minute
    public int neutralCreepsPerWave = 4;
    private float timeSinceLastWave = 0;
    Collider[] thingsInArea;


    private void Start()
    {
        GameObject timerHolder = GameObject.Find("--Timer--");
        timer = timerHolder.GetComponent<Timer>();
        spawnPoint = transform;
    }

    private void Update()
    {
        if (timer.gamePlaying == true && timer.timeElapsed >= 0)
        {
            bool thingsInArea = Physics.CheckBox(transform.position, boxSize / 2, transform.rotation, checkBoxesForThings);
            if (!thingsInArea) { campClear = true; }
            else { campClear = false; }
            //Debug.Log(campClear);

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
        if (campClear == true)
        {
            for (int i = 0; i < neutralCreepsPerWave; ++i)
            {
                float intX = Random.Range(-2f, 2f);
                float intZ = Random.Range(-2f, 2f);
                Vector3 randomPos = new Vector3(spawnPoint.position.x + intX, spawnPoint.position.y, spawnPoint.position.z + intZ);

                poolScript.SpawnFromPool(poolTag, randomPos, spawnPoint.rotation);
            }
        }
    }
}
