using UnityEngine;

namespace MiniNovel.Player
{
    [CreateAssetMenu(fileName = nameof(NovelBackgroundImageSettings), menuName = "MiniNovel/" + nameof(NovelBackgroundImageSettings))]
    public class NovelBackgroundImageSettings : ScriptableObject
    {
        [SerializeField]
        private NovelBackgroundImageInfo[] _backgroundImages = default;

        public bool TryGetSprite(string name, out Sprite sprite)
        {
            foreach (var backgroundImage in _backgroundImages)
            {
                if (backgroundImage.Name == name)
                {
                    sprite = backgroundImage.Texture;
                    return true;
                }
            }

            sprite = null;
            return false;
        }
    }

    [System.Serializable]
    public class NovelBackgroundImageInfo
    {
        [SerializeField]
        public string Name = string.Empty;

        [SerializeField]
        public Sprite Texture = null;
    }
}
