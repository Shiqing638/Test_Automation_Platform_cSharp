namespace TestFlow.Models;

public class TestSummary
{
    public int TotalTests { get; set; }
    public int PassedTests { get; set; }
    public int FailedTests { get; set; }
    public int ErrorTests { get; set; }
    public List<TestExecutionResult> Results { get; set; } = new();
}