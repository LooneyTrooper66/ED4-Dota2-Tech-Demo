using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Scroll Values")]
    public float scrollSpeed = 10;
    public Quaternion zoomedOutRotation;
    public Quaternion zoomedInRotation;
    public AnimationCurve curve;
    // for the version that involves the ease-in-out zoom
    //public Vector3 zoomedIn;
    //public Vector3 zoomedOut;
    //bool zoomingIn = false;
    //bool zoomingOut = false;
    //float temp = 0f;

    [Header("Edge Pan Values")]
    public float panSpeed = 25;

    // Mouse Values
    private Vector3 dragOrigin;
    private Vector3 cameraDragOrigin;
    //private Vector3 currentPosition;



    void Update()
    {
        MouseDragInputs();
        ZoomCamera();
        EdgePanInputs();
        // add showcase view
    }

    // Take x and z inputs and translate based on camera rotation
    private void MoveCamera(float xInput, float zInput)
    {
        float zMove = Mathf.Cos(transform.eulerAngles.y * Mathf.PI / 180) * zInput - Mathf.Sin(transform.eulerAngles.y * Mathf.PI / 180) * xInput;
        float xMove = Mathf.Sin(transform.eulerAngles.y * Mathf.PI / 180) * zInput + Mathf.Cos(transform.eulerAngles.y * Mathf.PI / 180) * xInput;

        transform.position = transform.position + new Vector3(xMove, 0, zMove);
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
        Vector3 camPos = transform.position;

        if (mousePos.x <= 0)
        {
            camPos.x -= panSpeed * Time.deltaTime;
        }
        else if (mousePos.x >= Screen.width)
        {
            camPos.x += panSpeed * Time.deltaTime;
        }
        else if (mousePos.y <= 0)
        {
            camPos.z -= panSpeed * Time.deltaTime;
        }
        else if (mousePos.y >= Screen.height)
        {
            camPos.z += panSpeed * Time.deltaTime;
        }

        transform.position = camPos;
    }

    // zoom via scrollwheel
    void ZoomCamera()
    {
        /*Vector3 pos = transform.position; // needed for timer for thingy
        float scroll = Input.GetAxis("Mouse ScrollWheel"); // 0 is still, 0.1 is zoom in, -0.1 is zoom out
        if (scroll == 0.1f)
        {
            zoomingOut = false; zoomingIn = true; StopCoroutine(ZoomOutLerps()); // set zooming out to false and zooming in to true
            StartCoroutine(ZoomInLerps());
        }
        else if (scroll == -0.1f)
        {
            zoomingIn = false; zoomingOut = true; StopCoroutine(ZoomInLerps()); // set zooming in to false and zooming out to true
            StartCoroutine(ZoomOutLerps());
        }*/


        Vector3 pos = transform.position;
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * Time.deltaTime * 300f;
        pos.y = Mathf.Clamp(pos.y, 5, 12);
        transform.position = pos;

        float temp = pos.y / 10;
        transform.rotation = Quaternion.Slerp(zoomedInRotation, zoomedOutRotation, curve.Evaluate(temp));
    }

    /*IEnumerator ZoomInLerps()
    {
        if (zoomingIn == true)
        {
            transform.position = Vector3.Lerp(zoomedOut, zoomedIn, curve.Evaluate(temp));
            transform.rotation = Quaternion.Slerp(zoomedInRotation, zoomedOutRotation, curve.Evaluate(temp));
            temp += Time.deltaTime;
            if (transform.position.y >= 11)
            {
                transform.position = zoomedIn;
                zoomingIn = false;
                temp = 0f;
                yield return null;
            }
        }
    }
    IEnumerator ZoomOutLerps()
    {
        if (zoomingOut == true)
        {
            transform.position = Vector3.Lerp(zoomedIn, zoomedOut, curve.Evaluate(temp));
            transform.rotation = Quaternion.Slerp(zoomedOutRotation, zoomedInRotation, curve.Evaluate(temp));
            temp += Time.deltaTime;
            if (transform.position == zoomedOut)
            {
                zoomingOut = false;
                temp = 0f;
                yield return null;
            }
        }
    }*/
}