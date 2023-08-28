namespace RowbotTools.Core.DeveloperConsole
{
    using UnityEngine;

    /// <summary>
    /// Base option class that options inherit from.
    /// </summary>
    public abstract class Option : MonoBehaviour
    {
        /// <summary>
        /// Updates the data in the Option.
        /// </summary>
        /// <typeparam name="T">The type of data for the option.</typeparam>
        /// <param name="optionData">The data.</param>
        public abstract void UpdateData<T>(T optionData) where T : OptionData;

        /// <summary>
        /// Cleanup the Option, mainly any listeners that should be cleared.
        /// </summary>
        public abstract void Cleanup();
    }

    /// <summary>
    /// Base class for the data needed to create a new option.
    /// Each option type should have it's own data derived from this struct.
    /// </summary>
    public abstract class OptionData
    {

    }
}