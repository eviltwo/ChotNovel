using UnityEngine;

namespace ChotNovel.Player
{
    [CreateAssetMenu(fileName = nameof(NovelMessagePlayerSettings), menuName = NovelModuleDefine.AssetMenuRoot + "/" + nameof(NovelMessagePlayerSettings))]
    public class NovelMessagePlayerSettings : ScriptableObject
    {
        [SerializeField]
        public float MessageInterval = 0.05f;
    }
}
