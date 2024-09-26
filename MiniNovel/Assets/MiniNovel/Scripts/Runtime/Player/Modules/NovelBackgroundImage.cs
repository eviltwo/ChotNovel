using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace MiniNovel.Player
{
    public class NovelBackgroundImage : NovelModule
    {
        [SerializeField]
        private string _commandName = "back";

        [SerializeField]
        private string _folderName = "bgimage";

        [SerializeField]
        private Image _image = null;

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

        public override bool IsExecutable(TextElement textElement)
        {
            return textElement.ElementType == TextElementType.Command && textElement.Content == _commandName;
        }

        public override async UniTask Execute(TextElement textElement, NovelModulePayload payload, CancellationToken cancellationToken)
        {
            if (textElement.TryGetStringParameter("name", out var name))
            {
                ReleaseCreatedImages();
                _createdTexture = await FindTexture(name);
                _image.enabled = _createdTexture != null;
                if (_createdTexture != null)
                {
                    _image.enabled = true;
                    _createdSprite = Sprite.Create(_createdTexture, new Rect(0, 0, _createdTexture.width, _createdTexture.height), Vector2.zero);
                    _image.sprite = _createdSprite;
                }
            }
        }

        private async UniTask<Texture2D> FindTexture(string fileName)
        {
            var texture = await FindTexture(Path.Combine(Application.persistentDataPath, _folderName), fileName);
            if (texture != null)
            {
                return texture;
            }

            return await FindTexture(Path.Combine(Application.streamingAssetsPath, _folderName), fileName);
        }

        private static async UniTask<Texture2D> FindTexture(string folderPath, string fileName)
        {
            if (!Directory.Exists(folderPath))
            {
                return null;
            }
            var hasExtension = Path.HasExtension(fileName);
            var searchFilter = hasExtension ? new Regex(fileName) : new Regex(fileName + ".*");
            var file = Directory.GetFiles(folderPath).Where(fileName => searchFilter.IsMatch(fileName)).FirstOrDefault();
            var request = await UnityWebRequestTexture.GetTexture(file).SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"{request.result} {request.error}");
                return null;
            }

            return DownloadHandlerTexture.GetContent(request);
        }
    }
}
