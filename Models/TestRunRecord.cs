namespace TestFlow.Models;

public class TestRunRecord
{
    public int RunId { get; set; }
    public string SuiteName { get; set; } = "";
    public DateTime ExecutedAt { get; set; }
    public int TotalTests { get; set; }
    public int PassedTests { get; set; }
    public int FailedTests { get; set; }
    
    public int ErrorTests { get; set; }
    public string Status { get; set; } = "";
}