using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MiniNovel.Player
{
    public class NovelPageBreaker : NovelModule
    {
        [SerializeField]
        private string[] _commandNames = { "p" };

        [SerializeField]
        private TMProMessageController _messageController = null;

        private bool _clicked;

        protected override void Reset()
        {
            base.Reset();
            _messageController = FindObjectOfType<TMProMessageController>();
        }

        public override bool IsExecutable(TextElement textElement)
        {
            return textElement.ElementType == TextElementType.Command && Array.IndexOf(_commandNames, textElement.Content) >= 0;
        }

        public override async UniTask Execute(TextElement textElement, NovelModulePayload payload, CancellationToken cancellationToken)
        {
            _clicked = false;
            await UniTask.WaitUntil(() => _clicked, cancellationToken: cancellationToken);
            _messageController.ClearMessage();
            payload.SkipToStopper = false;
        }

        public void OnClick()
        {
            _clicked = true;
        }
    }
}
