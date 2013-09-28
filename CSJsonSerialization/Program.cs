using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSHelperLibrary.Serialization;
using System.IO;

namespace CSJsonSerialization
{
    public class TestStructure
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            TestStructure sampleData = new TestStructure() { Name = "C# Test", Id = 52 };
            JsonSerialization.SerializeObjectToFile(sampleData, string.Format("{0}\\csjson.txt", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)));

            FileInfo cppFile = new FileInfo(string.Format("{0}\\cppjson.txt", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)));
            if (cppFile.Exists)
            {
                TestStructure cppData = JsonSerialization.DeserializeObjectFromFile<TestStructure>(cppFile.FullName);
                Console.WriteLine("Cpp data: Name={0} Id={1}", cppData.Name, cppData.Id);
            }

            FileInfo pyFile = new FileInfo(string.Format("{0}\\pyjson.txt", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)));
            if (pyFile.Exists)
            {
                TestStructure pyData = JsonSerialization.DeserializeObjectFromFile<TestStructure>(pyFile.FullName);
                Console.WriteLine("Py data: Name={0} Id={1}", pyData.Name, pyData.Id);
            }
        }
    }
}
