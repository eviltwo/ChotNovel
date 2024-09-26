using System.Collections.Generic;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MiniNovel.Player
{
    public class NovelActorImage : NovelModule
    {
        [SerializeField]
        private string _commandName = "actor";

        [SerializeField]
        private string _folderName = "actor";

        [SerializeField]
        private ActorImageManager _actorImageManager = null;

        private class CreatedAssetContainer
        {
            public Texture2D Texture;
            public Sprite Sprite;
        }

        private Dictionary<string, CreatedAssetContainer> _createdAssets = new Dictionary<string, CreatedAssetContainer>();

        private void OnDestroy()
        {
            ReleaseCreatedAssetAll();
        }

        private void ReleaseCreatedAssetAll()
        {
            foreach (var createdAsset in _createdAssets.Keys)
            {
                ReleaseCreatedAsset(createdAsset);
            }
            _createdAssets.Clear();
        }

        private void ReleaseCreatedAsset(string actorName)
        {
            if (_createdAssets.TryGetValue(actorName, out var createdAsset))
            {
                if (createdAsset.Sprite != null)
                {
                    Destroy(createdAsset.Sprite);
                }
                if (createdAsset.Texture != null)
                {
                    Destroy(createdAsset.Texture);
                }
            }
        }

        public override bool IsExecutable(TextElement textElement)
        {
            return textElement.ElementType == TextElementType.Command && textElement.Content == _commandName;
        }

        public override async UniTask Execute(TextElement textElement, NovelModulePayload payload, CancellationToken cancellationToken)
        {
            if (textElement.TryGetStringParameter("name", out var actorName))
            {
                _actorImageManager.CreateActor(actorName);

                if (textElement.TryGetStringParameter("texture", out var textureName))
                {
                    var texture = await FindTexture(textureName);
                    if (texture != null)
                    {
                        ReleaseCreatedAsset(actorName);
                        var createdAsset = new CreatedAssetContainer
                        {
                            Texture = texture,
                            Sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero)
                        };
                        _createdAssets[actorName] = createdAsset;
                        _actorImageManager.SetActorSprite(actorName, createdAsset.Sprite);
                    }
                }
            }
        }

        private async UniTask<Texture2D> FindTexture(string fileName)
        {
            var texture = await NovelModuleUtility.FindTexture(Path.Combine(Application.persistentDataPath, _folderName), fileName);
            if (texture != null)
            {
                return texture;
            }

            return await NovelModuleUtility.FindTexture(Path.Combine(Application.streamingAssetsPath, _folderName), fileName);
        }
    }
}
