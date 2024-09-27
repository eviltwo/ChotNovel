using UnityEngine;

namespace MiniNovel.Player
{
    [CreateAssetMenu(fileName = nameof(NovelActorImageSettings), menuName = "MiniNovel/" + nameof(NovelActorImageSettings))]
    public class NovelActorImageSettings : ScriptableObject
    {
        [System.Serializable]
        public class Layout
        {
            [SerializeField]
            private string _name = string.Empty;
            public string Name => _name;

            [SerializeField]
            private Vector2 _position = Vector2.zero;
            public Vector2 Position => _position;
        }

        [SerializeField]
        private Layout[] _layouts = default;

        public bool TryGetLayoutPosition(string layoutName, out Vector2 position)
        {
            foreach (var layout in _layouts)
            {
                if (layout.Name == layoutName)
                {
                    position = layout.Position;
                    return true;
                }
            }

            position = Vector2.zero;
            return false;
        }
    }
}
