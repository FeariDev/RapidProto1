using UnityEngine;

public class VicotryHole : MonoBehaviour
{
    [SerializeField] private Bars bar;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sand"))
        {
            Destroy(collision.gameObject);
            bar.AddScore(1);
        }
    }
}
