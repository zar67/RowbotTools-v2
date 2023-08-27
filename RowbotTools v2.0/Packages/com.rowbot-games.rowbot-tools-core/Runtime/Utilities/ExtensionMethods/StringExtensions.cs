namespace RowbotTools.Core.Extensions
{
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
    }
}