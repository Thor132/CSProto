// -----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="N/A">
// (C) Travis Thornton 2013
// </copyright>
// -----------------------------------------------------------------------

namespace CSHelperLibrary.Extensions
{
    /// <summary>
    /// String extensions holder class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// String extension to add quotes around the string.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>Quoted input string.</returns>
        public static string Quote(this string input)
        {
            return string.Format("\"{0}\"", input);
        }
    }
}