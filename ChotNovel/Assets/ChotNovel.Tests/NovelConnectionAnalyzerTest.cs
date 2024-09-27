using System.Collections.Generic;
using ChotNovel.Player;
using NUnit.Framework;

namespace ChotNovel.Tests
{
    public class NovelConnectionAnalyzerTest
    {
        private NovelConnectionAnalyzer _analyzer;
        private List<TextElement> _file1Texts;
        private List<TextElement> _file2Texts;
        private List<TextElement> _file3Texts;

        [SetUp]
        public void Setup()
        {
            // file1 jump to file1 and file2
            _file1Texts = new List<TextElement>();
            {
                _file1Texts.Add(new TextElement("Start", TextElementType.Label));
                var jump1 = new TextElement("jump", TextElementType.Command);
                jump1.AddParameter("label", "Next");
                _file1Texts.Add(jump1);
                _file1Texts.Add(new TextElement("Next", TextElementType.Label));
                var jump2 = new TextElement("jump", TextElementType.Command);
                jump2.AddParameter("file", "file2");
                jump2.AddParameter("label", "Start");
                _file1Texts.Add(jump2);

            }
            // file2 choice to file2 or file3
            _file2Texts = new List<TextElement>();
            {
                _file2Texts.Add(new TextElement("Start", TextElementType.Label));
                var choice1 = new TextElement("choice", TextElementType.Command);
                choice1.AddParameter("label", "Next");
                _file2Texts.Add(choice1);
                var choice2 = new TextElement("choice", TextElementType.Command);
                choice2.AddParameter("file", "file3");
                choice2.AddParameter("label", "Start");
                _file2Texts.Add(choice2);
                _file2Texts.Add(new TextElement("Next", TextElementType.Label));
            }
            // file3 no jump
            _file3Texts = new List<TextElement>();
            {
                _file3Texts.Add(new TextElement("Start", TextElementType.Label));
            }
            // Push texts
            _analyzer = new NovelConnectionAnalyzer();
            _analyzer.AddTargetCommands("jump");
            _analyzer.AddTargetCommands("choice");
            _analyzer.PushFileTexts("file1", _file1Texts);
            _analyzer.PushFileTexts("file2", _file2Texts);
            _analyzer.PushFileTexts("file3", _file3Texts);
        }

        [Test]
        public void GetPreviousEmpty()
        {
            var results = _analyzer.GetPreviousLabels("file1", "Start");
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void GetNextJumpLocalFile()
        {
            var results = _analyzer.GetNextLabels("file1", "Start");
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("file1", results[0].File);
            Assert.AreEqual("Next", results[0].Label);
        }

        [Test]
        public void GetPreviousJumpLocalFile()
        {
            var results = _analyzer.GetPreviousLabels("file1", "Next");
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("file1", results[0].File);
            Assert.AreEqual("Start", results[0].Label);
        }

        [Test]
        public void GetNextJumpOtherFile()
        {
            var results = _analyzer.GetNextLabels("file1", "Next");
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("file2", results[0].File);
            Assert.AreEqual("Start", results[0].Label);
        }

        [Test]
        public void GetPreviousJumpOtherFile()
        {
            var results = _analyzer.GetPreviousLabels("file2", "Start");
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("file1", results[0].File);
            Assert.AreEqual("Next", results[0].Label);
        }

        [Test]
        public void GetNextChoice()
        {
            var results = _analyzer.GetNextLabels("file2", "Start");
            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("file2", results[0].File);
            Assert.AreEqual("Next", results[0].Label);
            Assert.AreEqual("file3", results[1].File);
            Assert.AreEqual("Start", results[1].Label);
        }

        [Test]
        public void GetPreviousChoiceLocalFile()
        {
            var results = _analyzer.GetPreviousLabels("file2", "Next");
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("file2", results[0].File);
            Assert.AreEqual("Start", results[0].Label);
        }

        [Test]
        public void GetPreviousChoiceOtherFile()
        {
            var results = _analyzer.GetPreviousLabels("file3", "Start");
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("file2", results[0].File);
            Assert.AreEqual("Start", results[0].Label);
        }

        [Test]
        public void GetNextEmpty()
        {
            var results = _analyzer.GetNextLabels("file3", "Start");
            Assert.AreEqual(0, results.Count);
        }
    }
}
