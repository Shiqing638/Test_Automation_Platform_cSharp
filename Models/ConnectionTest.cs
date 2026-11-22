namespace TestFlow.Models;

public class ConnectionTest : ITestCase
{
    public string TestName { get; set; } = "";
    public bool IsConnected { get; set; }
    public bool IsPassed()
    {
        return IsConnected;
    }
}