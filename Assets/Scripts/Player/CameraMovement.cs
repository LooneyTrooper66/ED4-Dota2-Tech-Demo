using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class CameraMovement : MonoBehaviour
{
    [Header("Scroll Values")]
    public float scrollSpeed = 10;
    private CinemachineCamera thisCam;
    public float smoothTime = 0.5f;
    private float zoom;
    private float velocity = 0f;
    public Quaternion zoomedOutRotation;
    public Quaternion zoomedInRotation;
    public AnimationCurve curve;

    [Header("Edge Pan Values")]
    public float panSpeed = 25;

    // Mouse Values \\
    private Vector3 dragOrigin;
    private Vector3 cameraDragOrigin;

    [Header("Showcase View Values")]
    public bool isShowcaseActivated = false;
    public CinemachineCamera showcaseCam;
    public GameObject selectedUnit;
    private Transform selectedUnitTransform;
    public Camera mainCam;



    private void Start()
    {
        thisCam = GetComponent<CinemachineCamera>();
        zoom = thisCam.transform.position.y;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!isShowcaseActivated) { isShowcaseActivated = true; }
            else { isShowcaseActivated = false; }
        }

        if (isShowcaseActivated) { ShowcaseModeInputs(); showcaseCam.Priority = 5; }
        else { MouseDragInputs(); EdgePanInputs(); showcaseCam.Priority = 0; } // only use these functions if not in showcase mode

        SelectUnit();
        ZoomCamera();
    }

    // Take x and z inputs and translate based on camera rotation
    private void MoveCamera(float xInput, float zInput)
    {
        float zMove = Mathf.Cos(transform.eulerAngles.y * Mathf.PI / 180) * zInput - Mathf.Sin(transform.eulerAngles.y * Mathf.PI / 180) * xInput;
        float xMove = Mathf.Sin(transform.eulerAngles.y * Mathf.PI / 180) * zInput + Mathf.Cos(transform.eulerAngles.y * Mathf.PI / 180) * xInput;

        transform.position = transform.position + new Vector3(xMove, 0, zMove);
    }

    // showcase mode movement things
    void ShowcaseModeInputs()
    {
        selectedUnitTransform = selectedUnit.transform;
        showcaseCam.Target.TrackingTarget = selectedUnitTransform;
    }

    // Get mouse drag inputs and put values into move camera
    void MouseDragInputs()
    {
        if (Input.GetMouseButtonDown(2))
        {
            cameraDragOrigin = transform.position;
            dragOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition) - dragOrigin;
            Vector3 desirePos = cameraDragOrigin + -1 * new Vector3(pos.x, 0, pos.y) * panSpeed;
            Vector3 move = desirePos - transform.position;
            MoveCamera(move.x, move.z);
        }
    }

    // Get mouse position inputs and put values into move camera if at edge of screen
    void EdgePanInputs()
    {
        Vector3 mousePos = Input.mousePosition;
        int mouseSizeOffset = 20;
        Vector3 camPos = transform.position;

        if (mousePos.x <= mouseSizeOffset)
        {
            camPos.x -= panSpeed * Time.deltaTime;
        }
        else if (mousePos.x >= (Screen.width - mouseSizeOffset))
        {
            camPos.x += panSpeed * Time.deltaTime;
        }
        else if (mousePos.y <= mouseSizeOffset)
        {
            camPos.z -= panSpeed * Time.deltaTime;
        }
        else if (mousePos.y >= (Screen.height - mouseSizeOffset))
        {
            camPos.z += panSpeed * Time.deltaTime;
        }

        transform.position = camPos;
    }

    // select unit based on left mouse click
    void SelectUnit()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            {
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    GameObject temp = hit.transform.gameObject;
                    if (temp.GetComponent<ShowcaseViewSelect>() != null)
                    {
                        selectedUnit = temp;
                    }
                }
            }
        }
    }

    // zoom via scrollwheel
    void ZoomCamera()
    {
        Vector3 pos = transform.position;
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoom -= scroll * scrollSpeed * Time.deltaTime * 300f;
        zoom = Mathf.Clamp(zoom, 6, 14);
        pos.y = Mathf.SmoothDamp(pos.y, zoom, ref velocity, smoothTime);
        transform.position = pos;

        float temp = pos.y / 10;
        transform.rotation = Quaternion.Slerp(zoomedInRotation, zoomedOutRotation, curve.Evaluate(temp));
    }
}