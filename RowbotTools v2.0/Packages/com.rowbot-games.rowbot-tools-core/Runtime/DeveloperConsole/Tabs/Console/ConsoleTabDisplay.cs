namespace RowbotTools.Core.DeveloperConsole
{
    using RowbotTools.Core.Extensions;
    using RowbotTools.Core.Utilities;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    /// <summary>
    /// Component for managing the display of the console tab in the developer console.
    /// </summary>
    public class ConsoleTabDisplay : TabDisplay
    {
        /// <summary>
        /// Struct to hold the information about a single log.
        /// </summary>
        public struct Log
        {
            public string LogText;
            public string StackTrace;
            public LogType LogType;
        }

        [SerializeField] private Button m_clearAllLogsButton;

        [SerializeField] private Toggle m_logsEnabledToggle;
        [SerializeField] private Toggle m_warningsEnabledToggle;
        [SerializeField] private Toggle m_errorsEnabledToggle;

        [SerializeField] private TextMeshProUGUI m_numLogsText;
        [SerializeField] private TextMeshProUGUI m_numWarningsText;
        [SerializeField] private TextMeshProUGUI m_numErrorsText;

        [SerializeField] private Transform m_logDisplaysParent;
        [SerializeField] private ConsoleLogDisplay m_logDisplayPrefab;

        [SerializeField] private TextMeshProUGUI m_stackTraceText;
        [SerializeField] private Button m_copyStackTraceButton;
        [SerializeField] private Button m_clearSelectedButton;

        [SerializeField] private DisableAutoLayout m_disableAutoLayout;

        private List<Log> m_cachedLogs = new List<Log>();
        private Dictionary<LogType, int> m_numLogsByType = new Dictionary<LogType, int>();
        private Dictionary<LogType, List<ConsoleLogDisplay>> m_logDisplaysByType = new Dictionary<LogType, List<ConsoleLogDisplay>>();

        /// <summary>
        /// Populates the display with the list logs from Unity.
        /// </summary>
        public override void Select()
        {
            base.Select();
            Populate();
        }

        private void Awake()
        {
            Application.logMessageReceived += HandleLog;
            ConsoleLogDisplay.OnLogDisplayClicked += HandleLogDisplayClicked;

            m_clearAllLogsButton.onClick.AddListener(HandleClearAllLogsClicked);

            m_logsEnabledToggle.onValueChanged.AddListener((bool enabled) => HandleLogTypeEnabledChanged(LogType.Log, enabled));
            m_warningsEnabledToggle.onValueChanged.AddListener((bool enabled) => HandleLogTypeEnabledChanged(LogType.Warning, enabled));
            m_errorsEnabledToggle.onValueChanged.AddListener((bool enabled) => HandleLogTypeEnabledChanged(LogType.Error, enabled));

            m_copyStackTraceButton.onClick.AddListener(HandleCopyStackTraceClicked);
            m_clearSelectedButton.onClick.AddListener(HandleClearSelectedClicked);
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= HandleLog;
            ConsoleLogDisplay.OnLogDisplayClicked -= HandleLogDisplayClicked;

            m_clearAllLogsButton.onClick.RemoveListener(HandleClearAllLogsClicked);

            m_logsEnabledToggle.onValueChanged.RemoveAllListeners();
            m_warningsEnabledToggle.onValueChanged.RemoveAllListeners();
            m_errorsEnabledToggle.onValueChanged.RemoveAllListeners();

            m_copyStackTraceButton.onClick.RemoveListener(HandleCopyStackTraceClicked);
            m_clearSelectedButton.onClick.RemoveListener(HandleClearSelectedClicked);
        }

        private void HandleLog(string logText, string stackTrace, LogType type)
        {
            if (type == LogType.Assert)
            {
                type = LogType.Error;
            }

            var log = new Log()
            {
                LogText = logText,
                StackTrace = stackTrace,
                LogType = type
            };

            m_cachedLogs.Add(log);

            if (!m_numLogsByType.ContainsKey(type))
            {
                m_numLogsByType.Add(type, 0);
            }

            m_numLogsByType[type]++;

            m_numLogsText.text = m_numLogsByType.ContainsKey(LogType.Log) ? m_numLogsByType[LogType.Log].ToString(): "0";
            m_numWarningsText.text = m_numLogsByType.ContainsKey(LogType.Warning) ? m_numLogsByType[LogType.Warning].ToString() : "0";
            m_numErrorsText.text = m_numLogsByType.ContainsKey(LogType.Error) ? m_numLogsByType[LogType.Error].ToString() : "0";
        }

        private void HandleLogDisplayClicked(ConsoleLogDisplay display)
        {
            m_stackTraceText.text = $"{display.Log.LogText}\n\n{display.Log.StackTrace}";
        }

        private void HandleClearAllLogsClicked()
        {
            m_cachedLogs = new List<Log>();
            Populate();
        }

        private void HandleLogTypeEnabledChanged(LogType type, bool enabled)
        {
            if (m_logDisplaysByType.ContainsKey(type))
            {
                foreach (var logDisplay in m_logDisplaysByType[type])
                {
                    logDisplay.gameObject.SetActive(enabled);
                }

                m_disableAutoLayout.ForceLayoutRefresh();
            }
        }

        private void HandleCopyStackTraceClicked()
        {
            m_stackTraceText.text.CopyToClipboard();
        }

        private void HandleClearSelectedClicked()
        {
            EventSystem.current.SetSelectedGameObject(null);
            m_stackTraceText.text = string.Empty;
        }

        private void Populate()
        {
            m_logDisplaysParent.DestroyAllChildren();
            m_logDisplaysByType = new Dictionary<LogType, List<ConsoleLogDisplay>>();

            foreach (var log in m_cachedLogs)
            {
                // TODO: Instead of instantiating here, a pool system and an on demand loading scroll would be a better alternative.
                var newLogDisplay = Instantiate(m_logDisplayPrefab, m_logDisplaysParent);
                newLogDisplay.Populate(log);

                if (!m_logDisplaysByType.ContainsKey(log.LogType))
                {
                    m_logDisplaysByType.Add(log.LogType, new List<ConsoleLogDisplay>());
                }

                m_logDisplaysByType[log.LogType].Add(newLogDisplay);
            }

            m_disableAutoLayout.FindAutoLayoutComponents();
            HandleClearSelectedClicked();
        }
    }
}