// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="N/A">
// (C) Travis Thornton 2013
// </copyright>
// -----------------------------------------------------------------------

namespace AttributeReflectionProto
{
    using System;

    /// <summary>
    /// Main program class.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main program method.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        private static void Main(string[] args)
        {
            TestClassBase class1 = new TestClassA() { IntPropertyA = 1, IntPropertyB = 59, IntPropertyC = 10110 };
            TestClassBase class2 = new TestClassB() { StringPropertyA = "Class 2 string", FloatPropertyA = 99.981f };
            TestClassBase class3 = new TestClassC() { StringPropertyA = "Class 3 string", IntPropertyA = -42 };

            Console.WriteLine(class1.DataString);
            Console.WriteLine(class2.DataString);
            Console.WriteLine(class3.DataString);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}