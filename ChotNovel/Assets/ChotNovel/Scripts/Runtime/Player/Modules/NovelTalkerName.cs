using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace ChotNovel.Player
{
    public class NovelTalkerName : NovelModule
    {
        [SerializeField]
        private string _commandName = "talker";

        [SerializeField]
        private TMP_Text _nameText = null;

        private void Awake()
        {
            _nameText.text = string.Empty;
        }

        public override void ClearDisplayedObjects()
        {
            base.ClearDisplayedObjects();
            _nameText.text = string.Empty;
        }

        public override bool IsExecutable(TextElement textElement)
        {
            return textElement.ElementType == TextElementType.Command && textElement.Content == _commandName;
        }

        public override UniTask Execute(TextElement textElement, NovelModulePayload payload, CancellationToken cancellationToken)
        {
            if (textElement.TryGetStringParameter("clear", out _))
            {
                _nameText.text = string.Empty;
            }

            if (textElement.TryGetStringParameter("name", out var talkerName))
            {
                _nameText.text = talkerName;
            }

            return UniTask.CompletedTask;
        }
    }
}
