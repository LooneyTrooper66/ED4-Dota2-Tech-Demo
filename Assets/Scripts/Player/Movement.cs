using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    public NavMeshAgent agent;
    public float rotateSpeed = 0.1f;
    float rotateVelocity;

    [Header("Enemy Targeting")]
    public Transform currentTarget;
    private float stoppingDist = 0;
    private float attackRange;
    [Tooltip("half the heroes radius pretty please")] public float offset;
    public LayerMask layerMask;
    HeroCombat combat;
    HeroStats stats;


    private void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        combat = GetComponent<HeroCombat>();
        stats = GetComponent<HeroStats>();
        stoppingDist += offset;
        attackRange = stats.attackRange - offset;
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        GetInputs();

        if (currentTarget != null)
        {
            float dist = Vector3.Distance(transform.position, currentTarget.position);
            if (dist >= attackRange && combat.state == HeroCombat.currentState.travelling) // only move closer if not attacking and not in attack range
            {
                MoveToEnemy(currentTarget);
            }
            else
            {
                agent.SetDestination(transform.position);
            }
        }
    }

    void GetInputs()
    {
        if (Input.GetMouseButtonDown(1))
        {
            combat.state = HeroCombat.currentState.travelling;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "Ground")
                {
                    currentTarget = null;
                    MoveToPos(hit.point);
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
            {
                CurrentAttackingState attackable = hit.collider.gameObject.GetComponent<CurrentAttackingState>();
                if (attackable != null)
                {
                    currentTarget = hit.collider.gameObject.transform;
                }
            }
        }
    }

    void MoveToPos(Vector3 pos)
    {
        agent.SetDestination(pos);
        agent.stoppingDistance = 0;
        RotateHero(pos);
    }

    public void MoveToEnemy(Transform enemy)
    {
        currentTarget = enemy;
        agent.SetDestination(currentTarget.transform.position);
        agent.stoppingDistance = stoppingDist;

        RotateHero(currentTarget.position);
    }

    void RotateHero(Vector3 lookPos)
    {
        Quaternion rotationToLookAt = Quaternion.LookRotation(lookPos - transform.position);
        float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y, ref rotateVelocity, rotateSpeed * (Time.deltaTime * 5));
        transform.eulerAngles = new Vector3(0, rotationY, 0);
    }
}
