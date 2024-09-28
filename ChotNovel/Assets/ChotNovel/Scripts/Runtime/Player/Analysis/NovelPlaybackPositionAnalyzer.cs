using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

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

            // Search clear command and collect elements from same label section.
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

            var fileNames = new List<string>();
            if (!await textContainer.GetAllFileName(fileNames, cancellationToken))
            {
                return false;
            }

            var targetFile = file;
            var targetLabel = label;
            const int maxItr = 10;
            for (int itr = 0; itr < maxItr; itr++)
            {
                // Serach previous label from same file.
                var connectionAnalyzer = new NovelConnectionAnalyzer();
                connectionAnalyzer.AddTargetCommands(_jumpCommands);
                connectionAnalyzer.PushFileTexts(targetFile, fileElements);
                var previousLabels = connectionAnalyzer.GetPreviousLabels(targetFile, targetLabel);
                if (previousLabels.Count > 0)
                {
                    var previousLabel = previousLabels[0];
                    // Search clear command and collect elements.
                    // Not allow including jump command.
                    if (!CollectElementsAndSearchClearCommand(fileElements, previousLabel.Label, targetLabel, results, out hasClearCommand))
                    {
                        return false;
                    }
                    if (hasClearCommand)
                    {
                        return true;
                    }
                    // Itr continue
                    targetFile = previousLabel.File;
                    targetLabel = previousLabel.Label;
                    continue;
                }

                // Search previous label from other files.
                {
                    var foundRef = false;
                    var previousFile = string.Empty;
                    var previousLabel = string.Empty;
                    foreach (var otherFile in fileNames)
                    {
                        if (otherFile == targetFile)
                        {
                            continue;
                        }
                        // Update file elements.
                        if (!await textContainer.LoadTextElements(otherFile, fileElements, cancellationToken))
                        {
                            return false;
                        }
                        connectionAnalyzer = new NovelConnectionAnalyzer();
                        connectionAnalyzer.AddTargetCommands(_jumpCommands);
                        connectionAnalyzer.PushFileTexts(otherFile, fileElements);
                        previousLabels = connectionAnalyzer.GetPreviousLabels(targetFile, targetLabel);
                        if (previousLabels.Count > 0)
                        {
                            foundRef = true;
                            previousFile = previousLabels[0].File;
                            previousLabel = previousLabels[0].Label;
                            break;
                        }
                    }
                    if (!foundRef)
                    {
                        return false;
                    }

                    // Search clear and collect elements.
                    if (!CollectElementsAndSearchClearCommand(fileElements, previousLabel, targetLabel, results, out hasClearCommand))
                    {
                        return false;
                    }
                    if (hasClearCommand)
                    {
                        return true;
                    }
                    // Itr continue
                    targetFile = previousFile;
                    targetLabel = previousLabel;
                    continue;
                }
            }

            return false;
        }

        private bool CollectElementsAndSearchClearCommand(IReadOnlyList<TextElement> fileElements, string previousLabel, string targetLabel, List<TextElement> insertResult, out bool hasClearCommand)
        {
            var previousElements = new List<TextElement>();
            if (!NovelPlayerUtility.PickLabeledTextElements(fileElements, previousLabel, previousElements))
            {
                hasClearCommand = false;
                return false;
            }
            var jumpElementStep = previousElements.FindIndex(v =>
                v.ElementType == TextElementType.Command
                && _jumpCommands.Contains(v.Content)
                && v.TryGetStringParameter("label", out var labelValue)
                && labelValue == targetLabel);
            if (jumpElementStep == -1)
            {
                Debug.LogWarning($"Failed to find jump command. label:{targetLabel}");
                jumpElementStep = previousElements.Count - 1;
            }
            for (int i = jumpElementStep; i >= 0; i--)
            {
                var element = previousElements[i];
                if (element.ElementType == TextElementType.Command && _jumpCommands.Contains(element.Content))
                {
                    continue;
                }
                insertResult.Insert(0, element);
                if (element.ElementType == TextElementType.Command && element.Content == _clearCommand)
                {
                    hasClearCommand = true;
                    return true;
                }
            }

            hasClearCommand = false;
            return true;
        }
    }
}
