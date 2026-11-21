using System.Collections.Generic;
using TestFlow.Models;
using System;

namespace TestFlow.Services;

public class TestRunner
{
    public TestSummary Run(List<TestResult> tests)
    {
        int passedCount = 0;
        TestSummary summary = new ();
        
        foreach (TestResult test in tests)
        {
            bool passed = test.IsPassed();
            summary.Results.Add(passed);

            if(passed)
            {
                passedCount++;
            }
        }

        
        summary.PassedTests = passedCount;
        summary.TotalTests = tests.Count;
        summary.FailedTests = summary.TotalTests - summary.PassedTests;

        return summary;
    }
}