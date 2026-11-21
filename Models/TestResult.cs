namespace TestFlow.Models;

public class TestResult
{
    public string TestName {get; set; } = "";
    public double MeasuredValue {get; set; }
    public double MaxLimit { get; set; }
    public bool IsPassed()
    {
        return MeasuredValue <= MaxLimit;
    }
}
