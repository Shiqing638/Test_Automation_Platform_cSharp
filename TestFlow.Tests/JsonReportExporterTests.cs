using TestFlow.Models;
using TestFlow.Reporting;
using System.Text.Json;

namespace TestFlow.tests;

public class JsonReportExporterTests
{
    [Fact]
    public void Export_WhenCalled_CreatesJsonFile()
    {
        TestSummary summary = new()
        {
            TotalTests = 1,
            PassedTests = 1
        };

        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"test-report-{Guid.NewGuid()}.json"
        );

        JsonReportExporter exporter = new();

        try
        {
            exporter.Export(summary, filePath);

            Assert.True(File.Exists(filePath));

            string json = File.ReadAllText(filePath);
            using JsonDocument document = JsonDocument.Parse(json);
            JsonElement root = document.RootElement;

            Assert.Equal(1, root.GetProperty("totalTests").GetInt32());
            Assert.Equal(1, root.GetProperty("passedTests").GetInt32());
            Assert.Equal("Passed", root.GetProperty("status").GetString());
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}