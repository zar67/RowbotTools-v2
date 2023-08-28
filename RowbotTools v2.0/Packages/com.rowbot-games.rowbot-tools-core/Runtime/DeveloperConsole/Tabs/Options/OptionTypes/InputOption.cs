namespace RowbotTools.Core.DeveloperConsole
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// An option with an input field that calls an action on submitted.
    /// </summary>
    public class InputOption : Option
    {
        [SerializeField] private TMP_InputField m_inputField;
        [SerializeField] private TextMeshProUGUI m_labelText;

        /// <summary>
        /// Updates the input option data.
        /// </summary>
        /// <typeparam name="T">The option data type, should be InputOptionData.</typeparam>
        /// <param name="optionData">The option data.</param>
        public override void UpdateData<T>(T optionData)
        {
            var inputData = optionData as InputOptionData;

            m_labelText.text = inputData.Label + ":";

            m_inputField.onSubmit.RemoveAllListeners();
            m_inputField.onSubmit.AddListener(inputData.OnInputSubmitted);

            m_inputField.onValueChanged.RemoveAllListeners();
            m_inputField.onValueChanged.AddListener(inputData.OnInputChanged);
        }
    }

    /// <summary>
    /// Class for storing the data needed to create a new InputOption.
    /// </summary>
    public class InputOptionData : OptionData
    {
        public string Label;
        public UnityAction<string> OnInputSubmitted;
        public UnityAction<string> OnInputChanged;
    }
}