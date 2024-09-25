using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MiniNovel.Tests
{
    public class TextParserTest
    {
        private string _source = @"
*Label1
Hello
[Command1 param1=aaa param2=bbb]

*Label2
World[Command2 param1=ccc param2=ddd]OK[Command3]
";

        [Test]
        public void Parse()
        {
            var result = new List<TextElement>();
            TextParser.Parse(_source, result);
            Assert.AreEqual(8, result.Count);
        }

        [Test]
        public void ParseLabel()
        {
            var result = new List<TextElement>();
            TextParser.Parse(_source, result);
            var labels = result.Where(v => v.ElementType == TextElementType.Label).ToList();
            Assert.AreEqual(2, labels.Count);
            Assert.AreEqual("Label1", labels[0].Content);
            Assert.AreEqual("Label2", labels[1].Content);
        }

        [Test]
        public void ParseMessage()
        {
            var result = new List<TextElement>();
            TextParser.Parse(_source, result);
            var texts = result.Where(v => v.ElementType == TextElementType.Message).ToList();
            Assert.AreEqual(3, texts.Count);
            Assert.AreEqual("Hello", texts[0].Content);
            Assert.AreEqual("World", texts[1].Content);
            Assert.AreEqual("OK", texts[2].Content);
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
            Assert.IsTrue(commands[0].TryGetParameter("param1", out var param1));
            Assert.AreEqual("aaa", param1);
            Assert.IsTrue(commands[0].TryGetParameter("param2", out var param2));
            Assert.AreEqual("bbb", param2);
            Assert.IsTrue(commands[1].TryGetParameter("param1", out var param3));
            Assert.AreEqual("ccc", param3);
            Assert.IsTrue(commands[1].TryGetParameter("param2", out var param4));
            Assert.AreEqual("ddd", param4);
        }
    }
}
