using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MiniNovel.Player
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
            if (textElement.TryGetStringParameter("label", out var labelName))
            {
                if (textElement.TryGetStringParameter("file", out var fileName))
                {
                    payload.Player.Play(fileName, labelName);
                }
                else
                {
                    payload.Player.Play(labelName);
                }
            }
            return UniTask.CompletedTask;
        }
    }
}
