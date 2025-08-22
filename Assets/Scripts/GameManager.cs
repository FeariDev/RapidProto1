using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject retry;
    [SerializeField] private GameObject next;
    [SerializeField] private TextMeshProUGUI menuText;

    public void Event(int eventNumber)
    {
        menu.SetActive(true);
        retry.SetActive(true);
        if (eventNumber == 0)
        {
            //win
            menuText.text = "Stage Cleared";
            next.SetActive(true);
        }
        if (eventNumber == 1)
        {
            //loss
            menuText.text = "Stage Failed";
        }
    }
}
