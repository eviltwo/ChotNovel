using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ChotNovel.Player
{
    public class NovelClearDisplayedObjects : NovelModule
    {
        [SerializeField]
        private string _commandName = "clear";

        public override bool IsExecutable(TextElement textElement)
        {
            return textElement.ElementType == TextElementType.Command && textElement.Content == _commandName;
        }

        public override UniTask Execute(TextElement textElement, NovelModulePayload payload, CancellationToken cancellationToken)
        {
            payload.Player.ClearDisplayedObjects();
            return UniTask.CompletedTask;
        }
    }
}
