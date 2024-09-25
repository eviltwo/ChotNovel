using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MiniNovel.Player
{
    public class NovelLineBreaker : NovelModule
    {
        [SerializeField]
        private string[] _commandNames = { "r" };

        [SerializeField]
        private TMProMessageController _messageController = null;

        protected override void Reset()
        {
            base.Reset();
            _messageController = FindObjectOfType<TMProMessageController>();
        }

        public override bool IsExecutable(TextElement textElement)
        {
            return textElement.ElementType == TextElementType.Command && Array.IndexOf(_commandNames, textElement.Content) >= 0;
        }

        public override UniTask Execute(TextElement textElement, NovelModulePayload payload, CancellationToken cancellationToken)
        {
            _messageController.PushMessage(Environment.NewLine);
            return UniTask.CompletedTask;
        }
    }
}
