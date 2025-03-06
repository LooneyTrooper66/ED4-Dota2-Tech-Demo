using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Button Input Vars
    public float panSpeed = 25;
    public float scrollSpeed = 20;
    public float minY = 5f;
    public float maxY = 30f;

    // Mouse Input Vars
    private Vector3 dragOrigin;
    private Vector3 cameraDragOrigin;
    private Vector3 currentPosition;

    void Update()
    {
        ButtonInputs();
        MouseInputs();
        ZoomCamera();
    }

    // Take x and z inputs and translate based on camera rotation
    private void MoveCamera(float xInput, float zInput)
    {
        float zMove = Mathf.Cos(transform.eulerAngles.y * Mathf.PI / 180) * zInput - Mathf.Sin(transform.eulerAngles.y * Mathf.PI / 180) * xInput;
        float xMove = Mathf.Sin(transform.eulerAngles.y * Mathf.PI / 180) * zInput + Mathf.Cos(transform.eulerAngles.y * Mathf.PI / 180) * xInput;

        transform.position = transform.position + new Vector3(xMove, 0, zMove);
    }

    // Get mouse drag inputs
    void MouseInputs()
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

    // get keyboard inputs
    void ButtonInputs()
    {
        float inputZ = 0f;
        float inputX = 0f;

        if (Input.GetKey("w"))
        {
            inputZ += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            inputZ -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            inputX += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            inputX -= panSpeed * Time.deltaTime;
        }

        MoveCamera(inputX, inputZ);
    }

    // zoom via scrollwheel
    void ZoomCamera()
    {
        Vector3 pos = transform.position;
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * Time.deltaTime * 300f;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }
}
