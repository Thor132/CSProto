namespace GenerateCSStructures
{
    using System;
    using System.IO;

    class Program
    {
        static void Main(string[] args)
        {
            var options = new CommandLineOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                DirectoryInfo directory = new DirectoryInfo(options.InputDirectory);
                if (directory.Exists)
                {
                    ClassParser parser = new ClassParser();
                    parser.ParseDirectory(directory);

                    Console.WriteLine("Generating {0} classes.", parser.ClassInformationList.Count);

                    ClassGenerator generator = new ClassGenerator(options.OutputDirectory, options.BaseNamespace);
                    generator.GenerateFiles(parser.ClassInformationList);
                }
            }
        }
    }
}