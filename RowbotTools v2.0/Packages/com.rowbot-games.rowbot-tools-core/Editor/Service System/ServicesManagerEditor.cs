namespace RowbotTools.Core.ServiceSystem
{
    using RowbotTools.Core.Utilities;
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(ServicesManager))]
    public class ServicesManagerEditor : Editor
    {
        private List<Type> m_allServices = new List<Type>();
        private List<Type> m_allRowbotToolsServices = new List<Type>();
        private List<Type> m_allCustomServices = new List<Type>();

        public ServicesManagerEditor()
        {
            m_allServices = AssemblyUtilities.GetAllOfType<Service>();
            m_allRowbotToolsServices = AssemblyUtilities.GetAllOfTypeInNamespace<Service>(nameof(RowbotTools));
            m_allCustomServices = AssemblyUtilities.GetAllOfTypeExcludingNamespace<Service>(nameof(RowbotTools));

            ServicesManager.InitializeEnabledServices(m_allServices);
        }

        private void OnEnable()
        {
            List<Type> refreshedServices = AssemblyUtilities.GetAllOfType<Service>();

            foreach (Type service in m_allServices)
            {
                if (!refreshedServices.Contains(service))
                {
                    ServicesManager.DisableService(service);
                }
            }

            m_allRowbotToolsServices = AssemblyUtilities.GetAllOfTypeInNamespace<Service>(nameof(RowbotTools));
            m_allCustomServices = AssemblyUtilities.GetAllOfTypeExcludingNamespace<Service>(nameof(RowbotTools));
            m_allServices = refreshedServices;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
            EditorGUILayout.LabelField("Default Services", EditorStyles.boldLabel);

            bool guiPreviouslyEnabled = GUI.enabled;
            GUI.enabled = false;

            foreach (var service in m_allRowbotToolsServices)
            {
                EditorGUILayout.Toggle(service.Name, ServicesManager.IsServiceEnabled(service));
            }

            GUI.enabled = guiPreviouslyEnabled;

            EditorGUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
            EditorGUILayout.LabelField("Custom Services", EditorStyles.boldLabel);

            foreach (Type service in m_allCustomServices)
            {
                EditorGUI.BeginChangeCheck();
                bool enabled = EditorGUILayout.Toggle(service.Name, ServicesManager.IsServiceEnabled(service));
                if (EditorGUI.EndChangeCheck())
                {
                    if (enabled)
                    {
                        ServicesManager.EnableService(service);
                    }
                    else
                    {
                        ServicesManager.DisableService(service);
                    }
                }
            }
        }
    }
}