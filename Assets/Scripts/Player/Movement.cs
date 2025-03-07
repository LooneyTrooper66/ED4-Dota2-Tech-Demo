using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    public Camera mainCam;
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
    }
}
