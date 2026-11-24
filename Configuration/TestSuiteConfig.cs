namespace TestFlow.Configuration;

public class TestSuiteConfig
{
    public string SuiteName { get; set; } = "";
    public List<TestCaseConfig> Tests { get; set; } = new();
}