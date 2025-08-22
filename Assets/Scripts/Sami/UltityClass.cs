using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UltityClass
{
    public static float Approach(float curVal, float targetVal, float step) //Lähestyy arvoa annetulla step arvolla
    {
        if (curVal < targetVal)
        {
            return curVal + step;
        }
        else if (curVal > targetVal)
        {
            return curVal - step;
        }
        else { return curVal; }
    }
    public static void CreateDelayedEvent(UnityEvent dEvent, float executeAfterSeconds)
    {
        IEnumerator TEvent() {
            yield return new WaitForSeconds(executeAfterSeconds);
            dEvent.Invoke();
        }
        TEvent();
    }
}
