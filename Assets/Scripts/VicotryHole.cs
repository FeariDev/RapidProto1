using UnityEngine;

public class VicotryHole : MonoBehaviour
{
    [SerializeField] private Bars bar;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Sand"))
        {
            Destroy(collision.gameObject);
            bar.AddScore(1);
        }
    }
}
