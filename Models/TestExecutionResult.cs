namespace TestFlow.Models;

public class TestExecutionResult
{
    public string TestName { get; set; } = "";
    public TestExecutionStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
}