using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiniNovel.Player
{
    public class ActorImageManager : MonoBehaviour
    {
        [SerializeField]
        private Image _sourceImage = null;

        [SerializeField]
        private Transform _actorParent = null;

        private List<ActorController> _actors = new List<ActorController>();

        private void Awake()
        {
            _sourceImage.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            foreach (var actor in _actors)
            {
                actor.DestroyObject();
            }
            _actors.Clear();
        }

        public void CreateActor(string name)
        {
            if (!TryGetActor(name, out _))
            {
                var actorImage = Instantiate(_sourceImage, _actorParent);
                actorImage.gameObject.name = name;
                var actor = new ActorController(name, actorImage);
                _actors.Add(actor);
            }
        }

        public void RemoveActor(string name)
        {
            if (TryGetActor(name, out var actor))
            {
                _actors.Remove(actor);
                actor.DestroyObject();
            }
        }

        public void SetActorSprite(string name, Sprite sprite)
        {
            if (TryGetActor(name, out var actor))
            {
                actor.SetSprite(sprite);
            }
        }

        public Vector2 GetActorOffset(string name)
        {
            if (TryGetActor(name, out var actor))
            {
                return actor.Offset;
            }

            return Vector2.zero;
        }

        public void SetActorOffset(string name, Vector2 offset)
        {
            if (TryGetActor(name, out var actor))
            {
                actor.SetOffset(offset);
            }
        }

        public Vector2 GetActorPosition(string name)
        {
            if (TryGetActor(name, out var actor))
            {
                return actor.Position;
            }

            return Vector2.zero;
        }

        public void SetActorPosition(string name, Vector2 position)
        {
            if (TryGetActor(name, out var actor))
            {
                actor.SetPosition(position);
            }
        }

        private bool TryGetActor(string name, out ActorController result)
        {
            foreach (var actor in _actors)
            {
                if (actor.Name == name)
                {
                    result = actor;
                    return true;
                }
            }

            result = null;
            return false;
        }

        private class ActorController
        {
            public readonly string Name;
            private readonly Image _image;
            private readonly GameObject _gameObject;
            private readonly RectTransform _rectTransform;
            private Vector2 _offset;
            public Vector2 Offset => _offset;
            private Vector2 _position;
            public Vector2 Position => _position;

            public ActorController(string name, Image image)
            {
                Name = name;
                _image = image;
                _gameObject = image.gameObject;
                _rectTransform = image.GetComponent<RectTransform>();
                _gameObject.SetActive(false);
            }

            public void DestroyObject()
            {
                if (_image != null && _image.gameObject != null)
                {
                    Destroy(_image.gameObject);
                }
            }

            public void SetSprite(Sprite sprite)
            {
                _image.sprite = sprite;
                _rectTransform.sizeDelta = sprite != null ? new Vector2(sprite.rect.width, sprite.rect.height) : Vector2.zero;
                _gameObject.SetActive(sprite != null);
            }

            public void SetOffset(Vector2 offset)
            {
                _offset = offset;
                UpdateTransform();
            }

            public void SetPosition(Vector2 position)
            {
                _position = position;
                UpdateTransform();
            }

            private void UpdateTransform()
            {
                _rectTransform.anchoredPosition = _position + _offset;
            }
        }
    }
}
