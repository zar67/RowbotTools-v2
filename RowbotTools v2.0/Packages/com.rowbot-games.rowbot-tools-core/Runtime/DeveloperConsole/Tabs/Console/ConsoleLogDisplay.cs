namespace RowbotTools.Core.DeveloperConsole
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// A class for handling the display of a single log.
    /// </summary>
    public class ConsoleLogDisplay : MonoBehaviour
    {
        /// <summary>
        /// Event for when the log display is clicked, sends through the display that was clicked.
        /// </summary>
        public static event Action<ConsoleLogDisplay> OnLogDisplayClicked;

        /// <summary>
        /// A struct to hold information about the colors of each log type.
        /// </summary>
        [System.Serializable]
        public struct ColorsConfig
        {
            public Color LogColor;
            public Color WarningColor;
            public Color ErrorColor;
        }

        [SerializeField] private Button m_selectButton;

        [SerializeField] private Image m_backgroundImage;
        [SerializeField] private Image m_logTypeImage;
        [SerializeField] private TextMeshProUGUI m_logText;

        [SerializeField] private ColorsConfig m_logTypeImageColours;
        [SerializeField] private ColorsConfig m_backgroundColours;

        /// <summary>
        /// Reference to the log this isplay is currently showing.
        /// </summary>
        public ConsoleTabDisplay.Log Log { get; private set; }

        /// <summary>
        /// Populates the display with the given log information.
        /// </summary>
        /// <param name="logText">The text to display.</param>
        /// <param name="type">The type of log to show.</param>
        public void Populate(ConsoleTabDisplay.Log log)
        {
            Log = log;

            m_logText.text = log.LogText;

            switch (log.LogType)
            {
                case LogType.Warning:
                    m_logTypeImage.color = m_logTypeImageColours.WarningColor;
                    m_backgroundImage.color = m_backgroundColours.WarningColor;
                    break;
                case LogType.Error:
                case LogType.Assert:
                    m_logTypeImage.color = m_logTypeImageColours.ErrorColor;
                    m_backgroundImage.color = m_backgroundColours.ErrorColor;
                    break;
                default:
                    m_logTypeImage.color = m_logTypeImageColours.LogColor;
                    m_backgroundImage.color = m_backgroundColours.LogColor;
                    break;
            }
        }

        private void OnEnable()
        {
            m_selectButton.onClick.AddListener(HandleSelectButtonClicked);
        }

        private void OnDisable()
        {
            m_selectButton.onClick.RemoveListener(HandleSelectButtonClicked);
        }

        private void HandleSelectButtonClicked()
        {
            OnLogDisplayClicked?.Invoke(this);
        }
    }
}
