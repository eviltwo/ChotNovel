using TMPro;
using UnityEngine;

namespace ChotNovel.Player
{
    public class TMProMessageController : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _text = null;

        private int _visibleCharCount;
        private string _parsedText;

        private void Reset()
        {
            _text = GetComponent<TMP_Text>();
        }

        private void Awake()
        {
            _text.text = string.Empty;
        }

        public void ClearMessage()
        {
            _text.text = string.Empty;
            _visibleCharCount = 0;
            _parsedText = string.Empty;
        }

        public void PushMessage(string message)
        {
            _text.text += message;
            _text.ForceMeshUpdate();
            _parsedText = _text.GetParsedText();
            UpdateVertex();
        }

        public void ShowNextCharacter()
        {
            _visibleCharCount++;
            UpdateVertex();
        }

        public void ShowAllCharacter()
        {
            _visibleCharCount = _parsedText.Length;
            UpdateVertex();
        }

        private void UpdateVertex()
        {
            _text.maxVisibleCharacters = _visibleCharCount;
        }

        public int GetVisibleCharCount() => _visibleCharCount;

        public int GetTotalCharCount() => _parsedText.Length;
    }
}
