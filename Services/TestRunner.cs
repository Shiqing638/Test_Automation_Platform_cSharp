using System.Collections.Generic;
using TestFlow.Models;
using System;


namespace TestFlow.Services;

public class TestRunner
{
    public TestSummary Run(List<ITestCase> tests)
    {
        TestSummary summary = new ();
        
        foreach (ITestCase test in tests)
        {
            try
            {
                bool passed = test.IsPassed();
                TestExecutionResult result = new TestExecutionResult
                {
                    TestName = test.TestName,
                    Status = passed? TestExecutionStatus.Passed : TestExecutionStatus.Failed,
                    ErrorMessage = null
                };

                summary.Results.Add(result);
            }
            catch (ArgumentException ex)
            {
                TestExecutionResult result = new TestExecutionResult
                {
                    TestName = test.TestName,
                    Status = TestExecutionStatus.Error,
                    ErrorMessage = ex.Message
                };
                summary.Results.Add(result);
            }
        }

        summary.TotalTests = tests.Count;

        summary.PassedTests = summary.Results.Count(
            result => result.Status == TestExecutionStatus.Passed
        );

        summary.FailedTests = summary.Results.Count(
            result => result.Status == TestExecutionStatus.Failed
        );
        
        summary.ErrorTests = summary.Results.Count(
            result => result.Status == TestExecutionStatus.Error
        );

        return summary;
    }
}