namespace RowbotTools.Core.Utilities
{
    using UnityEngine;

    /// <summary>
    /// Class for ensuring a GameObject doesn't get destroyed on load.
    /// </summary>
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}