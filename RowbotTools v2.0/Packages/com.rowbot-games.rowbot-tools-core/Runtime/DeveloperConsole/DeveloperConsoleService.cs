namespace RowbotTools.Core.DeveloperConsole
{
    using RowbotTools.Core.ServiceSystem;
    using UnityEngine;

    /// <summary>
    /// A Service for managing the developer console instance.
    /// </summary>
    public class DeveloperConsoleService : Service
    {
        private DeveloperConsoleManager m_developerConsoleManager = null;
        private OptionsTabDisplay m_optionsTabDisplay = null;

        /// <summary>
        /// Setup of the developer console instance.
        /// </summary>
        public override void Init()
        {
            base.Init();

            m_developerConsoleManager = Object.FindObjectOfType<DeveloperConsoleManager>();
            if (m_developerConsoleManager == null)
            {
                Debug.LogError("Could not find DeveloperConsoleManager in DeveloperConsoleService init, make sure there is a DeveloperConsoleManager in the boot scene.");
                return;
            }

            m_optionsTabDisplay = m_developerConsoleManager.GetTab<OptionsTabDisplay>() as OptionsTabDisplay;
            if (m_optionsTabDisplay == null)
            {
                Debug.LogError("Could not find OptionsTabDisplay in DeveloperConsoleService init, make sure there is a OptionsTabDisplay in the boot scene.");
                return;
            }

            AddOption(new TextOptionData() { Text = "This is a test.", Label = "Test" });
            var toggleOption = AddOption(new ToggleOptionData() { Label = "Test Toggle", OnToggleChanged = (bool enabled) => Debug.Log(enabled) }, "Test");

            RemoveOption(toggleOption);
        }

        /// <summary>
        /// Adds an option to the options panel.
        /// </summary>
        /// <typeparam name="T">The type of option to add.</typeparam>
        /// <param name="category">The category identifier to add the option into.</param>
        /// <param name="optionData">The data to populate the option with.</param>
        public Option AddOption<T>(T optionData, string category = "Default") where T : OptionData
        {
            return m_optionsTabDisplay.AddOption(category, optionData);
        }

        public void RemoveOption(Option option)
        {
            m_optionsTabDisplay.RemoveOption(option);
        }
    }
}