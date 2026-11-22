namespace TestFlow.Models;

public interface ITestCase
{
    string TestName { get; }
    bool IsPassed();
}