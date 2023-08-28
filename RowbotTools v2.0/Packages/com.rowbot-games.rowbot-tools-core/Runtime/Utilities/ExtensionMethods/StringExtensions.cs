namespace RowbotTools.Core.Extensions
{
    using System.Text.RegularExpressions;
    using UnityEngine;

    /// <summary>
    /// A class for adding extension methods to transform components.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Copies the string to the clipboard.
        /// </summary>
        /// <param name="transform">The string to copy.</param>
        public static void CopyToClipboard(this string str)
        {
            GUIUtility.systemCopyBuffer = str;
        }

        /// <summary>
        /// Splits the given camel case string up into words with spaces between them.
        /// </summary>
        /// <param name="str">The string to spilt.</param>
        /// <returns>The split string.</returns>
        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }
    }
}