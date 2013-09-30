namespace GenerateCSStructures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;

    class ClassGenerator
    {
        public ClassGenerator(string outputDirectory, string baseNamespace)
        {
            this.OutputDirectory = outputDirectory;
            this.BaseNamespace = baseNamespace;
            this.ParentClassName = string.Empty;
            this.Indent = 4;
            this.IndentCharacter = ' ';
        }

        public string OutputDirectory { get; private set; }
        public string BaseNamespace { get; private set; }
        public string ParentClassName { get; set; }
        public int Indent { get; set; }
        public char IndentCharacter { get; set; }

        /// <summary>
        /// Generates the files for the passed in information list.
        /// </summary>
        /// <param name="classInformationList">List of class information to generate files with.</param>
        public void GenerateFiles(List<GeneratedClass> classInformationList)
        {
            foreach (GeneratedClass classData in classInformationList)
            {
                this.CreateClass(classData);
            }
        }

        /// <summary>
        /// Creates the class from the passed in data.
        /// </summary>
        /// <param name="classData">Class data to create.</param>
        private void CreateClass(GeneratedClass classData)
        {
            if (string.IsNullOrWhiteSpace(classData.Attributes.Name) || !classData.Properties.Any())
            {
                return;
            }

            string fileDirectory = this.OutputDirectory;
            string fullNamespace = this.BaseNamespace;

            if (!string.IsNullOrWhiteSpace(classData.Attributes.Namespace))
            {
                fileDirectory += '\\' + classData.Attributes.Namespace.Replace('.', '\\');
                fullNamespace += '.' + classData.Attributes.Namespace;
            }

            DirectoryInfo directoryInfo;
            directoryInfo = new DirectoryInfo(fileDirectory);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            List<string> allocations = new List<string>();
            List<List<string>> propertyDefinitions = new List<List<string>>();

            this.GeneratePropertyStrings(classData, propertyDefinitions, allocations);

            if (propertyDefinitions.Any())
            {
                using (TextWriter writer = File.CreateText(directoryInfo.FullName + '\\' + classData.Attributes.Name + ".cs"))
                {
                    writer.WriteLine("// This file was auto-generated");
                    if (classData.OriginFilename != null)
                    {
                        writer.WriteLine("// Origin file: {0}", this.RemoveCommonDirectoriesFromStart(classData.OriginFilename, directoryInfo.FullName));
                    }

                    writer.WriteLine("// using statements are not generated and will have to be manually added and verified.");
                    writer.WriteLine("namespace {0}", fullNamespace);
                    writer.WriteLine("{");

                    // Simple using if the type is a list or dictionary.
                    if (classData.Properties.Any(p => p.Type.StartsWith("List<") || p.Type.StartsWith("Dictionary<")))
                    {
                        writer.WriteLine("{0}using System.Collections.Generic;", GetIndent(1));
                    }

                    // If any of the property have attributes that require component model, add the using.
                    if (classData.Properties.Any(p => !string.IsNullOrWhiteSpace(p.Description) || string.IsNullOrWhiteSpace(p.DisplayName)))
                    {
                        writer.WriteLine("{0}using System.ComponentModel;{1}", GetIndent(1), Environment.NewLine);
                    }

                    writer.WriteLine("{0}public class {1}{2}", GetIndent(1), classData.Attributes.Name, this.ParentClassName);
                    writer.WriteLine("{0}{{", GetIndent(1));

                    if (allocations.Any())
                    {
                        writer.WriteLine("{0}public {1}()", GetIndent(2), classData.Attributes.Name);
                        writer.WriteLine("{0}{{", GetIndent(2));
                        foreach (string initialize in allocations)
                        {
                            writer.WriteLine("{0}{1}", GetIndent(3), initialize);
                        }

                        writer.WriteLine("{0}}}{1}", GetIndent(2), Environment.NewLine);
                    }


                    for (int i = 0, max = propertyDefinitions.Count - 1; i <= max; ++i)
                    {
                        foreach (string line in propertyDefinitions[i])
                        {
                            writer.WriteLine("{0}{1}", GetIndent(2), line);
                        }

                        if (i != max)
                        {
                            writer.WriteLine();
                        }
                    }

                    writer.WriteLine("{0}}}", GetIndent(1));
                    writer.Write("}");
                }
            }
        }

        /// <summary>
        /// Generates property and allocation strings.
        /// </summary>
        /// <param name="classData">Class data to utilize.</param>
        /// <param name="propertyDefinitionStrings">List of a List of strings for the property definitions and attributes.</param>
        /// <param name="complexAllocationStrings">List of allocation strings to be used in the constructor.</param>
        private void GeneratePropertyStrings(GeneratedClass classData, List<List<string>> propertyDefinitionStrings, List<string> complexAllocationStrings)
        {
            foreach (PropertyAttributes property in classData.Properties)
            {
                if (string.IsNullOrWhiteSpace(property.Name) || string.IsNullOrWhiteSpace(property.Type))
                {
                    continue;
                }

                if (property.Complex)
                {
                    complexAllocationStrings.Add(string.Format("this.{0} = new {1}();", property.Name, property.Type));
                }

                List<string> data = new List<string>();
                if (!string.IsNullOrWhiteSpace(property.DisplayName))
                {
                    data.Add(string.Format("[DisplayName(\"{0}\")]", property.DisplayName));
                }

                if (!string.IsNullOrWhiteSpace(property.Description))
                {
                    data.Add(string.Format("[Description(\"{0}\")]", property.Description));
                }

                data.Add(string.Format("public {0} {1} {{ get; set; }}", property.Type, property.Name));

                propertyDefinitionStrings.Add(data);
            }
        }

        /// <summary>
        /// Gets the indent level string.
        /// </summary>
        /// <param name="level">Indent level.</param>
        /// <returns>String of the indent.</returns>
        private string GetIndent(int level)
        {
            return String.Concat(Enumerable.Repeat(this.IndentCharacter, this.Indent * level));
        }

        /// <summary>
        /// Removes directories from the start of the first input path if they match in the second path.
        /// The returned result will be the remainder of the path when there is a deviation.
        /// </summary>
        /// <param name="editString">Master path that is being edited.</param>
        /// <param name="referenceString">Reference path to use for determining the common characters.</param>
        /// <returns></returns>
        private string RemoveCommonDirectoriesFromStart(string editString, string referenceString)
        {
            //for (int i = 0; i < editString.Length && i < referenceString.Length; ++i)
            //{
            //    if (editString[i] == referenceString[i])
            //    {
            //        continue;
            //    }

            //    return editString.Remove(0, i);
            //}

            //return editString;

            List<string> editList = editString.Split('\\').ToList();
            List<string> referenceList = referenceString.Split('\\').ToList();

            for (int i = 0; i < editList.Count && i < referenceList.Count; ++i)
            {
                if (editList[i] == referenceList[i])
                {
                    continue;
                }

                editList.RemoveRange(0, i);
                return string.Join("\\", editList);
            }

            return editString;
        }
    }
}