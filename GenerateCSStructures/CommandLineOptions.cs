namespace GenerateCSStructures
{
    using CommandLine;
    using CommandLine.Text;

    class CommandLineOptions
    {
        [Option('d', "directory", Required = true, HelpText = "Input directory.")]
        public string InputDirectory { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output directory.")]
        public string OutputDirectory { get; set; }

        [Option('n', "namespace", Required = true, HelpText = "The base namespace for generated files.")]
        public string BaseNamespace { get; set; }

        // omitting long name, default --verbose
        [Option(DefaultValue = true, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}