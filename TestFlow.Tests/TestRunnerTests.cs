using TestFlow.Models;
using TestFlow.Services;

namespace TestFlow.Tests;

public class TestRunnerTests
{
    [Fact]
    public void Run_WhenOneTestThrows_ContinuesExecutingOtherTests()
    {
        RangeTest test = new()
        {
            TestName = "Voltage Test",
            MeasuredValue = 4.8,
            MinLimit = 6.0,
            MaxLimit = 5.0
        };

        ConnectionTest test2 = new()
        {
            TestName = "connection test",
            IsConnected = true
        };

        List<ITestCase> tests = new()
        {
            test,
            test2
        };

        TestRunner runner = new();
        TestSummary summary = runner.Run(tests);

        Assert.Equal(2, summary.TotalTests);
        Assert.Equal(1, summary.PassedTests);
        Assert.Equal(1, summary.ErrorTests);
        Assert.Equal(2, summary.Results.Count);

        Assert.Equal(TestExecutionStatus.Error, summary.Results[0].Status);
        Assert.Equal(TestExecutionStatus.Passed, summary.Results[1].Status);
        Assert.Equal(TestSuiteStatus.Failed, summary.Status);
    }

    [Fact]
    public void Run_WhenTestListIsEmpty_ReturnsZeroCounts()
    {
        List<ITestCase> tests = new();

        TestRunner runner = new();
        TestSummary summary = runner.Run(tests);

        Assert.Equal(0, summary.TotalTests);
        Assert.Equal(0, summary.PassedTests);
        Assert.Equal(0, summary.FailedTests);
        Assert.Equal(0, summary.ErrorTests);
        Assert.Empty(summary.Results);
        Assert.Equal(TestSuiteStatus.NoTests, summary.Status);
    }

    [Fact]
    public void Run_WhenValueExceedsLimit_RecordsFailedStatus()
    {
        RangeTest test = new()
        {
            TestName = "Voltage Test new",
            MeasuredValue = 5.5,
            MinLimit = 4.5,
            MaxLimit = 5.0
        };

        List<ITestCase> tests = new()
        {
            test
        };

        TestRunner runner = new();
        TestSummary summary = runner.Run(tests);

        Assert.Equal(1, summary.TotalTests);
        Assert.Equal(0, summary.PassedTests);
        Assert.Equal(1, summary.FailedTests);
        Assert.Equal(0, summary.ErrorTests);
        Assert.Equal(TestExecutionStatus.Failed, summary.Results[0].Status);
        Assert.Equal(TestSuiteStatus.Failed, summary.Status);
    }

    [Fact]
    public void Run_WhenAllTestPass_ReturnsPassedSuiteStatus()
    {
        RangeTest test = new()
        {
            TestName = "Range test new",
            MeasuredValue = 4.8,
            MinLimit = 4.5,
            MaxLimit = 5.0
        };

        ConnectionTest test2 = new()
        {
            TestName = "connection test",
            IsConnected = true
        };

        List<ITestCase> tests = new()
        {
            test,
            test2
        };

        TestRunner runner = new();
        TestSummary summary = runner.Run(tests);

        Assert.Equal(2, summary.TotalTests);
        Assert.Equal(2, summary.PassedTests);
        Assert.Equal(0, summary.FailedTests);
        Assert.Equal(0, summary.ErrorTests);
        Assert.Equal(TestSuiteStatus.Passed, summary.Status);
    }
}
