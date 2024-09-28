using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ChotNovel.Player
{
    public class NovelJump : NovelModule
    {
        [SerializeField]
        private string _commandName = "jump";

        public override bool IsExecutable(TextElement textElement)
        {
            return textElement.ElementType == TextElementType.Command && textElement.Content == _commandName;
        }

        public override UniTask Execute(TextElement textElement, NovelModulePayload payload, CancellationToken cancellationToken)
        {
            if (payload.IgnoreJump)
            {
                return UniTask.CompletedTask;
            }

            if (textElement.TryGetStringParameter("label", out var labelName))
            {
                if (textElement.TryGetStringParameter("file", out var fileName))
                {
                    payload.Player.Jump(fileName, labelName);
                }
                else
                {
                    payload.Player.Jump(labelName);
                }
            }
            return UniTask.CompletedTask;
        }
    }
}
