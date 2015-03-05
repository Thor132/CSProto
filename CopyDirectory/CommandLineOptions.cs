namespace VisualCopyDirectory
{
    using CommandLine;
    using CommandLine.Text;

    internal class CommandLineOptions
    {
        [Option('s', "source", Required = true, HelpText = "Source directory")]
        public string SourceDirectory { get; set; }

        [Option('d', "destination", Required = true, HelpText = "Destination directory")]
        public string DestinationDirectory { get; set; }

        [Option('t', "text", DefaultValue = "", HelpText = "Custom text")]
        public string CustomText { get; set; }

        [Option('k', "keepOpen", DefaultValue = false, HelpText = "Whether to stay open once copied")]
        public bool KeepOpen { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
