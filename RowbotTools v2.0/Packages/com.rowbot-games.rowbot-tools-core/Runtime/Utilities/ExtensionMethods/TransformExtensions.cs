namespace RowbotTools.Core.Extensions
{
    using UnityEngine;

    /// <summary>
    /// A class for adding extension methods to transform components.
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        /// Destroy all the childen of the transform.
        /// </summary>
        /// <param name="transform">The transform to destroy the children of.</param>
        public static void DestroyAllChildren(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
        }
    }
}