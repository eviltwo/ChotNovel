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

        private event System.Action _clickeEvent;

        protected override void Reset()
        {
            base.Reset();
            _messageController = FindAnyObjectByType<TMProMessageController>();
        }

        public override bool IsExecutable(TextElement textElement)
        {
            return textElement.ElementType == TextElementType.Message;
        }

        public override async UniTask Execute(TextElement textElement, NovelModulePayload payload, CancellationToken cancellationToken)
        {
            var message = textElement.Content;
            _messageController.PushMessage(message);
            var interval = _settings.MessageInterval * MessageIntervalMultiplier;
            if (interval == 0 || payload.SkipToEndOfPage)
            {
                _messageController.ShowAllCharacter();
            }
            else
            {
                System.Action skipCallback = () => payload.SkipToEndOfPage = true;
                _clickeEvent += skipCallback;
                try
                {
                    _messageController.ShowNextCharacter();
                    while (_messageController.GetVisibleCharCount() < _messageController.GetTotalCharCount())
                    {
                        await UniTask.WaitForSeconds(interval, cancellationToken: cancellationToken);
                        _messageController.ShowNextCharacter();
                        if (payload.SkipToEndOfPage)
                        {
                            _messageController.ShowAllCharacter();
                            break;
                        }
                    }
                }
                finally
                {
                    _clickeEvent -= skipCallback;
                }
            }
        }

        public void OnClick()
        {
            _clickeEvent?.Invoke();
        }
    }
}
