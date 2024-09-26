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
        private string _currentFileName;
        private List<TextElement> _textElements;
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
            Play(_currentFileName, label);
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

            if (_currentFileName != fileName)
            {
                _currentFileName = fileName;
                _textElements = new List<TextElement>();
                var success = await _textContainer.LoadTextElements(fileName, _textElements, cancellationToken);
                if (!success)
                {
                    Debug.LogError($"Failed to get text elements from {fileName}.");
                    return;
                }
            }

            var labelIndex = _textElements.FindIndex(x => x.ElementType == TextElementType.Label && x.Content == label);
            if (labelIndex < 0)
            {
                Debug.LogError($"Label {label} is not found.");
                return;
            }

            await PlayTexts(labelIndex + 1, cancellationToken);
        }

        private async UniTask PlayTexts(int textElementIndex, CancellationToken cancellationToken)
        {
            var payload = new NovelModulePayload();
            payload.Player = this;
            for (int i = textElementIndex; i < _textElements.Count; i++)
            {
                var textElement = _textElements[i];
                await PlayModules(textElement, payload, cancellationToken);
            }
        }

        private async UniTask PlayModules(TextElement textElement, NovelModulePayload payload, CancellationToken cancellationToken)
        {
            foreach (var module in _modules)
            {
                if (module.IsExecutable(textElement))
                {
                    await module.Execute(textElement, payload, cancellationToken);
                }
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
    }
}
