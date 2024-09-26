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

        public override bool IsExecutable(TextElement textElement)
        {
            return textElement.ElementType == TextElementType.Command && textElement.Content == _commandName;
        }

        public override async UniTask Execute(TextElement textElement, NovelModulePayload payload, CancellationToken cancellationToken)
        {
            if (textElement.TryGetStringParameter("name", out var name))
            {
                var texture = await FindTexture(name);
                _image.enabled = texture != null;
                if (texture != null)
                {
                    _image.enabled = true;
                    _image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                }
            }
        }

        private async UniTask<Texture2D> FindTexture(string fileName)
        {
            var hasExtension = Path.HasExtension(fileName);
            var streamingAssetsFilePath = Path.Combine(Application.streamingAssetsPath, _folderName);
            var searchFilter = hasExtension ? new Regex(fileName) : new Regex(fileName + ".*");
            var file = Directory.GetFiles(streamingAssetsFilePath).Where(fileName => searchFilter.IsMatch(fileName)).FirstOrDefault();
            var request = UnityWebRequestTexture.GetTexture(file);
            await request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"{request.result} {request.error}");
                return null;
            }

            return DownloadHandlerTexture.GetContent(request);
        }
    }
}
