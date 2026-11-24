using System.Text.Json;

namespace TestFlow.Configuration;

public class TestConfigurationLoader
{
    public TestSuiteConfig Load(string filePath)
    {
        string json = File.ReadAllText(filePath);

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        TestSuiteConfig? config = 
            JsonSerializer.Deserialize<TestSuiteConfig>(json, options);
        
        if (config == null)
        {
            throw new InvalidDataException("Test configuration is null.");
        }

        return config;
    }
}