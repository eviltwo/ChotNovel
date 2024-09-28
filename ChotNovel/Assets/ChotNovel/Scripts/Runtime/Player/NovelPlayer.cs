using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ChotNovel.Player
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

        /// <summary>
        /// Start playback from a specific label within a specific file.
        /// </summary>
        public void Play(string file, string label)
        {
            ClearDisplayedObjects();
            // TODO: Rewind to "clear" command and setup environment.
            //       This is dummy implementation.
            Jump(file, label);
        }

        /// <summary>
        /// Jump to a specific label within the file being played.
        /// </summary>
        public void Jump(string label)
        {
            Jump(_lastPlayedFileName, label);
        }

        /// <summary>
        /// Jump to a specific label within a specific file.
        /// </summary>
        public void Jump(string file, string label)
        {
            if (_playerCancellation != null)
            {
                _playerCancellation.Cancel();
                _playerCancellation.Dispose();
                _playerCancellation = null;
            }

            _playerCancellation = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);
            JumpInternal(file, label, _playerCancellation.Token).Forget();
        }

        private async UniTask JumpInternal(string file, string label, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(file))
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
            var success = await _textContainer.LoadTextElements(file, _textElementBuffer, cancellationToken);
            if (!success)
            {
                Debug.LogError($"Failed to get text elements from {file}.");
                return;
            }
            _textElements.Clear();
            success = NovelPlayerUtility.PickLabeledTextElements(_textElementBuffer, label, _textElements);
            if (!success)
            {
                Debug.LogError($"Label {label} is not found.");
                return;
            }

            _lastPlayedFileName = file;
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
    }
}
