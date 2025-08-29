using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public Slider slider;
    public GameManager manager;

    public enum SliderType
    {
        SandBar,
        TimerBar
    }
    public SliderType sliderType;

    [Header("SandBar Settings")]
    public int winAmount;
    public SandRenderer sandRenderer;

    [Header("TimerBar Settings")]
    public float gameEndTime = 80f;
    [ReadOnly] public float currentTime;



    void Start()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        switch (sliderType)
        {
            case SliderType.SandBar:
                UpdateSandBar();
                break;
            case SliderType.TimerBar:
                UpdateTimerBar();
                break;
        }
    }



    public float sandBarValue;
    void UpdateSandBar()
    {
        sandBarValue = (float)sandRenderer.sandCollected / (float)winAmount;

        SetSlider(sandBarValue);
    }

    public float timerBarValue;
    void UpdateTimerBar()
    {
        currentTime += Time.deltaTime;
        timerBarValue = currentTime / gameEndTime;

        SetSlider(timerBarValue);

        if (currentTime >= gameEndTime)
        {
            manager.Event(1);
        }
    }



    void SetSlider(float value)
    {
        slider.value = value;
    }
}