using System.Diagnostics;
using TestFlow.Models;
using TestFlow.Services;
using TestFlow.Reporting;
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
            PrintSummary(summary);

            JsonReportExporter exporter = new();
            exporter.Export(summary, "test-report.json");            
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Configuration error: {ex.Message}");
        }

    }
    static void PrintSummary(TestSummary summary)
        {
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

            Console.WriteLine($"Test suite status: {summary.Status}");
            
        }
}
