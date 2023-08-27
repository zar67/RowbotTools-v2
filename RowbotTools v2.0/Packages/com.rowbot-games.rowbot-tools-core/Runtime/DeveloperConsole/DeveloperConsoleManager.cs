namespace RowbotTools.Core.DeveloperConsole
{
    using RowbotTools.Core.Utilities;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Component for managing the display of the developer console.
    /// </summary>
    [RequireComponent(typeof(DontDestroyOnLoad))]
    public class DeveloperConsoleManager : MonoBehaviour
    {
        [SerializeField] private Button m_openButton;
        [SerializeField] private Button m_closeButton;
        [SerializeField] private GameObject m_developerConsoleHolder;
        [SerializeField] private TextMeshProUGUI m_contentTitle;
        [SerializeField] private TabDisplay[] m_tabs;
        [SerializeField] private GameObject[] m_titleButtons;
        [SerializeField] private DisableAutoLayout m_disableAutoLayout;

        private TabDisplay m_selectedTab;

        private void OnEnable()
        {
            m_openButton.onClick.AddListener(HandleOpenClicked);
            m_closeButton.onClick.AddListener(HandleCloseClicked);

            foreach (var tab in m_tabs)
            {
                var cachedTab = tab;
                cachedTab.TabButton.onClick.AddListener(() => HandleTabClicked(cachedTab));
            }

            HandleCloseClicked();
        }

        private void OnDisable()
        {
            m_openButton.onClick.RemoveListener(HandleOpenClicked);
            m_closeButton.onClick.RemoveListener(HandleCloseClicked);

            foreach (var tab in m_tabs)
            {
                tab.TabButton.onClick.RemoveAllListeners();
            }
        }

        private void HandleOpenClicked()
        {
            m_openButton.interactable = false;
            m_developerConsoleHolder.SetActive(true);

            foreach (var tab in m_tabs)
            {
                tab.OnConsoleOpen();
            }

            HandleTabClicked(m_tabs[0]);
        }

        private void HandleCloseClicked()
        {
            foreach (var tab in m_tabs)
            {
                tab.OnConsoleClose();
            }

            m_developerConsoleHolder.SetActive(false);
            m_openButton.interactable = true;
        }

        private void HandleTabClicked(TabDisplay selectedTab)
        {
            if (m_selectedTab != null)
            {
                m_selectedTab.Deselect();
            }

            m_selectedTab = selectedTab;
            m_selectedTab.Select();

            m_contentTitle.text = selectedTab.Name;

            foreach (var button in m_titleButtons)
            {
                button.SetActive(selectedTab.IsTitleButtonEnabled(button));
            }

            m_disableAutoLayout.ForceLayoutRefresh();
        }
    }
}