// This file was auto-generated
// Origin file: CPPJsonSerialization\TestCppStructure.h
// using statements are not generated and will have to be manually added and verified.
namespace CSJsonSerialization
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public class TestCppStructure
    {
        public TestCppStructure()
        {
            this.ListOfInts = new List<int>();
        }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Id")]
        [Description("The structure's id")]
        public int Id { get; set; }

        public List<int> ListOfInts { get; set; }
    }
}