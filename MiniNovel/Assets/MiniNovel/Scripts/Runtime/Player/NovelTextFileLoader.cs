using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace MiniNovel.Player
{
    public class NovelTextFileLoader : MonoBehaviour, ITextContainer
    {
        [SerializeField]
        private string _folderName = "scenario";

        [SerializeField]
        private string _textEncoding = "utf-8";

        public bool TryGetTextElements(string fileName, List<TextElement> results)
        {
            var encoding = Encoding.GetEncoding(_textEncoding);
            var persistentFilePath = Path.Combine(Application.persistentDataPath, _folderName, fileName);
            if (File.Exists(persistentFilePath))
            {
                var texts = File.ReadAllText(persistentFilePath, encoding);
                TextParser.Parse(texts, results);
                return true;
            }

            var streamingAssetsFilePath = Path.Combine(Application.streamingAssetsPath, _folderName, fileName);
            if (File.Exists(streamingAssetsFilePath))
            {
                var texts = File.ReadAllText(streamingAssetsFilePath, encoding);
                TextParser.Parse(texts, results);
                return true;
            }

            return false;
        }
    }
}
