using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MiniNovel.Tests
{
    public class TextParserTest
    {
        private string _source = @"
*Label1|Label1Name
Hello
[Command1 param1=aaa param2=bbb]

*Label2
World[Command2 param1=123 param2=1.23]OK[Command3]
Last Text";

        [Test]
        public void Parse()
        {
            var result = new List<TextElement>();
            TextParser.Parse(_source, result);
            Assert.AreEqual(9, result.Count);
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
            Assert.AreEqual("Last Text", texts[3].Content);
        }

        [Test]
        public void ParseCommand()
        {
            var result = new List<TextElement>();
            TextParser.Parse(_source, result);
            var commands = result.Where(v => v.ElementType == TextElementType.Command).ToList();
            Assert.AreEqual(3, commands.Count);
            Assert.AreEqual("Command1", commands[0].Content);
            Assert.AreEqual("Command2", commands[1].Content);
            Assert.AreEqual("Command3", commands[2].Content);
            Assert.AreEqual(true, commands[0].TryGetStringParameter("param1", out var param1));
            Assert.AreEqual("aaa", param1);
            Assert.AreEqual(true, commands[0].TryGetStringParameter("param2", out var param2));
            Assert.AreEqual("bbb", param2);
            Assert.AreEqual(true, commands[1].TryGetIntParameter("param1", out var param3));
            Assert.AreEqual(123, param3);
            Assert.AreEqual(true, commands[1].TryGetFloatParameter("param2", out var param4));
            Assert.AreEqual(1.23f, param4);
        }
    }
}
