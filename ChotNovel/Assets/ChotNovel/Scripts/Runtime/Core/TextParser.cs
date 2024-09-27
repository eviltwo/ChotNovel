using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace ChotNovel
{
    public class TextParser
    {
        // KAG Style
        public static char LabelSymbol = '*';
        public static char LabelParamSeparator = '|';
        public static char CommandSymbol = '@';
        public static char CommandStartSymbol = '[';
        public static char CommandEndSymbol = ']';
        public static char CommandParamSeparator = ' ';
        public static char CommandKeyValueSeparator = '=';

        private static StringBuilder _messageStringBuilder = new StringBuilder();
        private static StringBuilder _commandStringBuilder = new StringBuilder();

        public static void Parse(string source, List<TextElement> results)
        {
            using (var reader = new StringReader(source))
            {
                var line = string.Empty;
                _messageStringBuilder.Clear();
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    if (line.StartsWith(LabelSymbol))
                    {
                        if (_messageStringBuilder.Length > 0)
                        {
                            results.Add(new TextElement(_messageStringBuilder.ToString(), TextElementType.Message));
                            _messageStringBuilder.Clear();
                        }
                        // TODO: Add label parameters
                        results.Add(ParseLabel(line.Substring(1)));
                    }
                    else if (line.StartsWith(CommandSymbol))
                    {
                        if (_messageStringBuilder.Length > 0)
                        {
                            results.Add(new TextElement(_messageStringBuilder.ToString(), TextElementType.Message));
                            _messageStringBuilder.Clear();
                        }
                        results.Add(ParseCommand(line.Substring(1)));
                    }
                    else
                    {
                        _commandStringBuilder.Clear();
                        var charIndex = 0;
                        while (charIndex < line.Length)
                        {
                            if (line[charIndex] == CommandStartSymbol)
                            {
                                if (_messageStringBuilder.Length > 0)
                                {
                                    results.Add(new TextElement(_messageStringBuilder.ToString(), TextElementType.Message));
                                    _messageStringBuilder.Clear();
                                }

                                charIndex++;
                                while (charIndex < line.Length)
                                {
                                    if (line[charIndex] == CommandEndSymbol && _commandStringBuilder.Length > 0)
                                    {
                                        results.Add(ParseCommand(_commandStringBuilder.ToString()));
                                        _commandStringBuilder.Clear();
                                        break;
                                    }
                                    else
                                    {
                                        _commandStringBuilder.Append(line[charIndex]);
                                        charIndex++;
                                    }
                                }
                                if (_commandStringBuilder.Length > 0)
                                {
                                    Debug.LogError("Command not closed: " + _commandStringBuilder.ToString());
                                }
                            }
                            else
                            {
                                _messageStringBuilder.Append(line[charIndex]);
                            }
                            charIndex++;
                        }
                    }
                }
                if (_messageStringBuilder.Length > 0)
                {
                    results.Add(new TextElement(_messageStringBuilder.ToString(), TextElementType.Message));
                    _messageStringBuilder.Clear();
                }
            }
        }

        private static TextElement ParseLabel(string labelSource)
        {
            var splitResults = labelSource.Split(LabelParamSeparator);
            if (splitResults.Length != 2)
            {
                return new TextElement(labelSource, TextElementType.Label);
            }

            var element = new TextElement(splitResults[0], TextElementType.Label);
            element.AddParameter("name", splitResults[1]);
            return element;
        }

        private static List<string> _commandParamBuffer = new List<string>();
        private static StringBuilder _commandParamStringBuilder = new StringBuilder();
        private static TextElement ParseCommand(string commandSource)
        {
            var splitResults = commandSource.Split(CommandParamSeparator);
            if (splitResults.Length == 0)
            {
                return new TextElement(string.Empty, TextElementType.Command);
            }

            _commandParamBuffer.Clear();
            _commandParamStringBuilder.Clear();
            var quote = false;
            for (int i = 0; i < splitResults.Length; i++)
            {
                var param = splitResults[i];
                if (quote)
                {
                    _commandParamStringBuilder.Append(CommandParamSeparator);
                    if (param.EndsWith("\""))
                    {
                        _commandParamStringBuilder.Append(param.Replace("\"", ""));
                        _commandParamBuffer.Add(_commandParamStringBuilder.ToString());
                        _commandParamStringBuilder.Clear();
                        quote = false;
                    }
                    else
                    {
                        _commandParamStringBuilder.Append(param.Replace("\"", ""));
                    }
                }
                else
                {
                    var quateCount = param.Length - param.Replace("\"", "").Length;
                    if (quateCount == 1)
                    {
                        quote = true;
                        _commandParamStringBuilder.Append(param.Replace("\"", ""));
                    }
                    else
                    {
                        _commandParamBuffer.Add(param.Replace("\"", ""));
                    }
                }
            }

            var element = new TextElement(splitResults[0], TextElementType.Command);
            for (var i = 1; i < _commandParamBuffer.Count; i++)
            {
                var param = _commandParamBuffer[i];
                if (string.IsNullOrEmpty(param))
                {
                    continue;
                }
                var keyValue = param.Split(CommandKeyValueSeparator);
                if (keyValue.Length == 1)
                {
                    element.AddParameter(keyValue[0], string.Empty);
                }
                else if (keyValue.Length == 2)
                {
                    element.AddParameter(keyValue[0], keyValue[1]);
                }
                else
                {
                    Debug.LogError("Invalid command parameter: " + param);
                }
            }
            return element;
        }
    }
}
