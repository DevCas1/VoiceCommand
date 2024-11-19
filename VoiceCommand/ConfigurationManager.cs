using Microsoft.Extensions.Configuration;
using System.Text.Json;

using VoiceCommand.Input;

namespace VoiceCommand;

public class ConfigurationManager
{
    private readonly IConfiguration _configuration;
    private readonly string _filePath;

    public VoiceCommandConfig VoiceCommandConfig { get; private set; }

    public ConfigurationManager(string configFilePath)
    {
        _filePath = configFilePath;

        IConfiguration config = new ConfigurationBuilder()
            .SetFileLoadExceptionHandler((FileLoadExceptionContext handler) =>
            {
                Log.Error($"An exception occured while loading {configFilePath}!\nException: {handler.Exception}");
            })
            .AddJsonFile(configFilePath, false, false)
            .Build();

        _configuration = config;
        VoiceCommandConfig = new();

        try
        {
            config.Bind(VoiceCommandConfig, options => { options.ErrorOnUnknownConfiguration = true; });
        }
        catch(InvalidOperationException exception)
        {
            Log.Error($"Could not bind VoiceCommandConfig! Reason: {exception.InnerException?.Message}\nExiting...");
            return;
        }
    }
}