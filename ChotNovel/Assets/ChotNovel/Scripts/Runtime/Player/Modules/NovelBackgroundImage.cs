using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ChotNovel.Player
{
    public class NovelBackgroundImage : NovelModule
    {
        [SerializeField]
        private string _commandName = "background";

        [SerializeField]
        private Image _image = null;

        private string _folderName;
        private Texture2D _createdTexture;
        private Sprite _createdSprite;

        private void OnDestroy()
        {
            ReleaseCreatedImages();
        }

        private void ReleaseCreatedImages()
        {
            if (_image != null)
            {
                _image.sprite = null;
                _image.enabled = false;
            }
            if (_createdTexture != null)
            {
                Destroy(_createdTexture);
                _createdTexture = null;
            }
            if (_createdSprite != null)
            {
                Destroy(_createdSprite);
                _createdSprite = null;
            }
        }

        public override void ClearDisplayedObjects()
        {
            base.ClearDisplayedObjects();
            ReleaseCreatedImages();
        }

        public override bool IsExecutable(TextElement textElement)
        {
            return textElement.ElementType == TextElementType.Command && textElement.Content == _commandName;
        }

        public override async UniTask Execute(TextElement textElement, NovelModulePayload payload, CancellationToken cancellationToken)
        {
            if (textElement.TryGetStringParameter("folder", out var resourceDirectory))
            {
                _folderName = resourceDirectory;
            }

            if (textElement.TryGetStringParameter("texture", out var textureName))
            {
                var texturePath = PathUtility.CombineWithoutEmpty(_folderName, textureName);
                var texture = await NovelModuleUtility.FindTexture(texturePath);
                if (texture != null)
                {
                    ReleaseCreatedImages();
                    _createdTexture = texture;
                    _createdSprite = Sprite.Create(_createdTexture, new Rect(0, 0, _createdTexture.width, _createdTexture.height), Vector2.zero);
                    _image.enabled = true;
                    _image.sprite = _createdSprite;
                }
            }
        }
    }
}
