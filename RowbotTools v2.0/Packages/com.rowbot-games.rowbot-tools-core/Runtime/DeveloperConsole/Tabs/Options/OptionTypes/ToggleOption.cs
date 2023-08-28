namespace RowbotTools.Core.DeveloperConsole
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    /// <summary>
    /// An option with a toggle that triggers an action when toggled.
    /// </summary>
    public class ToggleOption : Option
    {
        [SerializeField] private Toggle m_toggle;
        [SerializeField] private TextMeshProUGUI m_labelText;

        /// <summary>
        /// Updates the toggle option data.
        /// </summary>
        /// <typeparam name="T">The option data type, should be ToggleOptionData.</typeparam>
        /// <param name="optionData">The option data.</param>
        public override void UpdateData<T>(T optionData)
        {
            var toggleData = optionData as ToggleOptionData;

            m_labelText.text = toggleData.Label + ":";

            m_toggle.onValueChanged.RemoveAllListeners();
            m_toggle.onValueChanged.AddListener(toggleData.OnToggleChanged);
        }
    }

    /// <summary>
    /// Class for storing the data needed to create a new ToggleOption.
    /// </summary>
    public class ToggleOptionData : OptionData
    {
        public string Label;
        public UnityAction<bool> OnToggleChanged;
    }
}