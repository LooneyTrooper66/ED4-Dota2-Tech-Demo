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
        campClear = CheckIfHasThingsInArea();
        Debug.Log(campClear);
        if (campClear == true)
        {
            for (int i = 0; i < neutralCreepsPerWave; ++i)
            {
                GameObject creep = Instantiate(neutralCreepPrefab, spawnPoint.position, spawnPoint.rotation);
            }
        }
        Debug.Log("called spawn function, " + gameObject.name + " = " + campClear);
    }

    bool CheckIfHasThingsInArea()
    {
        bool isClear;
        Collider[] thingsInArea = Physics.OverlapBox(spawnPoint.position, boxSize / 2, Quaternion.identity, checkBoxesForThings);
        if (thingsInArea.Length > 0) { isClear = false; }
        else { isClear = true;  }

        return isClear;
    }
}
