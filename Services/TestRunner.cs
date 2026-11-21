using System.Collections.Generic;
using TestFlow.Models;
using System;

namespace TestFlow.Services;

public class TestRunner
{
    public void Run(List<TestResult> tests)
    {
        int passedCount = 0;
        foreach (TestResult test in tests)
        {
            bool passed = test.IsPassed();

            if(passed)
            {
                passedCount++;
                Console.WriteLine($"{test.TestName}: PASS");
            }
            else
            {
                Console.WriteLine($"{test.TestName}: FAIL");
            }
        }
        Console.WriteLine();
        Console.WriteLine($"Total tests: {tests.Count}");
        Console.WriteLine($"Passed tests: {passedCount}");
    }
}