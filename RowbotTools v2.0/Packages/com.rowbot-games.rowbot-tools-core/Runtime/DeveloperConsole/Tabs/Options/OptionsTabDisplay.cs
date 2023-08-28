namespace RowbotTools.Core.DeveloperConsole
{
    using RowbotTools.Core.Extensions;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Component for managing the display of the options tab in the developer console.
    /// </summary>
    public class OptionsTabDisplay : TabDisplay
    {
        [SerializeField] private Transform m_categoriesParent;
        [SerializeField] private OptionCategory m_optionCategoryPrefab;

        // Custom Inspector Values
        [HideInInspector] public bool ShowOptionTypes = false;
        [HideInInspector] public List<string> OptionTypesList = new List<string>();
        [HideInInspector] public List<Option> OptionPrefabsList = new List<Option>();

        private Dictionary<string, Option> m_optionsTypeMap = new Dictionary<string, Option>();
        private Dictionary<string, OptionCategory> m_optionsCategoryMap = new Dictionary<string, OptionCategory>();

        /// <summary>
        /// Adds an option to the options tab.
        /// </summary>
        /// <typeparam name="T">The type of option to add.</typeparam>
        /// <param name="category">The category identifier to add the option into.</param>
        /// <param name="optionData">The data to populate the option with.</param>
        public Option AddOption<T>(string category, T optionData) where T : OptionData
        {
            if (m_optionsTypeMap.ContainsKey(typeof(T).Name))
            {
                if (!m_optionsCategoryMap.ContainsKey(category))
                {
                    AddCategory(category);
                }

                var newOption = Instantiate(m_optionsTypeMap[typeof(T).Name], m_optionsCategoryMap[category].OptionsParent);
                newOption.UpdateData(optionData);

                return newOption;
            }
            else
            {
                Debug.LogWarning($"Developer Console Option Type {typeof(T).Name} does not have a prefab set. Cannot add option to console.");
                return null;
            }
        }

        /// <summary>
        /// Removes the given option from the options tab.
        /// </summary>
        /// <param name="option">The option to remove.</param>
        public void RemoveOption(Option option)
        {
            OptionCategory optionCategory = null;
            foreach (var category in m_optionsCategoryMap)
            {
                if (option.transform.IsChildOf(category.Value.OptionsParent))
                {
                    optionCategory = category.Value;
                    break;
                }
            }
            
            option.Cleanup();

            if (optionCategory == null)
            {
                Debug.LogError($"{option.name} is not part of any category, something has gone wrong here.");
                Destroy(option.gameObject);
                return;
            }

            Destroy(option.gameObject);

            if (optionCategory.OptionsParent.transform.childCount == 1)
            {
                m_optionsCategoryMap.Remove(optionCategory.Name);
                Destroy(optionCategory.gameObject);
            }
        }

        private void Awake()
        {
            m_optionsTypeMap = new Dictionary<string, Option>();
            for (int i = 0; i < OptionTypesList.Count; i++)
            {
                if (OptionPrefabsList[i] == null)
                {
                    Debug.LogWarning($"Developer Console Option Type {OptionTypesList[i]} does not have a prefab set. Errors will occur if this option is used.");
                }
                else
                {
                    m_optionsTypeMap.Add(OptionTypesList[i], OptionPrefabsList[i]);
                }
            }

            m_categoriesParent.DestroyAllChildren();
            m_optionsCategoryMap = new Dictionary<string, OptionCategory>();
            AddCategory("Default");
        }

        private void AddCategory(string category)
        {
            var newCategory = Instantiate(m_optionCategoryPrefab, m_categoriesParent);
            newCategory.SetTitle(category);

            m_optionsCategoryMap.Add(category, newCategory);
        }
    }
}