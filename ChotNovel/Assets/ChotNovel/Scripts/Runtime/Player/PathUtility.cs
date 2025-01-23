using System.IO;

namespace ChotNovel.Player
{
    public static class PathUtility
    {
        public static string CombineWithoutEmpty(string path1, string path2)
        {
            return CombineWithoutEmpty(new string[] { path1, path2 });
        }

        public static string CombineWithoutEmpty(params string[] paths)
        {
            if (paths.Length == 0)
            {
                return string.Empty;
            }

            int startIndex = 0;
            for (var i = 0; i < paths.Length; i++)
            {
                if (!string.IsNullOrEmpty(paths[i]))
                {
                    startIndex = i;
                    break;
                }
            }

            if (startIndex == paths.Length)
            {
                return string.Empty;
            }

            var result = paths[startIndex];
            for (int i = startIndex + 1; i < paths.Length; i++)
            {
                if (!string.IsNullOrEmpty(paths[i]))
                {
                    result = Path.Combine(result, paths[i]);
                }
            }
            return result;
        }
    }
}
