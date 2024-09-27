using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ChotNovel.Player
{
    public interface ITextContainer
    {
        UniTask<bool> LoadTextElements(string file, List<TextElement> results, CancellationToken cancellationToken);
    }
}
