using Microsoft.Data.SqlClient;
using TestFlow.Models;
using Microsoft.Extensions.Logging;

namespace TestFlow.Database;

public class TestRunRepository
{
    private readonly SqlConnectionFactory _connectionFactory;
    private readonly ILogger<TestRunRepository> _logger;

    public TestRunRepository(SqlConnectionFactory connectionFactory,
        ILogger<TestRunRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public int Save(string suiteName, TestSummary summary)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var transaction = connection.BeginTransaction();

        int runId;

        try
        {
            string sql = """
                INSERT INTO dbo.TestRuns(
                    SuiteName,
                    TotalTests,
                    PassedTests,
                    FailedTests,
                    ErrorTests,
                    Status
                )
                OUTPUT INSERTED.RunId
                VALUES (
                    @SuiteName,
                    @TotalTests,
                    @PassedTests,
                    @FailedTests,
                    @ErrorTests,
                    @Status
                );
                """;

            using var command = new SqlCommand(sql, connection, transaction);

            command.Parameters.AddWithValue("@SuiteName", suiteName);
            command.Parameters.AddWithValue("@TotalTests", summary.TotalTests);
            command.Parameters.AddWithValue("@PassedTests", summary.PassedTests);
            command.Parameters.AddWithValue("@FailedTests", summary.FailedTests);
            command.Parameters.AddWithValue("@ErrorTests", summary.ErrorTests);
            command.Parameters.AddWithValue("@Status", summary.Status.ToString());

            runId = (int)command.ExecuteScalar()!;

            string resultSql = """
                INSERT INTO dbo.TestResults (
                    RunId,
                    TestName,
                    Status,
                    ErrorMessage
                )
                VALUES (
                    @RunId,
                    @TestName,
                    @Status,
                    @ErrorMessage
                );
                """;
            foreach (var result in summary.Results)
            {
                using var resultCommand = new SqlCommand(
                    resultSql, connection, transaction
                );

                resultCommand.Parameters.AddWithValue("@RunId", runId);
                resultCommand.Parameters.AddWithValue("@TestName", result.TestName);
                resultCommand.Parameters.AddWithValue("@Status", result.Status.ToString());
                resultCommand.Parameters.AddWithValue(
                    "@ErrorMessage", (object?)result.ErrorMessage ?? DBNull.Value);
                
                resultCommand.ExecuteNonQuery();
            }

            transaction.Commit();                   
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, 
                "Failed to save test suite {SuiteName}.",
                suiteName
            );

            transaction.Rollback();
            throw;
        }

        _logger.LogInformation(
            "Test run {RunId} saved successfully.",
            runId
        );

        return runId;
    }

    public List<TestRunRecord> GetAllRuns()
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        string sql = """
            SELECT RunId, SuiteName, ExecutedAt,
                TotalTests, PassedTests, FailedTests,
                ErrorTests, Status
            FROM dbo.TestRuns
            ORDER BY ExecutedAt DESC;
            """;
        
        using var command = new SqlCommand(sql, connection);
        using var reader = command.ExecuteReader();

        List<TestRunRecord> runs = new();

        while (reader.Read())
        {
            TestRunRecord run = new()
            {
                RunId = reader.GetInt32(0),
                SuiteName = reader.GetString(1),
                ExecutedAt = reader.GetDateTime(2),
                TotalTests = reader.GetInt32(3),
                PassedTests = reader.GetInt32(4),
                FailedTests = reader.GetInt32(5),
                ErrorTests = reader.GetInt32(6),
                Status = reader.GetString(7)
            };

            runs.Add(run);
        }

        return runs;
    }
}