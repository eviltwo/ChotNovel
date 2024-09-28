using ChotNovel.Player;
using UnityEngine;

namespace ChotNovel.Addon
{
    public class NovelSaveDataController : MonoBehaviour
    {
        [SerializeField]
        private NovelPlayer _player = null;

        [SerializeField]
        private string _dummyFile = "test";

        [SerializeField]
        private string _dummyLabel = "Start";

        [SerializeField]
        private int _dummyStep = 10;

        private void Reset()
        {
            _player = FindObjectOfType<NovelPlayer>();
        }

        public void Load()
        {
            _player.Play(_dummyFile, _dummyLabel, _dummyStep);
        }
    }
}
