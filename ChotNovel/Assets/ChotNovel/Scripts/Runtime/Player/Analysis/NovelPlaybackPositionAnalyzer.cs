using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ChotNovel.Player
{
    public class NovelPlaybackPositionAnalyzer
    {
        private string _clearCommand = "clear";
        private List<string> _jumpCommands = new List<string>();

        public void AddJumpCommand(string commandName)
        {
            _jumpCommands.Add(commandName);
        }

        public void AddJumpCommands(IReadOnlyList<string> commandNames)
        {
            foreach (var commandName in commandNames)
            {
                AddJumpCommand(commandName);
            }
        }

        public async UniTask<bool> CollectPreviousTextElements(string file, string label, int step, ITextContainer textContainer, List<TextElement> results, CancellationToken cancellationToken)
        {
            results.Clear();
            var fileElements = new List<TextElement>();
            var pickedElements = new List<TextElement>();
            var isSuccessPick = await textContainer.LoadTextElements(file, fileElements, cancellationToken)
                && NovelPlayerUtility.PickLabeledTextElements(fileElements, label, pickedElements)
                && pickedElements.Count > step;
            if (!isSuccessPick)
            {
                return false;
            }

            var hasClearCommand = step == 0
                && pickedElements.Count >= 2
                && pickedElements[1].ElementType == TextElementType.Command
                && pickedElements[1].Content == _clearCommand;
            if (hasClearCommand)
            {
                return true;
            }

            // Search clear command from current label section.
            // Allow including jump command.
            for (int i = step - 1; i >= 0; i--)
            {
                var element = pickedElements[i];
                results.Insert(0, element);
                if (element.ElementType == TextElementType.Command && element.Content == _clearCommand)
                {
                    return true;
                }
            }

            // Search previous label from current file.
            // Not allow including jump command.
            var connectionAnalyzer = new NovelConnectionAnalyzer();
            connectionAnalyzer.AddTargetCommands(_jumpCommands);
            connectionAnalyzer.PushFileTexts(file, fileElements);
            var previousLabels = connectionAnalyzer.GetPreviousLabels(file, label);
            if (previousLabels.Count == 0)
            {
                return false;
            }
            var previousLabel = previousLabels[0];
            var previousElements = new List<TextElement>();
            if (!NovelPlayerUtility.PickLabeledTextElements(fileElements, previousLabel.Label, previousElements))
            {
                return false;
            }
            var jumpElementStep = previousElements.FindIndex(v =>
                    v.ElementType == TextElementType.Command
                    && _jumpCommands.Contains(v.Content)
                    && v.TryGetStringParameter("label", out var labelValue)
                    && labelValue == label);
            if (jumpElementStep == -1)
            {
                return false;
            }
            for (int i = jumpElementStep; i >= 0; i--)
            {
                var element = previousElements[i];
                if (element.ElementType == TextElementType.Command && _jumpCommands.Contains(element.Content))
                {
                    continue;
                }
                results.Insert(0, element);
                if (element.ElementType == TextElementType.Command && element.Content == _clearCommand)
                {
                    return true;
                }
            }

            // TODO: Search previous label from other files.
            return false;
        }
    }
}
