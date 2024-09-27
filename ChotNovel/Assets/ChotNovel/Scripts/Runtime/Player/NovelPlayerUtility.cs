using System.Collections.Generic;

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
    }
}
