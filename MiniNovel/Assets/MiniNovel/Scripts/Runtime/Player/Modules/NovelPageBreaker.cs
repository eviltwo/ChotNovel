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

        public override bool IsExecutable(TextElement textElement)
        {
            return textElement.ElementType == TextElementType.Command && Array.IndexOf(_commandNames, textElement.Content) >= 0;
        }

        public override async UniTask Execute(TextElement textElement, NovelPlayer novelPlayer, CancellationToken cancellationToken)
        {
            _clicked = false;
            await UniTask.WaitUntil(() => _clicked, cancellationToken: cancellationToken);
            _messageController.ClearMessage();
        }

        public void OnClick()
        {
            _clicked = true;
        }
    }
}
