using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ChotNovel.Player
{
    public class NovelChoice : NovelModule
    {
        [SerializeField]
        private string _commandName = "choice";

        [SerializeField]
        private ChoiceButtonManager _buttonManager = null;

        private List<Texture2D> _createdTextures = new List<Texture2D>();
        private List<Sprite> _createdSprites = new List<Sprite>();

        protected override void Reset()
        {
            base.Reset();
            _buttonManager = FindObjectOfType<ChoiceButtonManager>();
        }

        private void OnDestroy()
        {
            ReleaseCreatedImages();
        }

        private void ReleaseCreatedImages()
        {
            foreach (var texture in _createdTextures)
            {
                Destroy(texture);
            }
            _createdTextures.Clear();

            foreach (var sprite in _createdSprites)
            {
                Destroy(sprite);
            }
            _createdSprites.Clear();
        }

        public override void ClearDisplayedObjects()
        {
            base.ClearDisplayedObjects();
            ReleaseCreatedImages();
            _buttonManager.Clear();
        }

        public override bool IsExecutable(TextElement textElement)
        {
            return textElement.ElementType == TextElementType.Command && textElement.Content == _commandName;
        }

        public override async UniTask Execute(TextElement textElement, NovelModulePayload payload, CancellationToken cancellationToken)
        {
            if (textElement.TryGetStringParameter("label", out var labelName))
            {
                Sprite sprite = null;
                if (textElement.TryGetStringParameter("texture", out var textureName))
                {
                    var texture = await NovelModuleUtility.LoadTexture(textureName);
                    if (texture != null)
                    {
                        _createdTextures.Add(texture);
                        sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                        if (sprite != null)
                        {
                            _createdSprites.Add(sprite);
                        }
                    }
                }

                var text = textElement.TryGetStringParameter("text", out var resultText) ? resultText : string.Empty;
                System.Action callback;
                if (textElement.TryGetStringParameter("file", out var fileName))
                {
                    callback = () => payload.Player.Jump(fileName, labelName);
                }
                else
                {
                    callback = () => payload.Player.Jump(labelName);
                }

                _buttonManager.AddButton(sprite, text, callback);
            }
        }
    }
}
