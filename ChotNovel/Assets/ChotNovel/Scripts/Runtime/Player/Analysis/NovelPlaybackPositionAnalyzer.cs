using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ChotNovel.Player
{
    public class NovelPlaybackPositionAnalyzer
    {
        public class Result
        {
            public bool Success;
            public string StartFile;
            public string StartLabel;
            public int StartStep;
            public List<LabelAddress> JumpTargets;
        }

        public class LabelAddress
        {
            public string File;
            public string Label;
        }

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

        public async UniTask<Result> Analyze(string file, string label, int step, ITextContainer textContainer, CancellationToken cancellationToken)
        {
            var fileElements = new List<TextElement>();
            var pickedElements = new List<TextElement>();
            var isSuccessPick = await textContainer.LoadTextElements(file, fileElements, cancellationToken)
                && NovelPlayerUtility.PickLabeledTextElements(fileElements, label, pickedElements)
                && pickedElements.Count > step;
            if (!isSuccessPick)
            {
                return new Result { Success = false };
            }

            var hasClearCommand = step == 0
                && pickedElements.Count >= 2
                && pickedElements[1].ElementType == TextElementType.Command
                && pickedElements[1].Content == _clearCommand;
            if (hasClearCommand)
            {
                return new Result
                {
                    Success = true,
                    StartFile = file,
                    StartLabel = label,
                    StartStep = step,
                    JumpTargets = new List<LabelAddress>()
                };
            }

            var jumpTargets = new List<LabelAddress>();

            // Search clear command from current label section.
            var foundIndex = FindClearCommandBefore(pickedElements, step);
            if (foundIndex > 0)
            {
                return new Result
                {
                    Success = true,
                    StartFile = file,
                    StartLabel = label,
                    StartStep = foundIndex,
                    JumpTargets = jumpTargets
                };
            }

            // Search previous label from current file.
            jumpTargets.Add(new LabelAddress { File = file, Label = label });
            var connectionAnalyzer = new NovelConnectionAnalyzer();
            connectionAnalyzer.AddTargetCommands(_jumpCommands);
            connectionAnalyzer.PushFileTexts(file, fileElements);
            var previousLabels = connectionAnalyzer.GetPreviousLabels(file, label);
            if (previousLabels.Count == 0)
            {
                return new Result { Success = false };
            }
            var previousLabel = previousLabels[0];
            var previousElements = new List<TextElement>();
            if (!NovelPlayerUtility.PickLabeledTextElements(fileElements, previousLabel.Label, previousElements))
            {
                return new Result { Success = false };
            }
            var jumpElementStep = previousElements.FindIndex(v =>
                    v.ElementType == TextElementType.Command
                    && _jumpCommands.Contains(v.Content)
                    && v.TryGetStringParameter("label", out var labelValue)
                    && labelValue == label);
            if (jumpElementStep == -1)
            {
                return new Result { Success = false };
            }
            var previousFoundIndex = FindClearCommandBefore(previousElements, jumpElementStep);
            if (previousFoundIndex > 0)
            {
                return new Result
                {
                    Success = true,
                    StartFile = file,
                    StartLabel = previousLabel.Label,
                    StartStep = previousFoundIndex,
                    JumpTargets = jumpTargets
                };
            }

            // TODO: Search previous label from other files.
            return new Result { Success = false };
        }

        public int FindClearCommandBefore(List<TextElement> elements, int startIndex)
        {
            if (startIndex < 0 || startIndex >= elements.Count)
            {
                return -1;
            }
            for (int i = startIndex; i >= 0; i--)
            {
                var element = elements[i];
                if (element.ElementType == TextElementType.Command && element.Content == _clearCommand)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
