using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace MiniNovel.Player
{
    public static class NovelModuleUtility
    {
        public static async UniTask<Texture2D> FindTexture(string folderPath, string fileName)
        {
            if (!Directory.Exists(folderPath))
            {
                return null;
            }
            var hasExtension = Path.HasExtension(fileName);
            var searchFilter = hasExtension ? new Regex(fileName) : new Regex(fileName + ".*");
            var file = Directory.GetFiles(folderPath).Where(fileName => searchFilter.IsMatch(fileName)).FirstOrDefault();
            var request = await UnityWebRequestTexture.GetTexture(file).SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"{request.result} {request.error}");
                return null;
            }

            return DownloadHandlerTexture.GetContent(request);
        }
    }
}
