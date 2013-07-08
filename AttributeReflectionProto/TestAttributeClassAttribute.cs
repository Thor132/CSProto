// -----------------------------------------------------------------------
// <copyright file="TestAttributeClassAttribute.cs" company="N/A">
// (C) Travis Thornton 2013
// </copyright>
// -----------------------------------------------------------------------

namespace AttributeReflectionProto
{
    using System;

    /// <summary>
    /// Test attribute class.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class TestAttributeClassAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestAttributeClassAttribute"/> class.
        /// </summary>
        public TestAttributeClassAttribute()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestAttributeClassAttribute"/> class.
        /// </summary>
        /// <param name="name">Attribute's custom name.</param>
        public TestAttributeClassAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the custom name for the attribute.
        /// </summary>
        public string Name { get; set; }
    }
}