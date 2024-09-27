using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MiniNovel.Player
{
    public class ChoiceButtonManager : MonoBehaviour
    {
        [SerializeField]
        private Button _sourceButton = null;

        [SerializeField]
        private bool _clearButtonsOnChoice = true;

        private List<Button> _buttons = new List<Button>();

        public void AddButton(string text, System.Action onClick)
        {
            var button = Instantiate(_sourceButton, _sourceButton.transform.parent);
            button.gameObject.SetActive(true);
            var buttonText = button.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = text;
            }
            button.onClick.AddListener(() => onClick());
            if (_clearButtonsOnChoice)
            {
                button.onClick.AddListener(() => Clear());
            }
            _buttons.Add(button);
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
