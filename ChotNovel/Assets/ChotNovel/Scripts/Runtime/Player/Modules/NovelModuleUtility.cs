using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace ChotNovel.Player
{
    public static class NovelModuleUtility
    {
        public static async UniTask<Texture2D> LoadTexture(string localFilePath)
        {
            var hasFilePath = NovelPlayerUtility.TryGetNovelFilePath(localFilePath, out var filePath);
            if (!hasFilePath)
            {
                Debug.LogError($"File not found: {localFilePath}");
                return null;
            }

            var request = await UnityWebRequestTexture.GetTexture(filePath).SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to load texture: {request.result} {request.error}");
                return null;
            }

            return DownloadHandlerTexture.GetContent(request);
        }
    }
}
