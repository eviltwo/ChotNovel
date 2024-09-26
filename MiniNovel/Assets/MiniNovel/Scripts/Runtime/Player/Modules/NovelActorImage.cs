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
        private NovelActorImageSettings _settings = null;

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

                if (textElement.TryGetFloatParameter("x", out var x))
                {
                    var position = _actorImageManager.GetActorPosition(actorName);
                    position.x = x;
                    _actorImageManager.SetActorPosition(actorName, position);
                }

                if (textElement.TryGetFloatParameter("y", out var y))
                {
                    var position = _actorImageManager.GetActorPosition(actorName);
                    position.y = y;
                    _actorImageManager.SetActorPosition(actorName, position);
                }

                if (textElement.TryGetFloatParameter("xoffset", out var xOffset))
                {
                    var position = _actorImageManager.GetActorOffset(actorName);
                    position.x += xOffset;
                    _actorImageManager.SetActorOffset(actorName, position);
                }

                if (textElement.TryGetFloatParameter("yoffset", out var yOffset))
                {
                    var position = _actorImageManager.GetActorOffset(actorName);
                    position.y += yOffset;
                    _actorImageManager.SetActorOffset(actorName, position);
                }

                if (textElement.TryGetStringParameter("layout", out var layoutName))
                {
                    if (_settings.TryGetLayoutPosition(layoutName, out var position))
                    {
                        _actorImageManager.SetActorPosition(actorName, position);
                    }
                    else
                    {
                        Debug.LogError($"Layout {layoutName} is not found.");
                    }
                }

                if (textElement.TryGetStringParameter("clear", out _))
                {
                    _actorImageManager.RemoveActor(actorName);
                    ReleaseCreatedAsset(actorName);
                    _createdAssets.Remove(actorName);
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
