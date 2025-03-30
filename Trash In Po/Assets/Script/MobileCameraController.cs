using UnityEngine;

public class MobileCameraController : MonoBehaviour
{
    public float panSpeed = 0.5f; // Adjust speed for dragging
    public float zoomSpeed = 0.5f;

    public float minZoom = 5f;
    public float maxZoom = 20f;

    public Vector2 boundaryMin = new Vector2(-20, -20);
    public Vector2 boundaryMax = new Vector2(20, 20);

    private Camera cam;
    private Vector3 lastTouchPos;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandlePan();
        HandleZoom();
    }

    void HandlePan()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector3 touchDelta = (Vector3)Input.GetTouch(0).deltaPosition;
            Vector3 move = new Vector3(-touchDelta.x * panSpeed * Time.deltaTime, 0, -touchDelta.y * panSpeed * Time.deltaTime);
            transform.position += move;

            ClampCameraPosition();
        }
    }

    void HandleZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            float prevDist = (touch1.position - touch1.deltaPosition - (touch2.position - touch2.deltaPosition)).magnitude;
            float currDist = (touch1.position - touch2.position).magnitude;

            float zoomDelta = (prevDist - currDist) * zoomSpeed * Time.deltaTime;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + zoomDelta, minZoom, maxZoom);
        }
    }

    void ClampCameraPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, boundaryMin.x, boundaryMax.x);
        pos.z = Mathf.Clamp(pos.z, boundaryMin.y, boundaryMax.y);
        transform.position = pos;
    }
}
