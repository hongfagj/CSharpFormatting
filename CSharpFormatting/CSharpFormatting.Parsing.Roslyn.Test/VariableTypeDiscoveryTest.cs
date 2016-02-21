﻿using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSharpFormatting.Parsing.Roslyn.Test.Helper;
using CSharpFormatting.Common;

namespace CSharpFormatting.Parsing.Roslyn.Test
{
    [TestClass]
    public class VariableTypeDiscoveryTest
    {
        [TestMethod]
        public void VariableType()
        {
            var expression = "var a = 1 + 3;";
            var result = new CSharpParser().Parse(expression);
            
            var aChunk = result.TextChunks.First(ch => ch.TextValue == "a");

            ExpressionHelper.Check(expression, result);
            Assert.AreEqual(CodeType.Variable, aChunk.CodeType);
            Assert.AreEqual("struct System.Int32", aChunk.TooltipValue);
        }

        [TestMethod]
        public void VarType()
        {
            var expression = "var a = 1 + 3;";
            var result = new CSharpParser().Parse(expression);

            var varChunk = result.TextChunks.First(ch => ch.TextValue == "var");

            ExpressionHelper.Check(expression, result);
            Assert.AreEqual(CodeType.Keyword, varChunk.CodeType);
            Assert.AreEqual("struct System.Int32", varChunk.TooltipValue);
        }

        [TestMethod]
        public void VariableMentionType()
        {
            var expression = "var a = 1 + 3; var b = a;";
            var result = new CSharpParser().Parse(expression);

            var aMention = result.TextChunks.Where(ch => ch.TextValue == "a").Skip(1).First();

            ExpressionHelper.Check(expression, result);
            Assert.AreEqual(CodeType.Variable, aMention.CodeType);
            Assert.AreEqual("struct System.Int32", aMention.TooltipValue);
        }
    }
}
