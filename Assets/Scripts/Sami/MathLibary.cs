public class MathLibary
{
    public static float Approach(float curVal, float targetVal, float step) //L�hestyy arvoa annetulla step arvolla
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
}
