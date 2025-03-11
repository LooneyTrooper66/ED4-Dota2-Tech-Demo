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
                    // Movement \\
                    agent.SetDestination(hit.point);
                    agent.stoppingDistance = 0;

                    // Rotation \\
                    Quaternion rotationToLookAt = Quaternion.LookRotation(hit.point - transform.position);
                    float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y, ref rotateVelocity, rotateSpeed * (Time.deltaTime * 5));
                    transform.eulerAngles = new Vector3(0, rotationY, 0);
                }
            }
        }
    }



    /*public Camera mainCam;
    public int layer = 3;
    public GameObject moveIcon;
    public Vector3 moveIconStartPoint;
    NavMeshAgent myNavAgent;
    private Coroutine coroutine;


    private void Start()
    {
        myNavAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            {
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject.layer == layer)
                    {
                        Vector3 offset = new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z);
                        if (coroutine != null) StopCoroutine(coroutine);
                        coroutine = StartCoroutine(MoveIcon(offset));

                        myNavAgent.SetDestination(hit.point);
                    }
                }
            }
        }
    }


    IEnumerator MoveIcon(Vector3 offset)
    {
        moveIcon.transform.position = offset;
        yield return new WaitForSecondsRealtime(1);
        moveIcon.transform.position = moveIconStartPoint;
        yield return null;
    }*/
}
