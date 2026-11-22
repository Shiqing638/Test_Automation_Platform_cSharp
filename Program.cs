using System.Diagnostics;
using TestFlow.Models;
using TestFlow.Services;
class Program
{
    static void Main(string[] args)
    {
        List<ITestCase> tests = new()
        {
            new RangeTest
            {
                TestName = "Voltage Test 1",
                MeasuredValue = 4.5,
                MaxLimit = 5.0,
                MinLimit = 6.6
            },
            new RangeTest
            {
                TestName = "Voltage Test 2",
                MeasuredValue = 5.3,
                MaxLimit = 5.0,
                MinLimit = 4.6
            },
            new RangeTest
            {
                TestName = "Voltage Test 3",
                MeasuredValue = 4.8,
                MaxLimit = 5.0,
                MinLimit = 4.6
            },  
            new ConnectionTest
            {
                TestName = "Device Connection Test",
                IsConnected = false
            }          
        };

        TestRunner runner = new TestRunner();

        try
        {
            TestSummary summary = runner.Run(tests);
            for (int i = 0; i < summary.Results.Count; i ++)
            {
                TestExecutionResult result = summary.Results[i];
                Console.WriteLine(
                    $"{result.TestName}: {result.Status}"
                );

                if (result.Status == TestExecutionStatus.Error)
                {
                    Console.WriteLine($"Reason: {result.ErrorMessage}");
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Total tests: {summary.TotalTests}");
            Console.WriteLine($"Passed tests: {summary.PassedTests}");
            Console.WriteLine($"Failed tests: {summary.FailedTests}");
            Console.WriteLine($"Error tests: {summary.ErrorTests}");

            var failedNames = summary.Results
            .Where(result => result.Status == TestExecutionStatus.Failed)
            .Select(result => result.TestName)
            .ToList();

            Console.WriteLine("Failed test names:");

            foreach (string failedName in failedNames)
            {
                Console.WriteLine(failedName);
            }

            TestSuiteStatus suiteStatus;

            if(summary.TotalTests == 0)
            {
                suiteStatus = TestSuiteStatus.NoTests;
            }
            else if (summary.FailedTests == 0 && summary.ErrorTests == 0)
            {
                suiteStatus = TestSuiteStatus.Passed;
            }
            else
            {
                suiteStatus = TestSuiteStatus.Failed;
            }

            Console.WriteLine($"Test suite status: {suiteStatus}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Configuration error: {ex.Message}");
        }

        ITestCase test = new RangeTest
        {
            TestName = "Voltage Test",
            MeasuredValue = 4.8,
            MinLimit = 4.5,
            MaxLimit = 5.0
        };

        Console.WriteLine(test.IsPassed());
    }
}
