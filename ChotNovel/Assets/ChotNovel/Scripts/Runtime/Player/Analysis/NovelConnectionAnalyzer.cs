using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ChotNovel.Player
{
    public class NovelConnectionAnalyzer
    {
        public class LabelAddress
        {
            public readonly string File;
            public readonly string Label;
            public LabelAddress(string file, string label)
            {
                File = file;
                Label = label;
            }
        }

        private class Connection
        {
            public readonly LabelAddress From;
            public readonly LabelAddress To;
            public Connection(LabelAddress from, LabelAddress to)
            {
                From = from;
                To = to;
            }
        }

        private readonly List<string> _commands = new List<string>();
        private readonly List<Connection> _connections = new List<Connection>();

        public void AddTargetCommands(string commandName)
        {
            _commands.Add(commandName);
        }

        private readonly List<TextElement> _textElementBuffer = new List<TextElement>();
        public void PushFileTexts(string file, IReadOnlyList<TextElement> textElements)
        {
            var labels = textElements
                .Where(v => v.ElementType == TextElementType.Label)
                .Select(v => v.Content);
            foreach (var label in labels)
            {
                _textElementBuffer.Clear();
                if (NovelPlayerUtility.PickLabeledTextElements(textElements, label, _textElementBuffer))
                {
                    PushLabeledSection(file, _textElementBuffer);
                }
            }
        }

        public void PushLabeledSection(string file, IReadOnlyList<TextElement> textElements)
        {
            var label = textElements.FirstOrDefault(v => v.ElementType == TextElementType.Label);
            if (label == null)
            {
                Debug.LogError("Label not found in labeled section.");
                return;
            }

            var jumpElements = textElements
                .Where(v => v.ElementType == TextElementType.Command)
                .Where(v => _commands.Contains(v.Content));
            foreach (var jumpElement in jumpElements)
            {
                var from = new LabelAddress(file, label.Content);
                var to = new LabelAddress(
                    jumpElement.TryGetStringParameter("file", out var fileValue) ? fileValue : file,
                    jumpElement.TryGetStringParameter("label", out var labelValue) ? labelValue : string.Empty);
                if (string.IsNullOrEmpty(to.Label))
                {
                    Debug.LogError($"label not found in command. file:{file}, command:{jumpElement.Content}");
                    continue;
                }
                _connections.Add(new Connection(from, to));
            }
        }

        public List<LabelAddress> GetPreviousLabels(string file, string label)
        {
            var results = new List<LabelAddress>();
            GetPreviousLabels(file, label, results);
            return results;
        }

        public void GetPreviousLabels(string file, string label, List<LabelAddress> results)
        {
            results.Clear();
            var connectedLabels = _connections
                .Where(v => v.To.File == file && v.To.Label == label)
                .Select(v => v.From);
            results.AddRange(connectedLabels);
        }

        public List<LabelAddress> GetNextLabels(string file, string label)
        {
            var results = new List<LabelAddress>();
            GetNextLabels(file, label, results);
            return results;
        }

        public void GetNextLabels(string file, string label, List<LabelAddress> results)
        {
            results.Clear();
            var connectedLabels = _connections
                .Where(v => v.From.File == file && v.From.Label == label)
                .Select(v => v.To);
            results.AddRange(connectedLabels);
        }
    }
}
