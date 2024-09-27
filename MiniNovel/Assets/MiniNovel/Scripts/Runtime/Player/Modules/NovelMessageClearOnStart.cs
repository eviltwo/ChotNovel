using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MiniNovel.Player
{
    public class NovelMessageClearOnStart : NovelModule
    {
        [SerializeField]
        private TMProMessageController _messageController = null;

        protected override void Reset()
        {
            base.Reset();
            _messageController = FindObjectOfType<TMProMessageController>();
        }

        public override bool IsExecutable(TextElement textElement)
        {
            return textElement.ElementType == TextElementType.Label;
        }

        public override UniTask Execute(TextElement textElement, NovelModulePayload payload, CancellationToken cancellationToken)
        {
            _messageController.ClearMessage();
            return UniTask.CompletedTask;
        }
    }
}
