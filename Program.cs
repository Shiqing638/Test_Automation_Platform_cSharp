using TestFlow.Models;
using TestFlow.Services;
class Program
{
    static void Main(string[] args)
    {
        List<TestResult> tests = new()
        {
            new TestResult
            {
                TestName = "Voltage Test 1",
                MeasuredValue = 4.5,
                MaxLimit = 5.0
            },
            new TestResult
            {
                TestName = "Voltage Test 2",
                MeasuredValue = 5.3,
                MaxLimit = 5.0
            },
            new TestResult
            {
                TestName = "Voltage Test 3",
                MeasuredValue = 4.8,
                MaxLimit = 5.0
            },            
        };

        TestRunner runner = new TestRunner();
        TestSummary summary = runner.Run(tests);

        for (int i = 0; i < tests.Count; i ++)
        {
            string status = summary.Results[i] ? "PASS": "FAIL";

            Console.WriteLine($"{tests[i].TestName}: {status}");
        }

        Console.WriteLine();
        Console.WriteLine($"Total tests: {summary.TotalTests}");
        Console.WriteLine($"Passed tests: {summary.PassedTests}");
        Console.WriteLine($"Failed tests: {summary.FailedTests}");



    }
}
