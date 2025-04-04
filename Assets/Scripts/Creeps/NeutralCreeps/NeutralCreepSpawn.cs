using UnityEngine;

public class NeutralCreepSpawn : MonoBehaviour
{
    private bool campClear = true;
    private Timer timer;

    public GameObject neutralCreepPrefab;
    private Transform spawnPoint;
    private Vector3 boxSize = new Vector3(10, 5, 10);
    public LayerMask checkBoxesForThings;
    public float spawnInterval = 60f; // should be at the turn of each minute
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
            thingsInArea = Physics.OverlapBox(transform.position, boxSize / 2, Quaternion.identity, checkBoxesForThings, QueryTriggerInteraction.Collide);
            campClear = CheckIfHasThingsInArea();

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
        //campClear = CheckIfHasThingsInArea();
        if (campClear == true)
        {
            for (int i = 0; i < neutralCreepsPerWave; ++i)
            {
                float intX = Random.Range(-2f, 2f);
                float intZ = Random.Range(-2f, 2f);
                Vector3 randomPos = new Vector3(spawnPoint.position.x + intX, spawnPoint.position.y, spawnPoint.position.z + intZ);

                GameObject creep = Instantiate(neutralCreepPrefab, randomPos, spawnPoint.rotation);
            }
        }
    }

    bool CheckIfHasThingsInArea()
    {
        bool isClear;
        /*foreach (Collider c in thingsInArea) { Debug.Log(c.gameObject); }*/

        if (thingsInArea.Length > 0) { isClear = false; }
        else { isClear = true;  }

        //Debug.Log(isClear); // DO NOT REMOVE THIS DEBUG. YES IT IS VERY ANNOYING. BUT FOR SOME STUPID REASON, THIS IS THE ONLY THING KEEPING THIS SYSTEM WORKING.
        return isClear;
    }
}
