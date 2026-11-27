using System.Diagnostics;
using TestFlow.Models;
using TestFlow.Services;
using TestFlow.Reporting;
using TestFlow.Configuration;
using System.Text.Json;
using TestFlow.Database;
using Microsoft.Extensions.Logging;


class Program
{
    static void Main(string[] args)
    {
        try
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Information);
            });

            ILogger<TestRunRepository> repositoryLogger = 
                loggerFactory.CreateLogger<TestRunRepository>();

            string connectionString =
                Environment.GetEnvironmentVariable("TESTFLOW_DB_CONNECTION")
                ?? throw new InvalidOperationException(
                    "Database connection string is missing."
                );

            SqlConnectionFactory factory = new(connectionString);

            // 1. load configuration
            TestConfigurationLoader loader = new();
            TestSuiteConfig config = loader.Load("tests.json");

            // 2. Validate configuration
            TestConfigurationValidator validator = new();
            validator.Validate(config);

            // 3. Map configuration to test cases
            TestCaseMapper mapper = new();
            List<ITestCase> tests = mapper.MapAll(config);
            
            // 4. Execute tests
            Console.WriteLine($"Suite: {config.SuiteName}");
            TestRunner runner = new TestRunner();
            TestSummary summary = runner.Run(tests);

            TestRunRepository repository = new(factory, repositoryLogger);
            int runId = repository.Save(config.SuiteName, summary);

            List<TestRunRecord> runs = repository.GetAllRuns();

            Console.WriteLine("\nTest History:");

            foreach (var run in runs)
            {
                Console.WriteLine(
                    $"{run.RunId} |  {run.SuiteName}" + 
                    $"{run.ExecutedAt} | {run.Status}"
                );
            }

            Console.WriteLine($"Test run saved. RunId: {runId}");


            // 5. Print results
            PrintSummary(summary);

            // 6. Export report
            JsonReportExporter exporter = new();
            exporter.Export(summary, "test-report.json");        
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"Configuration error: {ex.Message}");
        }
        catch (JsonException ex)
        {
            Console.Error.WriteLine(
                $"Invalid Json: {ex.Message}"
            );
        }
        catch (InvalidDataException ex)
        {
            Console.Error.WriteLine(
                $"Invalid configuration: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.Error.WriteLine(
                $"File operation failed: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.Error.WriteLine(
                $"Access denied: {ex.Message}");
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
