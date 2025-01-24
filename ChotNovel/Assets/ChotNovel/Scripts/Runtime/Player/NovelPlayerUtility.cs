using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ChotNovel.Player
{
    public static class NovelPlayerUtility
    {
        public static bool PickLabeledTextElements(IReadOnlyList<TextElement> source, string label, List<TextElement> results)
        {
            results.Clear();
            var startIndex = source.FindIndex(x => x.ElementType == TextElementType.Label && x.Content == label);
            if (startIndex < 0)
            {
                return false;
            }
            var endIndex = source.FindIndex(startIndex + 1, x => x.ElementType == TextElementType.Label);
            if (endIndex == -1)
            {
                results.AddRange(source.GetRange(startIndex, source.Count - startIndex));
            }
            else
            {
                results.AddRange(source.GetRange(startIndex, endIndex - startIndex));
            }
            return true;
        }

        private static int FindIndex<T>(this IReadOnlyList<T> source, System.Func<T, bool> predicate)
        {
            return FindIndex(source, 0, predicate);
        }

        private static int FindIndex<T>(this IReadOnlyList<T> source, int startIndex, System.Func<T, bool> predicate)
        {
            for (var i = startIndex; i < source.Count; i++)
            {
                if (predicate(source[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        private static List<T> GetRange<T>(this IReadOnlyList<T> source, int startIndex, int count)
        {
            var list = new List<T>();
            for (var i = 0; i < count; i++)
            {
                list.Add(source[startIndex + i]);
            }
            return list;
        }

        public static bool TryGetNovelFilePath(string localFilePath, out string result)
        {
            if (TryGetActualNovelFilePath(PathUtility.CombineWithoutEmpty(Application.persistentDataPath, localFilePath), out result))
            {
                return true;
            }

            if (TryGetActualNovelFilePath(PathUtility.CombineWithoutEmpty(Application.streamingAssetsPath, localFilePath), out result))
            {
                return true;
            }

            result = string.Empty;
            return false;
        }

        private static bool TryGetActualNovelFilePath(string filePath, out string result)
        {
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                result = string.Empty;
                return false;
            }

            var fileName = Path.GetFileName(filePath);
            var hasExtension = Path.HasExtension(fileName);
            var searchFilter = hasExtension ? new Regex(fileName) : new Regex(fileName + ".*");
            var file = Directory.GetFiles(directoryPath).Where(fileName => searchFilter.IsMatch(fileName)).FirstOrDefault();
            if (!File.Exists(file))
            {
                result = string.Empty;
                return false;
            }

            result = file;
            return true;
        }
    }
}
