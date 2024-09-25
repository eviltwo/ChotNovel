using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MiniNovel.Player
{
    public class NovelBackgroundImage : NovelModule
    {
        [SerializeField]
        private string _commandName = "back";

        [SerializeField]
        private NovelBackgroundImageSettings _settings = null;

        [SerializeField]
        private Image _image = null;

        public override bool IsExecutable(TextElement textElement)
        {
            return textElement.ElementType == TextElementType.Command && textElement.Content == _commandName;
        }

        public override UniTask Execute(TextElement textElement, NovelModulePayload payload, CancellationToken cancellationToken)
        {
            if (textElement.TryGetStringParameter("name", out var name))
            {
                if (_settings.TryGetSprite(name, out var sprite))
                {
                    _image.enabled = true;
                    _image.sprite = sprite;
                }
            }

            return UniTask.CompletedTask;
        }
    }
}
