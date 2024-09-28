using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ChotNovel.Player
{
    public class NovelWait : NovelModule
    {
        [SerializeField]
        private string _commandName = "wait";

        [SerializeField]
        private string _timeParamName = "time";

        private bool _clicked;

        public override bool IsExecutable(TextElement textElement)
        {
            return textElement.ElementType == TextElementType.Command && textElement.Content == _commandName;
        }

        public override async UniTask Execute(TextElement textElement, NovelModulePayload payload, CancellationToken cancellationToken)
        {
            if (payload.SkipToEndOfPage || payload.IgnoreWait)
            {
                return;
            }

            if (!textElement.TryGetFloatParameter(_timeParamName, out var time))
            {
                Debug.LogError($"Failed to get parameter {_timeParamName}.");
            }

            _clicked = false;
            var clickTask = UniTask.WaitUntil(() => _clicked, cancellationToken: cancellationToken);
            var timeTask = UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: cancellationToken);
            await UniTask.WhenAny(clickTask, timeTask);
            if (_clicked)
            {
                payload.SkipToEndOfPage = true;
            }
        }

        public void OnClick()
        {
            _clicked = true;
        }
    }
}
