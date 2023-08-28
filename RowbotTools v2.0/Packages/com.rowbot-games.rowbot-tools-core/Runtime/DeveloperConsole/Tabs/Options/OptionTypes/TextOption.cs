namespace RowbotTools.Core.DeveloperConsole
{
    using TMPro;
    using UnityEngine;

    /// <summary>
    /// An option with text that updates on an event call.
    /// </summary>
    public class TextOption : Option
    {
        [SerializeField] private TextMeshProUGUI m_labelText;
        [SerializeField] private TextMeshProUGUI m_mainText;

        /// <summary>
        /// Updates the text option data.
        /// </summary>
        /// <typeparam name="T">The option data type, should be TextOptionData.</typeparam>
        /// <param name="optionData">The option data.</param>
        public override void UpdateData<T>(T optionData)
        {
            var textData = optionData as TextOptionData;
            m_labelText.text = textData.Label + ":";
            m_mainText.text = textData.Text;
        }

        /// <summary>
        /// Cleans up the Option.
        /// </summary>
        public override void Cleanup()
        {

        }
    }

    /// <summary>
    /// Class for storing the data needed to create a new TextOption.
    /// </summary>
    public class TextOptionData : OptionData
    {
        public string Label;
        public string Text;
    }
}