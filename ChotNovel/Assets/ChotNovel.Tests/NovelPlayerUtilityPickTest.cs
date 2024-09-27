using System.Collections.Generic;
using ChotNovel.Player;
using NUnit.Framework;

namespace ChotNovel.Tests
{
    public class NovelPlayerUtilityPickTest
    {
        private List<TextElement> _textElements;

        [SetUp]
        public void SetUp()
        {
            _textElements = new List<TextElement>();
            _textElements.Add(new TextElement("Label1", TextElementType.Label));
            _textElements.Add(new TextElement("Message1", TextElementType.Message));
            _textElements.Add(new TextElement("Label2", TextElementType.Label));
            _textElements.Add(new TextElement("Message2", TextElementType.Message));
            _textElements.Add(new TextElement("Label3", TextElementType.Label));
            _textElements.Add(new TextElement("Message3", TextElementType.Message));
        }

        [Test]
        public void PickFirst()
        {
            var result = new List<TextElement>();
            var success = NovelPlayerUtility.PickLabeledTextElements(_textElements, "Label1", result);
            Assert.AreEqual(true, success, "success");
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Label1", result[0].Content);
            Assert.AreEqual("Message1", result[1].Content);
        }

        [Test]
        public void PickMiddle()
        {
            var result = new List<TextElement>();
            var success = NovelPlayerUtility.PickLabeledTextElements(_textElements, "Label2", result);
            Assert.AreEqual(true, success, "success");
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Label2", result[0].Content);
            Assert.AreEqual("Message2", result[1].Content);
        }

        [Test]
        public void PickLast()
        {
            var result = new List<TextElement>();
            var success = NovelPlayerUtility.PickLabeledTextElements(_textElements, "Label3", result);
            Assert.AreEqual(true, success, "success");
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Label3", result[0].Content);
            Assert.AreEqual("Message3", result[1].Content);
        }

        [Test]
        public void PickNotFound()
        {
            var result = new List<TextElement>();
            var success = NovelPlayerUtility.PickLabeledTextElements(_textElements, "Label4", result);
            Assert.AreEqual(false, success, "success");
        }
    }
}
