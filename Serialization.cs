using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace VoiceCommand.Serialization
{
    internal class Serialization
    {
        /// <summary>
        /// Load a list of predefined Commands from Json.
        /// </summary>
        /// <param name="commands">The list of Commands loaded from Json.</param>
        /// <returns>Whether or not loading from Json succeeded.</returns>
        public bool LoadCommandsFromJson(out Command[] commands)
        {
            commands = null;
            //var dialog = new 
            //var result = JsonSerializer.Deserialize<Command[]>()

            return false;
        }

        /// <summary>
        /// Save the supplied Command list to Json.
        /// </summary>
        /// <param name="commandList">The list of Commands to store.</param>
        /// <returns>Whether the saving to Json succeeded.</returns>
        public bool SaveToJson(Command[] commandList)
        {
            return false;
        }
    }
}