using UnityEngine;
using UnityEngine.UI;

public class Bars : MonoBehaviour
{
    [SerializeField] private int amountNeededForEvent = 80;
    [SerializeField] private int currentAmount;
    [SerializeField] private int eventNumber;
    [SerializeField] private Image progressBar;
    public GameManager manager;

    private void Update()
    {
        //For testing only
        if (Input.GetMouseButtonDown(0))
        {
            AddScore(1);
        }
    }

    public void AddScore(int amount)
    {
        currentAmount += amount;
        progressBar.rectTransform.localScale += new Vector3(0f, amount / 10f, 0f); //reaches 80
        progressBar.rectTransform.localPosition += new Vector3(0f, amount*5f, 0f); //for every /10 in scale, add *5 position
        if (currentAmount >= amountNeededForEvent)
        {
            manager.Event(eventNumber);
        }
    }
}
