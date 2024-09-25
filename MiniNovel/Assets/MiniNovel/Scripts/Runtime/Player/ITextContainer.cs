using System.Collections.Generic;

namespace MiniNovel.Player
{
    public interface ITextContainer
    {
        bool TryGetTextElements(string file, List<TextElement> results);
    }
}
