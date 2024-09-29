using System.Collections.Generic;
using NUnit.Framework;

namespace ChotNovel.Tests
{
    public class TextParserTest
    {
        [Test]
        public void IgnoreCommentLine()
        {
            var result = new List<TextElement>();
            TextParser.Parse(";Comment", result);
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void IgnoreTailComment()
        {
            var result = new List<TextElement>();
            TextParser.Parse("Hello;Comment", result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Hello", result[0].Content);
        }

        [Test]
        public void ParseLabelLine()
        {
            var result = new List<TextElement>();
            TextParser.Parse("*Label1", result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(TextElementType.Label, result[0].ElementType);
            Assert.AreEqual("Label1", result[0].Content);
        }

        [TestCase("Hello")]
        [TestCase("Hello World")]
        [TestCase("123")]
        [TestCase(".=+-*/_")]
        public void ParseMessageOneLine(string str)
        {
            var result = new List<TextElement>();
            TextParser.Parse(str, result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(TextElementType.Message, result[0].ElementType);
            Assert.AreEqual(str, result[0].Content);
        }

        [Test]
        public void ParseMessageMultipleLine()
        {
            var result = new List<TextElement>();
            TextParser.Parse("Hello\nWorld", result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(TextElementType.Message, result[0].ElementType);
            Assert.AreEqual("Hello", result[0].Content);
            Assert.AreEqual(TextElementType.Message, result[1].ElementType);
            Assert.AreEqual("World", result[1].Content);
        }

        [Test]
        public void ParseCommandLineNameOnly()
        {
            var result = new List<TextElement>();
            TextParser.Parse("@Command1", result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(TextElementType.Command, result[0].ElementType);
            Assert.AreEqual("Command1", result[0].Content);
        }

        [Test]
        public void ParseCommandLineParamNoValue()
        {
            var result = new List<TextElement>();
            TextParser.Parse("@Command1 param", result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(TextElementType.Command, result[0].ElementType);
            Assert.AreEqual("Command1", result[0].Content);
            Assert.AreEqual(true, result[0].TryGetStringParameter("param", out var _));
        }

        [Test]
        public void ParseCommandLineStringParam()
        {
            var result = new List<TextElement>();
            TextParser.Parse("@Command1 param=aaa", result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(TextElementType.Command, result[0].ElementType);
            Assert.AreEqual("Command1", result[0].Content);
            Assert.AreEqual(true, result[0].TryGetStringParameter("param", out var value));
            Assert.AreEqual("aaa", value);
        }

        [Test]
        public void ParseCommandLineQuoteParam()
        {
            var result = new List<TextElement>();
            TextParser.Parse("@Command1 param=\"aa bb\"", result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(TextElementType.Command, result[0].ElementType);
            Assert.AreEqual("Command1", result[0].Content);
            Assert.AreEqual(true, result[0].TryGetStringParameter("param", out var value));
            Assert.AreEqual("aa bb", value);
        }

        [Test]
        public void ParseCommandLineIntParam()
        {
            var result = new List<TextElement>();
            TextParser.Parse("@Command1 param=123", result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(TextElementType.Command, result[0].ElementType);
            Assert.AreEqual("Command1", result[0].Content);
            Assert.AreEqual(true, result[0].TryGetIntParameter("param", out var value));
            Assert.AreEqual(123, value);
        }

        [Test]
        public void ParseCommandLineFloatParam()
        {
            var result = new List<TextElement>();
            TextParser.Parse("@Command1 param=1.23", result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(TextElementType.Command, result[0].ElementType);
            Assert.AreEqual("Command1", result[0].Content);
            Assert.AreEqual(true, result[0].TryGetFloatParameter("param", out var value));
            Assert.AreEqual(1.23f, value);
        }

        [Test]
        public void ParseCommandBraketNameOnly()
        {
            var result = new List<TextElement>();
            TextParser.Parse("[Command1]", result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(TextElementType.Command, result[0].ElementType);
            Assert.AreEqual("Command1", result[0].Content);
        }

        [Test]
        public void ParseCommandBraketParamNoValue()
        {
            var result = new List<TextElement>();
            TextParser.Parse("[Command1 param]", result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(TextElementType.Command, result[0].ElementType);
            Assert.AreEqual("Command1", result[0].Content);
            Assert.AreEqual(true, result[0].TryGetStringParameter("param", out var _));
        }

        [Test]
        public void ParseCommandBraketStringParam()
        {
            var result = new List<TextElement>();
            TextParser.Parse("[Command1 param=aaa]", result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(TextElementType.Command, result[0].ElementType);
            Assert.AreEqual("Command1", result[0].Content);
            Assert.AreEqual(true, result[0].TryGetStringParameter("param", out var value));
            Assert.AreEqual("aaa", value);
        }

        [Test]
        public void ParseCommandBraketQuoteParam()
        {
            var result = new List<TextElement>();
            TextParser.Parse("[Command1 param=\"aa bb\"]", result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(TextElementType.Command, result[0].ElementType);
            Assert.AreEqual("Command1", result[0].Content);
            Assert.AreEqual(true, result[0].TryGetStringParameter("param", out var value));
            Assert.AreEqual("aa bb", value);
        }

        [Test]
        public void ParseCommandBraketIntParam()
        {
            var result = new List<TextElement>();
            TextParser.Parse("[Command1 param=123]", result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(TextElementType.Command, result[0].ElementType);
            Assert.AreEqual("Command1", result[0].Content);
            Assert.AreEqual(true, result[0].TryGetIntParameter("param", out var value));
            Assert.AreEqual(123, value);
        }

        [Test]
        public void ParseCommandBraketFloatParam()
        {
            var result = new List<TextElement>();
            TextParser.Parse("[Command1 param=1.23]", result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(TextElementType.Command, result[0].ElementType);
            Assert.AreEqual("Command1", result[0].Content);
            Assert.AreEqual(true, result[0].TryGetFloatParameter("param", out var value));
            Assert.AreEqual(1.23f, value);
        }

        [Test]
        public void ParseMessageAndCommand1()
        {
            var result = new List<TextElement>();
            TextParser.Parse("Hello[Command1]", result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(TextElementType.Message, result[0].ElementType);
            Assert.AreEqual("Hello", result[0].Content);
            Assert.AreEqual(TextElementType.Command, result[1].ElementType);
            Assert.AreEqual("Command1", result[1].Content);
        }

        [Test]
        public void ParseMessageAndCommand2()
        {
            var result = new List<TextElement>();
            TextParser.Parse("[Command1]Hello", result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(TextElementType.Command, result[0].ElementType);
            Assert.AreEqual("Command1", result[0].Content);
            Assert.AreEqual(TextElementType.Message, result[1].ElementType);
            Assert.AreEqual("Hello", result[1].Content);
        }

        [Test]
        public void ParseMessageAndCommand3()
        {
            var result = new List<TextElement>();
            TextParser.Parse("Hello[Command1]World", result);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(TextElementType.Message, result[0].ElementType);
            Assert.AreEqual("Hello", result[0].Content);
            Assert.AreEqual(TextElementType.Command, result[1].ElementType);
            Assert.AreEqual("Command1", result[1].Content);
            Assert.AreEqual(TextElementType.Message, result[2].ElementType);
            Assert.AreEqual("World", result[2].Content);
        }

        [Test]
        public void ParseMessageAndCommand4()
        {
            var result = new List<TextElement>();
            TextParser.Parse("[Command1]Hello[Command2]", result);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(TextElementType.Command, result[0].ElementType);
            Assert.AreEqual("Command1", result[0].Content);
            Assert.AreEqual(TextElementType.Message, result[1].ElementType);
            Assert.AreEqual("Hello", result[1].Content);
            Assert.AreEqual(TextElementType.Command, result[2].ElementType);
            Assert.AreEqual("Command2", result[2].Content);
        }

        [Test]
        public void IgnoreBrankLine()
        {
            var result = new List<TextElement>();
            TextParser.Parse("Hello\n\nWorld", result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(TextElementType.Message, result[0].ElementType);
            Assert.AreEqual("Hello", result[0].Content);
            Assert.AreEqual(TextElementType.Message, result[1].ElementType);
            Assert.AreEqual("World", result[1].Content);
        }
    }
}
