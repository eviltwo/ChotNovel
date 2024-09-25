using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MiniNovel.Player
{
    public class NovelMessagePlayer : NovelModule
    {
        [SerializeField]
        private NovelMessagePlayerSettings _settings = null;

        [SerializeField]
        public float MessageIntervalMultiplier = 1.0f;

        [SerializeField]
        private TMProMessageController _messageController = null;

        protected override void Reset()
        {
            base.Reset();
            _messageController = FindAnyObjectByType<TMProMessageController>();
        }

        public override bool IsExecutable(TextElement textElement)
        {
            return textElement.ElementType == TextElementType.Message;
        }

        public override async UniTask Execute(TextElement textElement, NovelPlayer novelPlayer, CancellationToken cancellationToken)
        {
            var message = textElement.Content;
            _messageController.PushMessage(message);
            var interval = _settings.MessageInterval * MessageIntervalMultiplier;
            if (interval == 0)
            {
                _messageController.ShowAllCharacter();
            }
            else
            {
                _messageController.ShowNextCharacter();
                while (_messageController.GetVisibleCharCount() < _messageController.GetTotalCharCount())
                {
                    await UniTask.WaitForSeconds(interval, cancellationToken: cancellationToken);
                    _messageController.ShowNextCharacter();
                }
            }
        }
    }
}
