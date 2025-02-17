using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChotNovel.Player
{
    public class ChoiceButtonManager : MonoBehaviour
    {
        [SerializeField]
        private Button _sourceButton = null;

        [SerializeField]
        private Transform _buttonParent = null;

        [SerializeField]
        private bool _clearButtonsOnChoice = true;

        private List<Button> _buttons = new List<Button>();

        private void Reset()
        {
            _buttonParent = transform;
        }

        public void AddButton(Sprite sprite, System.Action onClick)
        {
            AddButton(sprite, null, onClick);
        }

        public void AddButton(string text, System.Action onClick)
        {
            AddButton(null, text, onClick);
        }

        public void AddButton(Sprite sprite, string text, System.Action onClick)
        {
            var button = Instantiate(_sourceButton, _buttonParent);
            button.gameObject.SetActive(true);

            var image = GetImageInChildrenWithoutParent(button.gameObject);
            if (image != null)
            {
                image.sprite = sprite;
                image.gameObject.SetActive(sprite != null);
            }

            var textComponent = button.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = text;
                textComponent.gameObject.SetActive(!string.IsNullOrEmpty(text));
            }

            button.onClick.AddListener(() => onClick());
            if (_clearButtonsOnChoice)
            {
                button.onClick.AddListener(() => Clear());
            }
            _buttons.Add(button);
        }

        private Image GetImageInChildrenWithoutParent(GameObject parent)
        {
            var childCount = parent.transform.childCount;
            for (var i = 0; i < childCount; i++)
            {
                var child = parent.transform.GetChild(i);
                var image = child.GetComponentInChildren<Image>();
                if (image != null)
                {
                    return image;
                }
            }

            return null;
        }

        public void Clear()
        {
            foreach (var button in _buttons)
            {
                Destroy(button.gameObject);
            }
            _buttons.Clear();
        }
    }
}
