using Microsoft.Extensions.Configuration;
using System.Text.Json;

using VoiceCommand.Input;

namespace VoiceCommand;

public class ConfigurationManager
{
    private readonly IConfiguration _configuration;
    private readonly string _filePath;
    // private readonly Logger<ConfigurationManager> _logger;

    public VoiceCommandConfig VoiceCommandConfig { get; private set; }

    public ConfigurationManager(string configFilePath)
    {
        _filePath = configFilePath;
        // _logger = new();

        IConfiguration config = new ConfigurationBuilder()
            .SetFileLoadExceptionHandler((FileLoadExceptionContext handler) =>
            {
                // success = false;
                Log.LogToConsole(
                    $"An exception occured while loading {configFilePath}!\nException: {handler.Exception}", 
                    Log.LogType.Error
                );
            })
            .AddJsonFile(configFilePath, false, false)
            .Build();

        _configuration = config;
        VoiceCommandConfig = new();

        config.Bind(VoiceCommandConfig);
    }

    private void SaveConfig()
    {
        JsonSerializerOptions jsonSerializerOptions = new(){
            WriteIndented = true 
        };
    }
}