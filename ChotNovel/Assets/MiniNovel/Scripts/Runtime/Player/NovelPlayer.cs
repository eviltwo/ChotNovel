using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MiniNovel.Player
{
    public class NovelPlayer : MonoBehaviour
    {
        private ITextContainer _textContainer;
        private List<NovelModule> _modules = new List<NovelModule>();
        private string _lastPlayedFileName;
        private List<TextElement> _textElementBuffer = new List<TextElement>();
        private List<TextElement> _textElements = new List<TextElement>();
        private CancellationTokenSource _playerCancellation;

        private void Awake()
        {
            _textContainer = GetComponent<ITextContainer>();
            if (_textContainer == null)
            {
                Debug.LogError("ITextContainer is not found.");
                enabled = false;
            }
        }

        public void Play(string label)
        {
            Play(_lastPlayedFileName, label);
        }

        public void Play(string fileName, string label)
        {
            if (_playerCancellation != null)
            {
                _playerCancellation.Cancel();
                _playerCancellation.Dispose();
                _playerCancellation = null;
            }

            _playerCancellation = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);
            PlayInternal(fileName, label, _playerCancellation.Token).Forget();
        }

        private async UniTask PlayInternal(string fileName, string label, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                Debug.LogError("File name is empty.");
                return;
            }

            if (string.IsNullOrEmpty(label))
            {
                Debug.LogError("Label is empty.");
                return;
            }

            _textElementBuffer.Clear();
            var success = await _textContainer.LoadTextElements(fileName, _textElementBuffer, cancellationToken);
            if (!success)
            {
                Debug.LogError($"Failed to get text elements from {fileName}.");
                return;
            }
            _textElements.Clear();
            success = PickLabeledTextElements(_textElementBuffer, label, _textElements);
            if (!success)
            {
                Debug.LogError($"Label {label} is not found.");
                return;
            }

            _lastPlayedFileName = fileName;
            await PlayTexts(_textElements, cancellationToken);
        }

        private async UniTask PlayTexts(IReadOnlyList<TextElement> textElements, CancellationToken cancellationToken)
        {
            var payload = new NovelModulePayload();
            payload.Player = this;
            foreach (var textElement in textElements)
            {
                await PlayModules(textElement, payload, cancellationToken);
            }
        }

        private async UniTask PlayModules(TextElement textElement, NovelModulePayload payload, CancellationToken cancellationToken)
        {
            var excuted = false;
            foreach (var module in _modules)
            {
                if (module.IsExecutable(textElement))
                {
                    excuted = true;
                    await module.Execute(textElement, payload, cancellationToken);
                }
            }

            if (!excuted && textElement.ElementType != TextElementType.Label)
            {
                Debug.LogWarning($"No module is executed for {textElement.Content}");
            }
        }

        public void RegisterModule(NovelModule module)
        {
            _modules.Add(module);
        }

        public void UnregisterModule(NovelModule module)
        {
            _modules.Remove(module);
        }

        public void ClearDisplayedObjects()
        {
            foreach (var module in _modules)
            {
                module.ClearDisplayedObjects();
            }
        }

        private static bool PickLabeledTextElements(List<TextElement> source, string label, List<TextElement> results)
        {
            results.Clear();
            var startIndex = source.FindIndex(x => x.ElementType == TextElementType.Label && x.Content == label);
            if (startIndex < 0)
            {
                return false;
            }
            var endIndex = source.FindIndex(startIndex + 1, x => x.ElementType == TextElementType.Label);
            if (endIndex == -1)
            {
                results.AddRange(source.GetRange(startIndex, source.Count - startIndex));
            }
            else
            {
                results.AddRange(source.GetRange(startIndex, endIndex - startIndex));
            }
            return true;
        }
    }
}
