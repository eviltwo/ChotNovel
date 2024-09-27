using UnityEngine;

namespace ChotNovel.Player
{
    public class NovelPlayerInitiator : MonoBehaviour
    {
        [SerializeField]
        private NovelPlayer _player = null;

        [SerializeField]
        private string _fileName = "file.txt";

        [SerializeField]
        private string _label = "Start";

        private void Reset()
        {
            _player = GetComponent<NovelPlayer>();
        }

        private void Start()
        {
            _player.Play(_fileName, _label);
        }
    }
}
