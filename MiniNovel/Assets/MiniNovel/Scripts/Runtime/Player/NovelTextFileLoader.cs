using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MiniNovel.Player
{
    public class NovelTextFileLoader : MonoBehaviour, ITextContainer
    {
        [SerializeField]
        private string _folderName = "scenario";

        public bool TryGetTextElements(string fileName, List<TextElement> results)
        {
            var persistentFilePath = Path.Combine(Application.persistentDataPath, _folderName, fileName);
            if (File.Exists(persistentFilePath))
            {
                var texts = File.ReadAllText(persistentFilePath);
                TextParser.Parse(texts, results);
                return true;
            }

            var streamingAssetsFilePath = Path.Combine(Application.streamingAssetsPath, _folderName, fileName);
            if (File.Exists(streamingAssetsFilePath))
            {
                var texts = File.ReadAllText(streamingAssetsFilePath);
                TextParser.Parse(texts, results);
                return true;
            }

            return false;
        }
    }
}
