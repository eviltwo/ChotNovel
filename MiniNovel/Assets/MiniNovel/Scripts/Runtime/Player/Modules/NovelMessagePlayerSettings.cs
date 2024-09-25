using UnityEngine;

namespace MiniNovel.Player
{
    [CreateAssetMenu(fileName = nameof(NovelMessagePlayerSettings), menuName = "MiniNovel/" + nameof(NovelMessagePlayerSettings))]
    public class NovelMessagePlayerSettings : ScriptableObject
    {
        [SerializeField]
        public float MessageInterval = 0.1f;
    }
}
