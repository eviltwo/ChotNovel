using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MiniNovel.Player
{
    public class NovelWait : NovelModule
    {
        [SerializeField]
        private string[] _commandNames = { "wait" };

        [SerializeField]
        private string _timeParamName = "time";

        private bool _clicked;

        public override bool IsExecutable(TextElement textElement)
        {
            return textElement.ElementType == TextElementType.Command && Array.IndexOf(_commandNames, textElement.Content) >= 0;
        }

        public override async UniTask Execute(TextElement textElement, NovelModulePayload payload, CancellationToken cancellationToken)
        {
            if (payload.SkipToStopper)
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
                payload.SkipToStopper = true;
            }
        }

        public void OnClick()
        {
            _clicked = true;
        }
    }
}
