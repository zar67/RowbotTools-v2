namespace RowbotTools.Core.DeveloperConsole
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Base class for managing a tab display in the developer console.
    /// </summary>
    public abstract class TabDisplay : MonoBehaviour
    {
        [SerializeField] private string m_name;
        [SerializeField] private Button m_tabButton;
        [SerializeField] private GameObject m_contentHolder;
        [SerializeField] private List<GameObject> m_enabledTitleButtons;

        /// <summary>
        /// Reference to the display name of the tab.
        /// </summary>
        public string Name => m_name;

        /// <summary>
        /// Reference to the tab button.
        /// </summary>
        public Button TabButton => m_tabButton;

        /// <summary>
        /// Gets whether the given gameobject is in the list of enabled title buttons for this tab.
        /// </summary>
        /// <param name="button">The gameobject to check.</param>
        /// <returns>True if gameobject is in list of enabled title buttons, false if not.</returns>
        public bool IsTitleButtonEnabled(GameObject button)
        {
            return m_enabledTitleButtons.Contains(button);
        }

        /// <summary>
        /// Called when the developer console is opened.
        /// </summary>
        public virtual void OnConsoleOpen()
        {
            m_contentHolder.SetActive(false);
        }

        /// <summary>
        /// Called when the developer console is closed.
        /// </summary>
        public virtual void OnConsoleClose()
        {
        
        }

        /// <summary>
        /// Called when the tab is selected.
        /// </summary>
        public virtual void Select()
        {
            m_contentHolder.SetActive(true);
            m_tabButton.interactable = false;
        }

        /// <summary>
        /// Called when the tab is deselected (another tab is selected).
        /// </summary>
        public virtual void Deselect()
        {
            m_contentHolder.SetActive(false);
            m_tabButton.interactable = true;
        }
    }
}