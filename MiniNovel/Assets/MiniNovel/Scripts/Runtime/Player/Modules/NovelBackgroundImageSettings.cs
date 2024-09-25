using UnityEngine;

namespace MiniNovel.Player
{
    [CreateAssetMenu(fileName = nameof(NovelBackgroundImageSettings), menuName = "MiniNovel/" + nameof(NovelBackgroundImageSettings))]
    public class NovelBackgroundImageSettings : ScriptableObject
    {
        [SerializeField]
        private Sprite[] _sprites = default;

        public bool TryGetSprite(string name, out Sprite result)
        {
            foreach (var sprite in _sprites)
            {
                if (sprite.name == name)
                {
                    result = sprite;
                    return true;
                }
            }

            result = null;
            return false;
        }
    }
}
