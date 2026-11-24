using TestFlow.Models;

namespace TestFlow.Configuration;

public class TestCaseMapper
{
    public ITestCase Map(TestCaseConfig test)
    {
        return test.Type switch
        {
            "Range" => new RangeTest
            {
                TestName = test.TestName,
                MeasuredValue = test.MeasuredValue.Value,
                MinLimit = test.MinLimit.Value,
                MaxLimit = test.MaxLimit.Value
            },

            "Connection" => new ConnectionTest
            {
                TestName = test.TestName,
                IsConnected = test.IsConnected.Value
            },

            _ => throw new InvalidDataException(
                $"Unsupported test type: {test.Type}"
            )
        };
    }

    public List<ITestCase> MapAll(TestSuiteConfig config)
    {
        return config.Tests.Select(test => Map(test)).ToList();
    } 
}