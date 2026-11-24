namespace TestFlow.Configuration;

public class TestCaseConfig
{
    public string Type { get; set; } = "";
    public string TestName { get; set; } = "";
    public double? MeasuredValue { get; set; } 
    public double? MinLimit { get; set; }
    public double? MaxLimit { get; set; }
    public bool? IsConnected { get; set; }
}