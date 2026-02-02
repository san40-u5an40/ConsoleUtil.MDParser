internal class Program
{
    static async Task Main()
    {
        var services = new ServiceCollection()
            .AddSingleton<IParser<ContentOptions>, ContentParser>()
            .AddSingleton<IParser<LinksOptions>, LinksParser>()
            .AddTransient<ContentCLICommand>()
            .AddTransient<LinksCLICommand>()
            .BuildServiceProvider();

        await new CliApplicationBuilder()
            .AllowDebugMode(false)
            .AllowPreviewMode(false)
            .SetExecutableName("mdparse")
            .AddCommandsFromThisAssembly()
            .UseTypeActivator(services.GetRequiredService)
            .Build()
            .RunAsync();
    }
}