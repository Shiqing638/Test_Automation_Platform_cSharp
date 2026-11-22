namespace TestFlow.Models;

public class TestResult
{
    public string TestName {get; set; } = "";
    public double MeasuredValue {get; set; }
    public double MaxLimit { get; set; }
    public double MinLimit { get; set; }
    public bool IsPassed()
    {
        if (MinLimit > MaxLimit)
        {
            throw new ArgumentException("MinLimit cannot exceed Maxlimit");
        }
        return MeasuredValue >= MinLimit && MeasuredValue <= MaxLimit;
    }
}
