using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace MiniNovel
{
    public class TextParser
    {
        // KAG Style
        public static char LabelSymbol = '*';
        public static char CommandStartSymbol = '[';
        public static char CommandEndSymbol = ']';
        public static char CommandParamSeparator = ' ';
        public static char CommandKeyValueSeparator = '=';

        private static StringBuilder _messageBuffer = new StringBuilder();
        private static StringBuilder _commandBuffer = new StringBuilder();

        public static void Parse(string source, List<TextElement> results)
        {
            using (var reader = new StringReader(source))
            {
                var line = string.Empty;
                _messageBuffer.Clear();
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith(LabelSymbol))
                    {
                        if (_messageBuffer.Length > 0)
                        {
                            results.Add(new TextElement(_messageBuffer.ToString(), TextElementType.Message));
                            _messageBuffer.Clear();
                        }
                        // TODO: Add label parameters
                        results.Add(new TextElement(line.Substring(1), TextElementType.Label));
                    }
                    else
                    {
                        _commandBuffer.Clear();
                        var charIndex = 0;
                        while (charIndex < line.Length)
                        {
                            if (line[charIndex] == CommandStartSymbol)
                            {
                                if (_messageBuffer.Length > 0)
                                {
                                    results.Add(new TextElement(_messageBuffer.ToString(), TextElementType.Message));
                                    _messageBuffer.Clear();
                                }

                                charIndex++;
                                while (charIndex < line.Length)
                                {
                                    if (line[charIndex] == CommandEndSymbol && _commandBuffer.Length > 0)
                                    {
                                        results.Add(ParseCommand(_commandBuffer.ToString()));
                                        _commandBuffer.Clear();
                                        break;
                                    }
                                    else
                                    {
                                        _commandBuffer.Append(line[charIndex]);
                                        charIndex++;
                                    }
                                }
                                if (_commandBuffer.Length > 0)
                                {
                                    Debug.LogError("Command not closed: " + _commandBuffer.ToString());
                                }
                            }
                            else
                            {
                                _messageBuffer.Append(line[charIndex]);
                            }
                            charIndex++;
                        }
                    }
                }
            }
        }

        private static TextElement ParseCommand(string commandSource)
        {
            var splitResults = commandSource.Split(CommandParamSeparator);
            if (splitResults.Length == 0)
            {
                return new TextElement(string.Empty, TextElementType.Command);
            }

            var element = new TextElement(splitResults[0], TextElementType.Command);
            for (var i = 1; i < splitResults.Length; i++)
            {
                if (string.IsNullOrEmpty(splitResults[i]))
                {
                    continue;
                }
                var keyValue = splitResults[i].Split(CommandKeyValueSeparator);
                if (keyValue.Length == 2)
                {
                    element.AddParameter(keyValue[0], keyValue[1]);
                }
                else
                {
                    Debug.LogError("Invalid command parameter: " + splitResults[i]);
                }
            }
            return element;
        }
    }
}
