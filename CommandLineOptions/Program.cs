
namespace CommandLineOptions
{
    using System;
    using CommandLine;
    using CommandLine.Text;

    class Options
    {
        [Option('r', "read", Required = true, HelpText = "Input file to be processed.")]
        public string InputFile { get; set; }

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


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Args: " + string.Join(", ", args));

            var options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                // Values are available here
                if (options.Verbose) Console.WriteLine("Filename: {0}", options.InputFile);
            }

            Console.ReadKey();
        }
    }
}
