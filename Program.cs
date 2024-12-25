using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

public class Program
{
    public static async Task Main()
    {
        await TryingOutKernel.Execute();
    }
}

public class TryingOutKernel
{


    private static string model = "gpt-4";
    private static string endpoint = "https://linkedin-test.openai.azure.com/";
    private static string apikey = string.Empty;

    public static async Task Execute()
    {
        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("secrets.json", optional: true, reloadOnChange: true)
            .AddUserSecrets<Program>();

        IConfiguration configuration = configBuilder.Build();
        apikey = configuration["openAi"];

        var builder = Kernel.CreateBuilder();

        builder.Services.AddAzureOpenAIChatCompletion(model, endpoint, apikey);

        var kernel = builder.Build();

        var topic = "What is the meaning of life?";

        var prompt = "generate a short and concise answer about given topic. " +
            "Make sure to give famous citations from Omar Khayyam about given topic. " +
            $"topic: {topic}";

        var response = await kernel.InvokePromptAsync(prompt);

        Console.WriteLine(response);
    }
}
