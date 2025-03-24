using UnityEngine;
using UnityEngine.AI;

public class NeutralCreepAI : MonoBehaviour
{
    private NavMeshAgent creepAgent;
    public Transform currentTarget;
    public string enemyCreepTag;
    public string enemyTowerTag;
    private float stoppingDist = 0.9f;
    public float aggroRange = 6f;
    private float targetSwitchInterval = 0.5f;
    private float timeSinceLastTargetSwitch = 0f;


    private void Start()
    {
        creepAgent = GetComponent<NavMeshAgent>();
    }
}
