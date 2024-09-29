using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace ChotNovel
{
    public class TextParser
    {
        // KAG Style
        public static char CommentSymbol = ';';
        public static char LabelSymbol = '*';
        public static char CommandSymbol = '@';
        public static char CommandStartSymbol = '[';
        public static char CommandEndSymbol = ']';
        public static char CommandParamSeparator = ' ';
        public static char CommandKeyValueSeparator = '=';

        public static void Parse(string source, List<TextElement> results)
        {
            results.Clear();
            using (var reader = new StringReader(source))
            {
                var line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    ParseLine(line, results);
                }
            }
        }

        private static StringBuilder _elementStringBuilder = new StringBuilder();
        private static void ParseLine(string line, List<TextElement> results)
        {
            line = RemoveComment(line);
            if (string.IsNullOrEmpty(line))
            {
                return;
            }

            if (line.StartsWith(LabelSymbol))
            {
                ParseLabel(line.Substring(1), results);
                return;
            }

            if (line.StartsWith(CommandSymbol))
            {
                ParseCommand(line.Substring(1), results);
                return;
            }

            _elementStringBuilder.Clear();
            var command = false;
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (!command && c == CommandStartSymbol)
                {
                    if (_elementStringBuilder.Length > 0)
                    {
                        results.Add(new TextElement(_elementStringBuilder.ToString(), TextElementType.Message));
                        _elementStringBuilder.Clear();
                    }
                    command = true;
                    continue;
                }
                if (command && c == CommandEndSymbol)
                {
                    ParseCommand(_elementStringBuilder.ToString(), results);
                    _elementStringBuilder.Clear();
                    command = false;
                    continue;
                }
                _elementStringBuilder.Append(c);
            }
            if (_elementStringBuilder.Length > 0)
            {
                results.Add(new TextElement(_elementStringBuilder.ToString(), TextElementType.Message));
            }
        }

        private static string RemoveComment(string source)
        {
            var index = source.IndexOf(CommentSymbol);
            if (index == -1)
            {
                return source;
            }
            return source.Substring(0, index);
        }

        private static void ParseLabel(string source, List<TextElement> results)
        {
            if (string.IsNullOrEmpty(source))
            {
                return;
            }
            results.Add(new TextElement(source, TextElementType.Label));
        }

        private static List<string> _commandSplitBuffer = new List<string>();
        private static void ParseCommand(string source, List<TextElement> results)
        {
            if (string.IsNullOrEmpty(source))
            {
                return;
            }
            _commandSplitBuffer.Clear();
            SplitCommand(source, _commandSplitBuffer);
            var commandName = _commandSplitBuffer[0];
            if (string.IsNullOrEmpty(commandName))
            {
                Debug.LogError($"Command name is empty: {source}");
                return;
            }
            var element = new TextElement(commandName, TextElementType.Command);
            for (var i = 1; i < _commandSplitBuffer.Count; i++)
            {
                ParseCommandParam(_commandSplitBuffer[i], element);
            }
            results.Add(element);
        }

        private static StringBuilder _commandSplitStringBuilder = new StringBuilder();
        private static void SplitCommand(string source, List<string> results)
        {
            results.Clear();
            _commandSplitStringBuilder.Clear();
            var quate = false;
            for (int i = 0; i < source.Length; i++)
            {
                var c = source[i];
                if (c == '"')
                {
                    quate = !quate;
                    continue;
                }
                if (quate)
                {
                    _commandSplitStringBuilder.Append(c);
                    continue;
                }
                if (c == CommandParamSeparator)
                {
                    results.Add(_commandSplitStringBuilder.ToString());
                    _commandSplitStringBuilder.Clear();
                    continue;
                }
                _commandSplitStringBuilder.Append(c);
            }
            if (_commandSplitStringBuilder.Length > 0)
            {
                results.Add(_commandSplitStringBuilder.ToString());
            }
        }

        private static void ParseCommandParam(string source, TextElement result)
        {
            if (string.IsNullOrEmpty(source))
            {
                return;
            }
            var split = source.Split(CommandKeyValueSeparator);
            if (split.Length == 1)
            {
                result.AddParameter(split[0], string.Empty);
            }
            else if (split.Length == 2)
            {
                result.AddParameter(split[0], split[1]);
            }
            else
            {
                Debug.LogError("Invalid command parameter: " + source);
            }
        }
    }
}
