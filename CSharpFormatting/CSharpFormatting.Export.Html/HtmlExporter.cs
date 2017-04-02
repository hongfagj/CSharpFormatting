﻿using CSharpFormatting.Common;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System;
using CSharpFormatting.Export.Html.Helpers;
using CSharpFormatting.Common.Chunk;

namespace CSharpFormatting.Export.Html
{
    public sealed class HtmlExporter : IExporter
    {
        public string ExportAnnotationResult(IEnumerable<IChunk> chunks)
        {
            var headerFile = GetEmbeddedResource("CSharpFormatting.Export.Html.Static.header.html");
            var footerFile = GetEmbeddedResource("CSharpFormatting.Export.Html.Static.footer.html");

            var body = ExportAnnotationResultBody(chunks);

            return headerFile + body + footerFile;
        }

        public string ExportAnnotationResultBody(IEnumerable<IChunk> chunks)
        {
            var bodyHeader = "<table class='pre'>";
            
            var rawCode = GetRawCode(chunks);

            var lineCount = GetLineCount(rawCode);
            var lineNumbers = "<tr><td class='lines'><pre class='fssnip' style='text-align:right'>" + GetLineNumberSpans(lineCount) + "</pre></td>";

            var bodyFooter = "</table>";

            var tooltipDivs = GetTooltipDivs(chunks);

            return bodyHeader + lineNumbers + "<td class='snippet'><pre class='fssnip highlighted'><code lang='csharp'>" + rawCode + bodyFooter + tooltipDivs;
        }

        private string GetRawCode(IEnumerable<IChunk> chunks) =>
            string.Join(
                "",
                chunks.Select((ch, i) => HtmlizeChunk(ch, i)));
        
        private string HtmlizeChunk(IChunk chunk, int i)
        {
            if (chunk is AnnotatedCodeChunk)
            {
                return new CodeChunkHtmlizer().HtmlizeChunkText(i, (AnnotatedCodeChunk)chunk);
            }
            else if (chunk is MarkdownChunk)
            {
                return new MarkdownSharp.Markdown().Transform(((MarkdownChunk)chunk).MarkdownSource);
            }

            throw new NotSupportedException();
        }

        private string GetTooltipDivs(IEnumerable<IChunk> chunks) =>
            string.Join(
                Environment.NewLine,
                chunks
                    .Where(ch => ch is AnnotatedCodeChunk)
                    .Select((ch, i) => new CodeChunkHtmlizer().HtmlizeChunkTooltip(i, (AnnotatedCodeChunk)ch)));

        private string GetLineNumberSpans(int count) =>
            string.Join(
                Environment.NewLine,
                Enumerable
                    .Range(1, count)
                    .Select(i => $"<span class='{i}'>{i}: </span>"));

        private int GetLineCount(string rawCode)
        {
            return rawCode.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n').Count() - 1;
        }

        private string GetEmbeddedResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
