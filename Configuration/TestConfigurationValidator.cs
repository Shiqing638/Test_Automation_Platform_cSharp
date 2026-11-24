namespace TestFlow.Configuration;

public class TestConfigurationValidator
{
    
    public void Validate(TestSuiteConfig config)
    {
        if (string.IsNullOrWhiteSpace(config.SuiteName))
        {
            throw new InvalidDataException(
                "SuiteName cannot be empty.");
        }

        if (config.Tests == null || config.Tests.Count == 0)
        {
            throw new InvalidDataException(
                    "Test suite must contain at least one test"
                );
        }

        foreach (TestCaseConfig test in config.Tests)
        {
            ValidateTestCase(test);
        }
    }

    private void ValidateTestCase(TestCaseConfig? test)
    {
        if (test == null)
        {
            throw new InvalidDataException(
                "Test case cannot be null."
            );
        }

        if (string.IsNullOrWhiteSpace(test.TestName))
        {
            throw new InvalidDataException("TestName cannot be empty.");
        }

        switch (test.Type)
        {
            case "Range":
                ValidateRangeTest(test);
                break;

            case "Connection":
                ValidateConnectionTest(test);
                break;
            
            default:
                throw new InvalidDataException($"Unsupported test type: {test.Type}");

        }

        if (test.Type != "Range" && test.Type != "Connection")
        {
            throw new InvalidDataException(
                $"Unsupported test type: {test.Type}"
            );
        }
    }

    private void ValidateRangeTest(TestCaseConfig test)
    {
        if(!test.MeasuredValue.HasValue)
        {
            throw new InvalidDataException("MeasuredValue is required");
        }

        if (!test.MinLimit.HasValue)
        {
            throw new InvalidDataException("MinLimit is required.");
        }

        if (!test.MaxLimit.HasValue)
        {
            throw new InvalidDataException("MaxLimit is required.");
        }

        if (test.MinLimit > test.MaxLimit)
        {
            throw new InvalidDataException("MinLimit cannot exceed MaxLimit.");
        }
    }

    private void ValidateConnectionTest(TestCaseConfig test)
    {
        if (!test.IsConnected.HasValue)
        {
            throw new InvalidDataException("IsConnected is required.");
        }
    }
}