using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace ChotNovel.Player
{
    public class NovelTextFileLoader : MonoBehaviour, ITextContainer
    {
        [SerializeField]
        private string _folderName = "scenario";

        [SerializeField]
        private string _textEncoding = "utf-8";

        public async UniTask<bool> LoadTextElements(string file, List<TextElement> results, CancellationToken cancellationToken)
        {
            results.Clear();
            var encoding = Encoding.GetEncoding(_textEncoding);
            var text = await LoadText(Path.Combine(Application.persistentDataPath, _folderName), file, encoding);
            if (text == null)
            {
                text = await LoadText(Path.Combine(Application.streamingAssetsPath, _folderName), file, encoding);
            }
            if (text == null)
            {
                return false;
            }
            TextParser.Parse(text, results);
            return true;
        }

        private static async UniTask<string> LoadText(string folderPath, string fileName, Encoding encoding)
        {
            if (!Directory.Exists(folderPath))
            {
                return null;
            }
            var hasExtension = Path.HasExtension(fileName);
            var searchFilter = hasExtension ? new Regex(fileName) : new Regex(fileName + ".*");
            var file = Directory.GetFiles(folderPath).Where(fileName => searchFilter.IsMatch(fileName)).FirstOrDefault();
            var request = await UnityWebRequest.Get(file).SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"{request.result} {request.error}");
                return null;
            }

            return encoding.GetString(request.downloadHandler.data);
        }

        public UniTask<bool> GetAllFileName(List<string> results, CancellationToken cancellationToken)
        {
            // TODO: Since GetFiles() does not work on Android OS, create a file listing the file names.
            results.Clear();
            var persistentFolderPath = Path.Combine(Application.persistentDataPath, _folderName);
            if (Directory.Exists(persistentFolderPath))
            {
                var files = Directory.GetFiles(persistentFolderPath)
                    .Select(GetFileNameWithoutExAndMeta)
                    .Distinct();
                results.AddRange(files);
            }

            var streamingAssetsFolderPath = Path.Combine(Application.streamingAssetsPath, _folderName);
            if (Directory.Exists(streamingAssetsFolderPath))
            {
                var files = Directory.GetFiles(streamingAssetsFolderPath)
                    .Select(GetFileNameWithoutExAndMeta)
                    .Distinct()
                    .Where(fileName => !results.Contains(fileName));
                results.AddRange(files);
            }
            results.Sort();
            return UniTask.FromResult(true);
        }

        public static string GetFileNameWithoutExAndMeta(string filePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            if (filePath.EndsWith(".meta"))
            {
                fileName = Path.GetFileNameWithoutExtension(fileName);
            }
            return fileName;
        }
    }
}
