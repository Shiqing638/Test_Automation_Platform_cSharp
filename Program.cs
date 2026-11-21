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
        



    }
}
