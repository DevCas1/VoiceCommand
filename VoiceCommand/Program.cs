﻿using Microsoft.Extensions.Configuration;
using VoiceCommand.Input;

namespace VoiceCommand;

internal class Program
{
    private static ConfigurationManager _configurationManager;
    private static RecognitionHandler? _recognitionHandler;

    private static void Main() // Add (string[] args) to use command line arguments when starting the program
    {
        const string CONFIG_FILE_NAME = "vcconfig.json";
        const string CONFIG_VERSION = "1";

        _configurationManager = new ConfigurationManager(CONFIG_FILE_NAME);

        try
        {
            if (_configurationManager.VoiceCommandConfig.Version != CONFIG_VERSION)
            {
                string errorMessage = $"Config version does not match expected version! (Expected: {CONFIG_VERSION}, detected: {_configurationManager.VoiceCommandConfig.Version})" +
                    "\nEither restore original config file, or have a new one generated by renaming the incorrect one!";
                Log.LogToConsole(errorMessage, Log.LogType.Error);
                throw new ApplicationException(message: errorMessage);
            }
        }
        catch (InvalidOperationException)
        {
            Log.LogToConsole("Config version could not be parsed or found!", Log.LogType.Error);
            return;
        }

        _recognitionHandler = new RecognitionHandler(_configurationManager.VoiceCommandConfig);
        _recognitionHandler.Start();

        Console.WriteLine();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
