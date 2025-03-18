using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    NavMeshAgent agent;
    public float rotateSpeed = 0.1f;
    float rotateVelocity;

    public Animator anim;
    float motionSmoothTime = 0.1f;

    [Header("Enemy Targeting")]
    public GameObject targetEnemy;
    public float stoppingDist;


    private void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Animation();
        Move();
    }

    void Animation()
    {
        float speed = agent.velocity.magnitude / agent.speed;
        anim.SetFloat("Speed", speed, motionSmoothTime, Time.deltaTime);
    }

    void Move()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "Ground")
                {
                    MoveToPos(hit.point);
                }
            }
        }

        // use this if end up using MoveToEnemy function
        /*if (targetEnemy != null)
        {
            if (Vector3.Distance(transform.position, targetEnemy.transform.position) > stoppingDist)
            {
                agent.SetDestination(targetEnemy.transform.position);
            }
        }*/
    }

    void MoveToPos(Vector3 pos)
    {
        agent.SetDestination(pos);
        agent.stoppingDistance = 0;
        RotateHero(pos);
    }

    // access this if cast an ability while not within range of target (only dismember probably)
    public void MoveToEnemy(GameObject enemy)
    {
        targetEnemy = enemy;
        agent.SetDestination(targetEnemy.transform.position);
        agent.stoppingDistance = stoppingDist;

        RotateHero(targetEnemy.transform.position);
    }

    void RotateHero(Vector3 lookPos)
    {
        Quaternion rotationToLookAt = Quaternion.LookRotation(lookPos - transform.position);
        float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y, ref rotateVelocity, rotateSpeed * (Time.deltaTime * 5));
        transform.eulerAngles = new Vector3(0, rotationY, 0);
    }
}
