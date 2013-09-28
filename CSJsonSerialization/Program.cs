using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSHelperLibrary.Serialization;

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

            TestStructure cppData = JsonSerialization.DeserializeObjectFromFile<TestStructure>(string.Format("{0}\\cppjson.txt", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)));
            Console.WriteLine("Cpp data: Name={0} Id={1}", cppData.Name, cppData.Id);
        }
    }
}
