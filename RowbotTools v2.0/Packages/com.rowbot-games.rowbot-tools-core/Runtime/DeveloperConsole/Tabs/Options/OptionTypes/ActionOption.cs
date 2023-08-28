namespace RowbotTools.Core.DeveloperConsole
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    /// <summary>
    /// An option with a button that triggers an action when clicked.
    /// </summary>
    public class ActionOption : Option
    {
        [SerializeField] private Button m_selectButton;
        [SerializeField] private TextMeshProUGUI m_buttonText;

        /// <summary>
        /// Updates the action option data.
        /// </summary>
        /// <typeparam name="T">The option data type, should be ActionOptionData.</typeparam>
        /// <param name="optionData">The option data.</param>
        public override void UpdateData<T>(T optionData)
        {
            var actionData = optionData as ActionOptionData;

            m_selectButton.onClick.RemoveAllListeners();
            m_selectButton.onClick.AddListener(actionData.OnAction);
        }
    }

    /// <summary>
    /// Class for storing the data needed to create a new ActionOption.
    /// </summary>
    public class ActionOptionData : OptionData
    {
        public UnityAction OnAction;
    }
}