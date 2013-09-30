namespace GenerateCSStructures
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using CSHelperLibrary.Serialization;

    class ClassParser
    {
        public ClassParser()
        {
            this.ClassInformationList = new List<GeneratedClass>();
        }

        public List<GeneratedClass> ClassInformationList { get; private set; }

        public void ParseDirectory(DirectoryInfo directory)
        {
            foreach (FileInfo file in directory.EnumerateFiles("*.h", SearchOption.AllDirectories))
            {
                this.ParseFile(file, directory);
            }

            foreach (FileInfo file in directory.EnumerateFiles("*.py", SearchOption.AllDirectories))
            {
                this.ParseFile(file, directory);
            }
        }

        private void ParseFile(FileInfo file, DirectoryInfo baseDirectory)
        {
            if (!file.Exists)
            {
                return;
            }

            Regex classHeaderRegex = new Regex(@"^.*?\[GenerateClass(?:\((.*)\))?\].*");
            Regex memberHeaderRegex = new Regex(@"^.*?\[GenerateProperty(?:\((.*)\))?\].*");

            GeneratedClass readingClass = null;

            using (TextReader reader = File.OpenText(file.FullName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Match classHeaderMatch = classHeaderRegex.Match(line);
                    if (classHeaderMatch.Success && classHeaderMatch.Groups.Count == 2)
                    {
                        readingClass = new GeneratedClass() { OriginFilename = file.FullName };
                        readingClass.Attributes = TryParseAttributes<ClassAttributes>(classHeaderMatch.Groups[1].Value);
                        ClassInformationList.Add(readingClass);
                    }

                    if (readingClass != null)
                    {
                        Match memberHeaderMatch = memberHeaderRegex.Match(line);
                        if (memberHeaderMatch.Success && memberHeaderMatch.Groups.Count == 2)
                        {
                            readingClass.Properties.Add(TryParseAttributes<PropertyAttributes>(memberHeaderMatch.Groups[1].Value));
                        }
                    }
                }
            }
        }

        private T TryParseAttributes<T>(string input)
        {
            string modifiedInput = string.Format("{0},", input);

            // TODO: attributes all need capital letters for the first letter.
            // Removes quotes: (?:\s*(\w*)(?:\s*=\s*\""?(.*?)\""?\s*)?,\s*(?=[A-Z]|$))
            Regex re = new Regex(@"(?:\s*(\w*)(?:\s*=\s*\""?(.*?)\""?\s*)?,\s*(?=[A-Z]|$))");
            MatchCollection mc = re.Matches(modifiedInput);
            if (mc.Count == 0)
            {
                return default(T);
            }

            List<string> options = new List<string>();
            foreach (Match match in mc)
            {
                if (match.Success && match.Groups.Count == 3 && match.Groups[1].Success)
                {
                    string key = match.Groups[1].Value;
                    string value;
                    if (match.Groups[2].Success)
                    {
                        value = match.Groups[2].Value;
                    }
                    else
                    {
                        value = "true";
                    }

                    if (!string.IsNullOrWhiteSpace(key))
                    {
                        options.Add(string.Format("\"{0}\":\"{1}\"", key, value));
                    }
                }
            }

            Type type = typeof(T);
            string jsonAttributes = string.Format("{{\r\n  \"$type\": \"{0}, {1}\",\r\n {2} \r\n}}", type.FullName, type.Assembly.GetName().Name, string.Join(", ", options));

            // TODO: invalid data type exception
            return JsonSerialization.DeserializeObjectFromString<T>(jsonAttributes);
        }
    }
}
