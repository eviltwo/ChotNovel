using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ChotNovel.Player
{
    public class NovelChoice : NovelModule
    {
        [SerializeField]
        private string _commandName = "choice";

        [SerializeField]
        private ChoiceButtonManager _buttonManager = null;

        protected override void Reset()
        {
            base.Reset();
            _buttonManager = FindObjectOfType<ChoiceButtonManager>();
        }

        public override void ClearDisplayedObjects()
        {
            base.ClearDisplayedObjects();
            _buttonManager.Clear();
        }

        public override bool IsExecutable(TextElement textElement)
        {
            return textElement.ElementType == TextElementType.Command && textElement.Content == _commandName;
        }

        public override UniTask Execute(TextElement textElement, NovelModulePayload payload, CancellationToken cancellationToken)
        {
            if (textElement.TryGetStringParameter("label", out var labelName))
            {
                var text = textElement.TryGetStringParameter("text", out var resultText) ? resultText : string.Empty;
                System.Action callback;
                if (textElement.TryGetStringParameter("file", out var fileName))
                {
                    callback = () => payload.Player.Play(fileName, labelName);
                }
                else
                {
                    callback = () => payload.Player.Play(labelName);
                }
                _buttonManager.AddButton(text, callback);
            }
            return UniTask.CompletedTask;
        }
    }
}
