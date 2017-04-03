﻿using CommandLine;
using CommandLine.Text;

namespace CSharpFormatting.Cli
{
    class Options
    {
        [Option('r', "read", HelpText = "CSharp (.cs), CSharp script (.csx), or Markdown file (.md) to be processed.", Required = true)]
        public string InputFile { get; set; }

        [Option('m', "format", DefaultValue = InputFormat.ByExtension, HelpText = "Input format definition: Markdown / Csharp / CsharpScript / ByExtension.", Required = false)]
        public InputFormat InputFormat { get; set; }

        [Option('f', "references", HelpText = "Base path for DLL reference resolution.", Required = false)]
        public string BaseReferencePath { get; set; }

        [Option('v', "verbose", DefaultValue = false, HelpText = "Talk more about the processing.", Required = false)]
        public bool Verbose { get; set; }

        [Option('w', "write", HelpText = "Path of the output file.", Required = false)]
        public string OutputFile { get; set; }

        [Option('o', "overwrite", DefaultValue = false, HelpText = "Overwrite the file on the output.", Required = false)]
        public bool OverwriteOutputFile { get; set; }

        [Option('a', "failOnCompileWarning", DefaultValue = false, HelpText = "Fail the processing when a C# warning is encountered.", Required = false)]
        public bool FailOnCompileWarning { get; set; }

        [Option('e', "failOnCompileError", DefaultValue = true, HelpText = "Fail the processing when a C# error is encountered.", Required = false)]
        public bool FailOnCompileError { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
