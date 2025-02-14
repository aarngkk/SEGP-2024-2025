using UnityEngine;

public class DragCharacter : MonoBehaviour
{
    private Vector3 offset;
    private Camera cam;
    private Rigidbody rb;
    private bool isDragging = false;
    private Plane groundPlane;

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        groundPlane = new Plane(Vector3.up, Vector3.zero); // Define a flat plane at y=0
    }

    void OnMouseDown()
    {
        if (GetMouseWorldPosition(out Vector3 worldPosition))
        {
            offset = transform.position - worldPosition;
            isDragging = true;
        }
    }

    void OnMouseDrag()
    {
        if (isDragging && GetMouseWorldPosition(out Vector3 worldPosition))
        {
            Vector3 targetPosition = worldPosition + offset;
            targetPosition.y = transform.position.y; // Keep height fixed
            rb.MovePosition(targetPosition);
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    private bool GetMouseWorldPosition(out Vector3 worldPosition)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (groundPlane.Raycast(ray, out float distance))
        {
            worldPosition = ray.GetPoint(distance);
            return true;
        }
        worldPosition = Vector3.zero;
        return false;
    }
}
