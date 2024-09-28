using ChotNovel.Player;
using UnityEngine;

namespace ChotNovel.Addon
{
    public class NovelSaveDataController : MonoBehaviour
    {
        [SerializeField]
        private NovelPlayer _player = null;

        [SerializeField]
        private string _saveDataKey = "Novel_PlaybackAddress";

        private static readonly string Separator = ":::";

        private void Reset()
        {
            _player = FindObjectOfType<NovelPlayer>();
        }

        public void Save()
        {
            _player.GetCurrentPlaybackAddress(out var file, out var label, out var step);
            var serialized = SerializePlaybackAddress(file, label, step);
            PlayerPrefs.SetString(_saveDataKey, serialized);
        }

        public void Load()
        {
            var serialized = PlayerPrefs.GetString(_saveDataKey);
            if (DeserializePlaybackAddress(serialized, out var file, out var label, out var step))
            {
                _player.Play(file, label, step);
            }
            else
            {
                Debug.LogError($"Failed to load playback address. {serialized}");
            }
        }

        private static string SerializePlaybackAddress(string file, string label, int step)
        {
            return $"{file}{Separator}{label}{Separator}{step}";
        }

        private static bool DeserializePlaybackAddress(string serialized, out string file, out string label, out int step)
        {
            var parts = serialized.Split(Separator);
            if (parts.Length != 3)
            {
                file = string.Empty;
                label = string.Empty;
                step = 0;
                return false;
            }
            file = parts[0];
            label = parts[1];
            step = int.Parse(parts[2]);
            return true;
        }
    }
}
