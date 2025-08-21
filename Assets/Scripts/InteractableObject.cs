using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Collider2D objectCollider;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool isDragging;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        isDragging = false;
    }

    private void Update()
    {
        Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        if (isDragging)
        {
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
        }
    }
}
