using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace ChotNovel.Player
{
    public class NovelTextFileLoader : MonoBehaviour, ITextContainer
    {
        [SerializeField]
        private string _textFolderName = "";

        [SerializeField]
        private string _textEncoding = "utf-8";

        public async UniTask<bool> LoadTextElements(string localFilePath, List<TextElement> results, CancellationToken cancellationToken)
        {
            results.Clear();
            var encoding = Encoding.GetEncoding(_textEncoding);
            var hasFilePath = NovelPlayerUtility.TryGetNovelFilePath(PathUtility.CombineWithoutEmpty(_textFolderName, localFilePath), out var filePath);
            if (!hasFilePath)
            {
                Debug.LogError($"File not found: {localFilePath}");
                return false;
            }

            var request = await UnityWebRequest.Get(filePath).SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to load text: {request.result} {request.error}");
                return false;
            }

            var text = encoding.GetString(request.downloadHandler.data);
            if (string.IsNullOrEmpty(text))
            {
                Debug.LogError($"Failed to encode text: {filePath}");
                return false;
            }

            TextParser.Parse(text, results);
            return true;
        }

        public UniTask<bool> GetAllFileName(List<string> results, CancellationToken cancellationToken)
        {
            // TODO: Since GetFiles() does not work on Android OS, create a file listing the file names.
            results.Clear();
            var persistentFolderPath = Path.Combine(Application.persistentDataPath, _textFolderName);
            if (Directory.Exists(persistentFolderPath))
            {
                var files = Directory.GetFiles(persistentFolderPath)
                    .Select(GetFileNameWithoutExAndMeta)
                    .Distinct();
                results.AddRange(files);
            }

            var streamingAssetsFolderPath = Path.Combine(Application.streamingAssetsPath, _textFolderName);
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
