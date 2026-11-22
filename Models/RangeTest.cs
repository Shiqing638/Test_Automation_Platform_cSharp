namespace TestFlow.Models;

public class RangeTest : ITestCase
{
    public string TestName { get; set; } = "";
    public double MeasuredValue { get; set; }
    public double MinLimit { get; set; }
    public double MaxLimit { get; set; }
    public bool IsPassed()
    {
        if (MinLimit > MaxLimit)
        {
            throw new ArgumentException("MinLimit cannot exceed maxlimit");
        }

        return MeasuredValue >= MinLimit && MeasuredValue <= MaxLimit;
    } 
}