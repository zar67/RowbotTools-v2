namespace RowbotTool.Core.DeveloperConsole
{
    using RowbotTools.Core.DeveloperConsole;
    using RowbotTools.Core.Utilities;
    using System.Collections.Generic;
    using UnityEditor;

    /// <summary>
    /// Custom editor for populating the list of option types and their prefabs in the OptionsTabDisplay inspector.
    /// </summary>
    [CustomEditor(typeof(OptionsTabDisplay))]
    public class OptionsTabDisplayEditor : Editor
    {
        private SerializedProperty m_showOptionTypes;
        private SerializedProperty m_optionsTypeList;
        private SerializedProperty m_optionPrefabsList;

        private List<string> m_optionTypes;

        private void OnEnable()
        {
            var tabDisplay = (OptionsTabDisplay)target;

            m_showOptionTypes = serializedObject.FindProperty(nameof(tabDisplay.ShowOptionTypes));
            m_optionsTypeList = serializedObject.FindProperty(nameof(tabDisplay.OptionTypesList));
            m_optionPrefabsList = serializedObject.FindProperty(nameof(tabDisplay.OptionPrefabsList));

            m_optionTypes = AssemblyUtilities.GetAllNamesOfType<OptionData>();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();

            base.OnInspectorGUI();

            m_showOptionTypes.boolValue = EditorGUILayout.Foldout(m_showOptionTypes.boolValue, "Option Type Prefabs");
            if (m_showOptionTypes.boolValue)
            {
                if (m_optionTypes.Count != m_optionsTypeList.arraySize)
                {
                    CheckOptionTypes();
                }

                // Draw property fields
                for (int i = 0; i < m_optionsTypeList.arraySize; i++)
                {
                    var typeElement = m_optionsTypeList.GetArrayElementAtIndex(i);
                    var prefabElement = m_optionPrefabsList.GetArrayElementAtIndex(i);
                    prefabElement.objectReferenceValue = EditorGUILayout.ObjectField(typeElement.stringValue, prefabElement.objectReferenceValue, typeof(Option), false);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void CheckOptionTypes()
        {
            // Check that the properties has the right list, remove and add any as needed
            var typesToAdd = new List<string>();

            for (int i = m_optionsTypeList.arraySize - 1; i >= 0; i--)
            {
                var type = m_optionsTypeList.GetArrayElementAtIndex(i);

                if (m_optionTypes.Contains(type.stringValue))
                {
                    typesToAdd.Add(type.stringValue);
                }
                else
                {
                    m_optionsTypeList.DeleteArrayElementAtIndex(i);
                    m_optionPrefabsList.DeleteArrayElementAtIndex(i);
                }
            }

            foreach (var type in typesToAdd)
            {
                m_optionPrefabsList.arraySize++;
                m_optionsTypeList.arraySize++;

                m_optionsTypeList.GetArrayElementAtIndex(m_optionsTypeList.arraySize - 1).stringValue = type;
            }
        }
    }
}