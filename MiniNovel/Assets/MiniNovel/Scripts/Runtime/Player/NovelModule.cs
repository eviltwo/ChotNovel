using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MiniNovel.Player
{
    public abstract class NovelModule : MonoBehaviour
    {
        [SerializeField]
        private NovelPlayer _player = null;

        protected virtual void Reset()
        {
            _player = GetComponentInParent<NovelPlayer>();
        }

        protected virtual void OnEnable()
        {
            _player.RegisterModule(this);
        }

        protected virtual void OnDisable()
        {
            _player.UnregisterModule(this);
        }

        public abstract bool IsExecutable(TextElement textElement);

        public abstract UniTask Execute(TextElement textElement, NovelPlayer novelPlayer, CancellationToken cancellationToken);
    }
}
