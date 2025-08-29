using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isGameStarted;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject retry;
    [SerializeField] private GameObject next;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private TextMeshProUGUI menuText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private int timer;
    [SerializeField] private List<GameObject> ghosts;
    [SerializeField] private List<GameObject> whatToTurnOff;

    private void Awake()
    {
        timerText.text = timer.ToString();
        StartCoroutine(TimerRoutine());
        foreach (GameObject ghost in ghosts)
        {
            ghost.SetActive(false);
        }
    }

    IEnumerator TimerRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(1);
        while (true)
        {
            timer -= 1;
            timerText.text = timer.ToString();
            yield return delay;
            if (timer == 0)
            {
                GameStart();
                break;
            }
        }
    }

    IEnumerator TimerRoutineBar()
    {
        WaitForSeconds delay = new WaitForSeconds(1);
        while (true)
        {
            yield return delay;
            if (timer == 0)
            {
                GameStart();
                break;
            }
        }
    }

    private void GameStart()
    {
        foreach (GameObject ghost in ghosts)
        {
            ghost.SetActive(true);
        }
        StartCoroutine(TimerRoutineBar());
        isGameStarted = true;
        //Add starting the sand falling

    }

    public void Event(int eventNumber)
    {
        menu.SetActive(true);
        retry.SetActive(true);
        mainMenu.SetActive(true);
        foreach (GameObject item in whatToTurnOff)
        {
            item.SetActive(false);
        }
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
            next.SetActive(false);
        }
    }
}
