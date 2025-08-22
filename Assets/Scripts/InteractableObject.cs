using UnityEngine;
using UnityEngine.UIElements;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Collider2D objectCollider;
    [SerializeField] private bool isDragging;
    [SerializeField] private Rigidbody2D rb;
    private float currentRotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isDragging = false;
        currentRotation = 90;
    }

    private void Update()
    {
        Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        if (isDragging)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            this.transform.position = mousePosition;
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
    }
    private void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            rb.constraints = RigidbodyConstraints2D.None;
        }
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                currentRotation += 10;
                this.transform.rotation = Quaternion.Euler(Vector3.forward * currentRotation);
            }
            if (Input.mouseScrollDelta.y < 0)
            {
                currentRotation -= 10;
                this.transform.rotation = Quaternion.Euler(Vector3.forward * currentRotation);
            }
        }
    }
}
