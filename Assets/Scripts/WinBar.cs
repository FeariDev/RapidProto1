using UnityEngine.UI;

using UnityEngine;

public class WinBar : MonoBehaviour
{
    [SerializeField] private int amountNeededForEvent = 80;
    [SerializeField] SandRenderer sr;
    [SerializeField] private Image progressBar;
    public GameManager manager;

    private void Update()
    {
        progressBar.rectTransform.localScale += new Vector3(0f, sr.sandCollected / 10f, 0f); //reaches 80
        progressBar.rectTransform.localPosition += new Vector3(0f, sr.sandCollected * 5f, 0f); //Normal scane /10 scale, *5 position
        if (sr.sandCollected >= amountNeededForEvent)
        {
            manager.Event(0);
        }
    }
}
