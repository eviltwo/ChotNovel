using System.Collections.Generic;

namespace ChotNovel
{
    public enum TextElementType
    {
        Message,
        Label,
        Command,
    }

    public class TextElement
    {
        public readonly string Content;
        public readonly TextElementType ElementType;
        private readonly Dictionary<string, string> _params = new Dictionary<string, string>();

        public TextElement(string content, TextElementType elementType)
        {
            Content = content;
            ElementType = elementType;
        }

        public void AddParameter(string key, string value)
        {
            _params[key] = value;
        }

        public bool TryGetStringParameter(string key, out string value)
        {
            return _params.TryGetValue(key, out value);
        }

        public bool TryGetIntParameter(string key, out int value)
        {
            if (_params.TryGetValue(key, out var stringValue))
            {
                return int.TryParse(stringValue, out value);
            }
            value = 0;
            return false;
        }

        public bool TryGetFloatParameter(string key, out float value)
        {
            if (_params.TryGetValue(key, out var stringValue))
            {
                return float.TryParse(stringValue, out value);
            }
            value = 0;
            return false;
        }
    }
}
