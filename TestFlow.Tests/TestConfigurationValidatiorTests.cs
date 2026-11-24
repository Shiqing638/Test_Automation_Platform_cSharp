using TestFlow.Configuration;
using Xunit;

public class TestConfigurationValidatorTests
{
    [Fact]
    public void Validate_EmptySuiteName_ThrowsException()
    {
        TestSuiteConfig config = new()
        {
            SuiteName = ""
        };

        TestConfigurationValidator validator =  new();

        Assert.Throws<InvalidDataException>(() => validator.Validate(config));
    }

    [Fact]
    public void Validate_EmptyTests_ThrowsException()
    {
        TestSuiteConfig config = new()
        {
            SuiteName = "emptyTests",
            Tests =  new()
        };

        TestConfigurationValidator validator = new();

        Assert.Throws<InvalidDataException>(() => validator.Validate(config));
    }

    [Fact]
    public void Validate_MissingMeasuredValue_ThrowsException()
    {
        TestSuiteConfig config = new()
        {
            SuiteName = "Voltaget tests",
            Tests = new()
            {
                new TestCaseConfig
                {
                    Type = "Range",
                    TestName = "Voltage Test 1",
                    MinLimit = 4.6,
                    MaxLimit = 5.0
                }
            }
        };

        TestConfigurationValidator validator = new();

        var exception = Assert.Throws<InvalidDataException>(() => validator.Validate(config));

        Assert.Contains("MeasuredValue", exception.Message);
    }

    [Fact]
    public void Validate_InvalidRange_ThrowException()
    {
        TestSuiteConfig config = new()
        {
            SuiteName = "voltagetest",
            Tests = new()
            {
                new TestCaseConfig
                {
                    Type = "Range",
                    TestName = "test",
                    MeasuredValue = 4.8,
                    MinLimit = 6.0,
                    MaxLimit = 5.0
                }
            }
        };

        TestConfigurationValidator validator = new();
        var exception = Assert.Throws<InvalidDataException>(() => validator.Validate(config));

        Assert.Contains("MinLimit", exception.Message);
    }

    [Fact]
    public void Validate_ValidRange_Succeeds()
    {
        TestSuiteConfig config = new()
        {
            SuiteName = "Voltage Tests",
            Tests = new()
            {
                new TestCaseConfig
                {
                    Type = "Range",
                    TestName = "Voltage Test 1",
                    MeasuredValue = 4.8,
                    MinLimit = 4.6,
                    MaxLimit = 5.0
                }
            }
        };
        TestConfigurationValidator validator = new();

        var exception = Record.Exception(() => validator.Validate(config));

        Assert.Null(exception);
    }
}