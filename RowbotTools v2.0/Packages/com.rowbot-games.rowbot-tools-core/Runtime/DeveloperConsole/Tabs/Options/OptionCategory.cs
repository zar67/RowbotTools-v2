namespace RowbotTools.Core.DeveloperConsole
{
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// A class for handling a single category in the options tab.
    /// </summary>
    public class OptionCategory : MonoBehaviour
    {
        [SerializeField] private Toggle m_collapseToggle;
        [SerializeField] private TextMeshProUGUI m_categoryTitleText;
        [SerializeField] private GameObject m_optionsHolder;
        [SerializeField] private Transform m_optionsParent;

        /// <summary>
        /// Reference to the parent of the options in this category.
        /// </summary>
        public Transform OptionsParent => m_optionsParent;

        /// <summary>
        /// The name of the category.
        /// </summary>
        public string Name => m_categoryTitleText.text;

        /// <summary>
        /// Sets the category title text.
        /// </summary>
        /// <param name="title">The title to set.</param>
        public void SetTitle(string title)
        {
            m_categoryTitleText.text = title;
        }

        private void OnEnable()
        {
            m_collapseToggle.onValueChanged.AddListener(HandleCollapseToggleClicked);
        }

        private void OnDisable()
        {
            m_collapseToggle.onValueChanged.RemoveListener(HandleCollapseToggleClicked);
        }

        private void HandleCollapseToggleClicked(bool isOn)
        {
            m_optionsHolder.SetActive(isOn);
        }
    }
}