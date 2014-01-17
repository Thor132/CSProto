namespace CommandLineOptions
{
    using System;
    using CommandLine;
    using CommandLine.Text;

    internal class Options
    {
        [Option('r', "read", Required = true, HelpText = "Input file to be processed.")]
        public string InputFile { get; set; }

        // omitting long name, default --verbose
        [Option('v', "verbose", DefaultValue = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Args: " + string.Join(", ", args));

            CommandLine.Parser commandLineParser = new Parser(settings =>
            {
                settings.HelpWriter = Console.Error;
                settings.CaseSensitive = false;
                settings.IgnoreUnknownArguments = true;
            });

            var options = new Options();
            if (commandLineParser.ParseArguments(args, options))
            {
                Console.WriteLine("Filename: {0}", options.InputFile);
                // Values are available here
                if (options.Verbose) Console.WriteLine("[Verbose] Args: {0}", string.Join(",", args));
            }

            Console.ReadKey();
        }
    }
}