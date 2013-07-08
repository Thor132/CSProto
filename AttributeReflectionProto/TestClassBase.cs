// -----------------------------------------------------------------------
// <copyright file="TestClassBase.cs" company="N/A">
// (C) Travis Thornton 2013
// </copyright>
// -----------------------------------------------------------------------

namespace AttributeReflectionProto
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Base class for testing.
    /// </summary>
    public abstract class TestClassBase
    {
        /// <summary>
        /// Gets the formatted data of the class.
        /// </summary>
        public string DataString
        {
            get
            {
                return string.Format("{0} {{ {1} }}", this.GetType().Name, this.GetPropertyDataString());
            }
        }

        /// <summary>
        /// Finds all of the properties with the <see cref="TestAttributeClassAttribute"/> type and adds them and their value to a comma space delimited string.
        /// </summary>
        /// <returns>Comma space delimited string with defined properties.</returns>
        private string GetPropertyDataString()
        {
            List<string> customPropertyStrings = new List<string>();
            foreach (PropertyInfo property in this.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(TestAttributeClassAttribute))))
            {
                TestAttributeClassAttribute customAttribute = property.GetCustomAttributes(true).Where(p => p is TestAttributeClassAttribute).FirstOrDefault() as TestAttributeClassAttribute;

                string value = string.Empty;
                if (property.PropertyType.IsAssignableFrom(typeof(string)))
                {
                    value = string.Format("\"{0}\"", property.GetValue(this, null));
                }
                else
                {
                    value = string.Format("{0}", property.GetValue(this, null));
                }

                customPropertyStrings.Add(string.Format("{0}={1}", customAttribute != null && customAttribute.Name != null ? customAttribute.Name : property.Name, value));
            }

            return string.Join(", ", customPropertyStrings);
        }
    }
}