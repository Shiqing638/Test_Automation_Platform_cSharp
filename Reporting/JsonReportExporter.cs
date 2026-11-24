using System.Text.Json;
using TestFlow.Models;
using System.Text.Json.Serialization;

namespace TestFlow.Reporting;

public class JsonReportExporter
{
    public void Export(TestSummary summary, string filePath)
    {
        JsonSerializerOptions options = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = {new JsonStringEnumConverter()}
        };

        string json = JsonSerializer.Serialize(summary, options);

        File.WriteAllText(filePath, json);

    }
}