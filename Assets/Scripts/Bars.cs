using UnityEngine;
using UnityEngine.UI;

public class Bars : MonoBehaviour
{
    [SerializeField] private int amountNeededForEvent = 80;
    [SerializeField] private int currentAmount;
    [SerializeField] private Image progressBar;
    public GameManager manager;

    public void AddScore(int amount)
    {
        currentAmount += amount;
        progressBar.rectTransform.localScale += new Vector3(0f, amount / 10f, 0f); //reaches 80
        progressBar.rectTransform.localPosition += new Vector3(0f, amount*5f, 0f); //Normal scane /10 scale, *5 position
        if (currentAmount >= amountNeededForEvent)
        {
            manager.Event(1);
        }
    }
}
