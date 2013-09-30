namespace GenerateCSStructures
{
    using System.Collections.Generic;
    using System.IO;

    public class ClassAttributes
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
    }

    public class PropertyAttributes
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Complex { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }

    class GeneratedClass
    {
        public GeneratedClass()
        {
            this.Properties = new List<PropertyAttributes>();
        }

        public string OriginFilename { get; set; }
        public ClassAttributes Attributes { get; set; }
        public List<PropertyAttributes> Properties { get; set; }
    }
}