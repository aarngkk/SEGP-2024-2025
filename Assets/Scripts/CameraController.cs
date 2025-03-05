using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float dragSpeed = 0.1f;   // Speed for dragging
    public float rotateSpeed = 2f;   // Speed for rotating
    public float zoomSpeed = 5f;     // Speed for zooming

    public float minZoomDistance = 1f; // Minimum zoom distance
    public float maxZoomDistance = 50f; // Maximum zoom distance

    private Vector3 lastMousePosition;

    void Update()
    {
        HandleDragging();
        HandleRotation();
        HandleZoom();
    }

    void HandleDragging()
    {
        if (Input.GetMouseButtonDown(2))  // Middle mouse button pressed
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))  // While holding middle mouse button
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 move = new Vector3(-delta.x * dragSpeed, 0, -delta.y * dragSpeed);

            // Move the camera in world space
            transform.position += transform.right * move.x; // Move left/right
            transform.position += transform.up * move.z;    // Move up/down (moves the scene, not camera height)

            lastMousePosition = Input.mousePosition;
        }
    }

    void HandleRotation()
    {
        if (Input.GetMouseButtonDown(1))  // Right mouse button pressed
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))  // While holding right mouse button
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            float rotationX = delta.y * rotateSpeed;
            float rotationY = delta.x * rotateSpeed;

            transform.eulerAngles += new Vector3(-rotationX, rotationY, 0);
            lastMousePosition = Input.mousePosition;
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel"); // Get scroll input (-1 to 1)
        Vector3 direction = transform.forward * scroll * zoomSpeed;
        Vector3 newPosition = transform.position + direction;

        // Calculate distance from camera to scene
        float distance = Vector3.Distance(newPosition, Vector3.zero);

        // Clamp zooming so it doesn’t go too far or too close
        if (distance >= minZoomDistance && distance <= maxZoomDistance)
        {
            transform.position = newPosition;
        }
    }
}
