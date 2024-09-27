using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ChotNovel.Tests
{
    public class TextParserTest
    {
        private string _source = @"
*Label1|Label1Name
Hello
[Command1 strParam=aaa quoteParam=""aa bb"" intParam=123 floatParam=1.23 nameOnlyParam]
@Command2 strParam=aaa quoteParam=""aa bb"" intParam=123 floatParam=1.23 nameOnlyParam

*Label2
World[Command3]OK[Command4]
Line Break Text
Last Text";

        [Test]
        public void Parse()
        {
            var result = new List<TextElement>();
            TextParser.Parse(_source, result);
            Assert.AreEqual(10, result.Count);
        }

        [Test]
        public void ParseLabel()
        {
            var result = new List<TextElement>();
            TextParser.Parse(_source, result);
            var labels = result.Where(v => v.ElementType == TextElementType.Label).ToList();
            Assert.AreEqual(2, labels.Count);
            Assert.AreEqual("Label1", labels[0].Content);
            Assert.AreEqual(true, labels[0].TryGetStringParameter("name", out var name));
            Assert.AreEqual("Label1Name", name);
            Assert.AreEqual("Label2", labels[1].Content);
        }

        [Test]
        public void ParseMessage()
        {
            var result = new List<TextElement>();
            TextParser.Parse(_source, result);
            var texts = result.Where(v => v.ElementType == TextElementType.Message).ToList();
            Assert.AreEqual(4, texts.Count);
            Assert.AreEqual("Hello", texts[0].Content);
            Assert.AreEqual("World", texts[1].Content);
            Assert.AreEqual("OK", texts[2].Content);
            Assert.AreEqual("Line Break TextLast Text", texts[3].Content);
        }

        [Test]
        public void ParseCommand()
        {
            var result = new List<TextElement>();
            TextParser.Parse(_source, result);
            var commands = result.Where(v => v.ElementType == TextElementType.Command).ToList();
            Assert.AreEqual(4, commands.Count);
            {
                Assert.AreEqual("Command1", commands[0].Content);
                Assert.AreEqual(true, commands[0].TryGetStringParameter("strParam", out var strValue), "strParam exists");
                Assert.AreEqual("aaa", strValue);
                Assert.AreEqual(true, commands[0].TryGetStringParameter("quoteParam", out var quoteValue), "quateParam exists");
                Assert.AreEqual("aa bb", quoteValue);
                Assert.AreEqual(true, commands[0].TryGetIntParameter("intParam", out var intValue), "intParam exists");
                Assert.AreEqual(123, intValue);
                Assert.AreEqual(true, commands[0].TryGetFloatParameter("floatParam", out var floatValue), "floatParam exists");
                Assert.AreEqual(1.23f, floatValue);
                Assert.AreEqual(true, commands[0].TryGetStringParameter("nameOnlyParam", out var _), "nameOnlyParam exists");
            }
            {
                Assert.AreEqual("Command2", commands[1].Content);
                Assert.AreEqual(true, commands[1].TryGetStringParameter("strParam", out var strValue), "strParam exists");
                Assert.AreEqual("aaa", strValue);
                Assert.AreEqual(true, commands[1].TryGetStringParameter("quoteParam", out var quoteValue), "quateParam exists");
                Assert.AreEqual("aa bb", quoteValue);
                Assert.AreEqual(true, commands[1].TryGetIntParameter("intParam", out var intValue), "intParam exists");
                Assert.AreEqual(123, intValue);
                Assert.AreEqual(true, commands[1].TryGetFloatParameter("floatParam", out var floatValue), "floatParam exists");
                Assert.AreEqual(1.23f, floatValue);
                Assert.AreEqual(true, commands[1].TryGetStringParameter("nameOnlyParam", out var _), "nameOnlyParam exists");
            }
            Assert.AreEqual("Command3", commands[2].Content);
            Assert.AreEqual("Command4", commands[3].Content);
        }
    }
}
