// This file was auto-generated
// Origin file: PYJsonSerialization\PYJsonSerialization.py
// using statements are not generated and will have to be manually added and verified.
namespace CSJsonSerialization
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public class TestPythonStructure
    {
        public TestPythonStructure()
        {
            this.ListOfStrings = new List<string>();
        }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Id")]
        [Description("The structure's id")]
        public int Id { get; set; }

        public List<string> ListOfStrings { get; set; }
    }
}